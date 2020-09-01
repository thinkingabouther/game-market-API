using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using game_market_API.DTOs;
using game_market_API.ExceptionHandling;
using game_market_API.Models;
using game_market_API.Utilities;
using game_market_API.ViewModels;
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
        private readonly IMapper _mapper;

        public PaymentSessionService(GameMarketDbContext context, IConfiguration configuration, IAcquiringService acquiringService, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _acquiringService = acquiringService;
            _mapper = mapper;
        }

        public async Task<PaymentSessionViewModel> PreparePaymentSession(string clientUserName, PurchaseDto purchaseDto)
        {
            var client = _context.Users.Single(c => c.Username == clientUserName);
            var availableKeys = LoadAvailableKeys(clientUserName, purchaseDto);
            var paymentSession = new PaymentSession
            {
                Date = DateTime.Now,
                GameKeys = new List<GameKey>(),
                Client = client
            };
            for (int i = 0; i < purchaseDto.GameCount; i++)
            {
                AttachKeyToSession(paymentSession, availableKeys[i]);
            }
            _context.PaymentSessions.Add(paymentSession);
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentSessionViewModel>(paymentSession);
        }
        
        

        private void AttachKeyToSession(PaymentSession session, GameKey key)
        {
            session.GameKeys.Add(key);
            key.IsActivated = true;
        }

        private GameKey[] LoadAvailableKeys(string clientUserName, PurchaseDto purchaseDto)
        {
            var game = _context.Games.Include("GameKeys").Single(g => g.ID == purchaseDto.GameID);
            if (game == null) throw new ItemNotFoundException();
            var availableKeys = game.GameKeys.Where(gameKey => !gameKey.IsActivated);
            var gameKeysArray = availableKeys.ToArray();
            
            if (gameKeysArray.Length < purchaseDto.GameCount) 
                throw new NotEnoughKeysException();
            return gameKeysArray;
        }

        public async Task<PaymentSessionViewModel> PerformPayment(string clientUserName, int sessionID, PaymentDto paymentDto)
        {
            var isValid = ValidateCardNumber(paymentDto.CardNumber);
            var session = await LoadSession(clientUserName, sessionID, paymentDto);
            var marketShare = double.Parse(_configuration.GetSection("Settings").GetSection("MarketShare").Value);
            var sum = session.GameKeys.Sum(key => key.Game.Price);
            if (!await isValid) throw new InvalidCardException();
            
            await _acquiringService.CreditMarket(paymentDto.CardNumber, sum * marketShare);
            await _acquiringService.CreditVendor(paymentDto.CardNumber, sum * (1 - marketShare));
            session.IsCompleted = true;
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentSessionViewModel>(session);
        }

        public async Task<PaymentSession> LoadSession(string clientUserName, int sessionID, PaymentDto paymentDto)
        {
            var session = _context.PaymentSessions.Find(sessionID);
            if (session == null) throw new ItemNotFoundException();
            await Task.Run(() => session = _context.PaymentSessions
                .Include(s => s.Client)
                .Include(s => s.GameKeys)
                .ThenInclude(g => g.Game)
                .ThenInclude(g => g.Vendor)
                .Single(s => s.ID == sessionID));
            if (session.Client.Username != clientUserName) throw new WrongClientException();
            if (session.IsCompleted) throw new SessionCompletedException();
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
}