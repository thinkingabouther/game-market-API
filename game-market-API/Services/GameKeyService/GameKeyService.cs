using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game_market_API.Services
{
    public class GameKeyService : IGameKeyService
    {
        private readonly GameMarketDbContext _context;

        public GameKeyService(GameMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameKey>> GetGameKeysAsync()
        {
            return await _context.GameKeys.ToListAsync();
        }

        public async Task<GameKey> GetGameKeyAsync(int id)
        {
            var gameKey = await _context.GameKeys.FindAsync(id);
            if (gameKey == null) throw new ItemNotFoundException();
            return gameKey;
        }

        public async Task<GameKey> PostGameKeyAsync(string vendorUserName, GameKeyRequest gameKeyRequest)
        {
            var gameKey = MapToEntity(vendorUserName, gameKeyRequest);
            _context.GameKeys.Add(gameKey);
            await _context.SaveChangesAsync();
            return gameKey;
        }

        public async Task<ActionResult<GameKey>> DeleteGameKey(string vendorUserName, int id)
        {
            var gameKey = await _context.GameKeys.FindAsync(id);
            if (gameKey == null) throw new ItemNotFoundException();
            if (gameKey.Vendor.Username != vendorUserName) throw new WrongVendorException();
            _context.GameKeys.Remove(gameKey);
            await _context.SaveChangesAsync();
            return gameKey;
        }

        private GameKey MapToEntity(string vendorUserName, GameKeyRequest request)
        {
            var game = _context.Games.Find(request.GameID);
            if (game == null) throw new ItemNotFoundException();
            var vendor = _context.Users.Single(user => user.Username == vendorUserName);
            return new GameKey
            {
                Key = request.Key,
                Game = game,
                Vendor = vendor
            };
        }
        
        public class WrongVendorException : Exception {}
    }
}