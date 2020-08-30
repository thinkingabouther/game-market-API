using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        
        public async Task PutGameKeyAsync(int id, GameKey gameKey)
        {
            
            _context.Entry(gameKey).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameKeyExists(id))
                {
                    throw new ItemNotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }
        
        
        public async Task<GameKey> PostGameKeyAsync(GameKey gameKey)
        {
            _context.GameKeys.Add(gameKey);
            await _context.SaveChangesAsync();
            return gameKey;
        }

        public async Task<ActionResult<GameKey>> DeleteGameKey(int id)
        {
            var gameKey = await _context.GameKeys.FindAsync(id);
            if (gameKey == null)
            {
                throw new ItemNotFoundException();
            }
            _context.GameKeys.Remove(gameKey);
            await _context.SaveChangesAsync();

            return gameKey;
        }

        private bool GameKeyExists(int id)
        {
            return _context.GameKeys.Any(e => e.ID == id);
        }
    }
}