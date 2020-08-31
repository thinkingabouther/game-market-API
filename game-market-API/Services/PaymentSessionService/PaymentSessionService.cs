using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace game_market_API.Services
{
    public class PaymentSessionService : IPaymentSessionService
    {
        private readonly GameMarketDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAcquiringService _acquiringService;

        public PaymentSessionService(GameMarketDbContext context, IConfiguration configuration, IAcquiringService acquiringService)
        {
            _context = context;
            _configuration = configuration;
            _acquiringService = acquiringService;
        }

        public async Task<PaymentSession> PreparePaymentSession(string clientUserName, PurchaseDto purchaseDto)
        {
            var client = _context.Users.Single(c => c.Username == clientUserName);
            var game = _context.Games.Include("GameKeys").Single(g => g.ID == purchaseDto.GameID);
            if (game == null) throw new ItemNotFoundException();
            var availableKeys = game.GameKeys.Where(gameKey => !gameKey.IsActivated);
            var gameKeysArray = availableKeys.ToArray();
            
            if (gameKeysArray.Length < purchaseDto.GameCount) 
                throw new NotEnoughKeysException();
            
            var paymentSession = new PaymentSession
            {
                Date = DateTime.Now,
                GameKeys = new List<GameKey>(),
                Client = client
            };
            for (int i = 0; i < purchaseDto.GameCount; i++)
            {
                paymentSession.GameKeys.Add(gameKeysArray[i]);
                gameKeysArray[i].IsActivated = true;
            }
            _context.PaymentSessions.Add(paymentSession);
            await _context.SaveChangesAsync();
            return paymentSession;
        }

        public async Task<PaymentSession> PerformPayment(string clientUserName, PaymentDto paymentDto)
        {
            
            var isValid = ValidateCardNumber(paymentDto.CardNumber);
            var session = _context.PaymentSessions
                .Include(s => s.Client)
                .Include(s => s.GameKeys)
                .ThenInclude(g => g.Game)
                .Single(s => s.ID == paymentDto.SessionID);
            if (session == null) throw new ItemNotFoundException();
            if (session.Client.Username != clientUserName) throw new WrongClientException();
            if (session.IsCompleted) throw new SessionCompletedException();
            
            var marketShare = double.Parse(_configuration.GetSection("Settings").GetSection("MarketShare").Value);
            var sum = session.GameKeys.Sum(key => key.Game.Price);
            if (!await isValid) throw new InvalidCardException();
            
            await _acquiringService.CreditMarket(paymentDto.CardNumber, sum * marketShare);
            await _acquiringService.CreditVendor(paymentDto.CardNumber, sum * (1 - marketShare));
            session.IsCompleted = true;
            await _context.SaveChangesAsync();
            return session;
        }

        private async Task<bool> ValidateCardNumber(string cardNumber)
        {
            if (cardNumber == String.Empty || !cardNumber.All(char.IsDigit)) return false;
            int sumOfDigits = await Task.Run(() => cardNumber.Where(e => e >= '0' && e <= '9')
                .Reverse()
                .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                .Sum(e => e / 10 + e % 10));
            return sumOfDigits % 10 == 0;
        }
    }

    public class NotEnoughKeysException : Exception
    {
        
    }

    public class InvalidCardException : Exception
    {
        
    }

    public class SessionCompletedException : Exception
    {
        
    }

    public class WrongClientException : Exception
    {
        
    }
}