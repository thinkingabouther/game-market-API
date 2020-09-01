using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.ExceptionHandling;
using game_market_API.Models;
using game_market_API.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace game_market_API.Services.ClientService
{
    public class UserService : IUserService
    {
        private readonly GameMarketDbContext _context;

        public UserService(GameMarketDbContext context)
        {
            _context = context;
        }

        public async Task<User> PostUser(UserCredentialsDto credentials)
        {
            var user = new User
            {
                Password = credentials.Password,
                Username = credentials.UserName,
                Role = credentials.IsVendor ? User.VendorRole : User.ClientRole
            };
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new UsernameUnavailableException();
            }
            return user;
        }

        public async Task<string> GetToken(User credentials)
        {
            var identity = await GetIdentity(credentials);
            if (identity == null)
            {
                throw new UserNotFoundException();
            }
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private async Task<ClaimsIdentity> GetIdentity(User credentials)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == credentials.Username
                                                                 && x.Password == credentials.Password);
            if (currentUser != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, currentUser.Role.ToString())
                }; 
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}