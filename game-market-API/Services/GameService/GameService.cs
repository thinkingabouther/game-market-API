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
    public class GameService : IGameService
    {
        private readonly GameMarketDbContext _context;

        public GameService(GameMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetGamesAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game> GetGameAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) throw new ItemNotFoundException();
            return game;
        }
        
        
        public async Task PatchGameAsync(string vendorUserName, int id, GameDto game)
        {
            var gameInDB = _context.Games.Include("Vendor").Single(game => game.ID == id);
            if (gameInDB.Vendor.Username != vendorUserName) throw new WrongVendorException();
            gameInDB.Name = game.Name;
            gameInDB.Price = game.Price;
            _context.Entry(gameInDB).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    throw new ItemNotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }
        
        
        public async Task<Game> PostGameAsync(string vendorUserName, Game game)
        {
            var vendor = _context.Users.Single(u => u.Username == vendorUserName);
            game.Vendor = vendor;
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game> DeleteGame(string vendorUserName, int id)
        {
            var game = _context.Games.Include("Vendor").Single(game => game.ID == id);
            if (game == null)
            {
                throw new ItemNotFoundException();
            }
            if (game.Vendor.Username != vendorUserName) throw new WrongVendorException();
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<PaymentSession> PreparePaymentSession(string clientUserName, PurchaseDto purchaseRequest)
        {
            var client = _context.Users.Single(c => c.Username == clientUserName);
            var game = _context.Games.Find(purchaseRequest.GameID);
            var availableKeys = game.GameKeys.Where(gameKey => gameKey.IsActivated);
            var gameKeysArray = availableKeys as GameKey[] ?? availableKeys.ToArray();
            if (gameKeysArray.Length < purchaseRequest.GameCount) 
                throw new NotEnoughKeysException();
            var paymentSession = new PaymentSession
            {
                Date = DateTime.Now
            };
            for (int i = 0; i < purchaseRequest.GameCount; i++)
            {
                paymentSession.GameKeys.Add(gameKeysArray[i]);
                gameKeysArray[i].IsActivated = true;
                gameKeysArray[i].Client = client;
            }
            _context.PaymentSessions.Add(paymentSession);
            await _context.SaveChangesAsync();
            return paymentSession;
        }
        
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.ID == id);
        }
    }

    public class NotEnoughKeysException : Exception
    {
        
    }
}