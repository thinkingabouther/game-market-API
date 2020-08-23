using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace game_market_API.Security
{
    public class AuthOptions
    {
        public const string ISSUER = "AuthServer"; 
        public const string AUDIENCE = "AuthClient";
        const string KEY = "123123123123123123123123123123";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}