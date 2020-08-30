using System.Collections.Generic;
using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game_market_API.Services
{
    public interface IGameService
    {
        public Task<IEnumerable<Game>> GetGamesAsync();

        public Task<Game> GetGameAsync(int id);

        public Task PatchGameAsync(string vendorUserName, int id, GameDto game);
        
        public  Task<Game> PostGameAsync(string vendorUserName, Game game);

        public Task<Game> DeleteGame(string vendorUserName, int id);
        
        public Task<PaymentSession> PreparePaymentSession(string clientUserName, PurchaseDto purchaseRequest);
    }
}