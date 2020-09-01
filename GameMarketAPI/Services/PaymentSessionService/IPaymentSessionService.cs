using System.Collections.Generic;
using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game_market_API.Services
{
    public interface IPaymentSessionService
    {
        public Task<PaymentSessionViewModel> PreparePaymentSession(string clientUserName, PurchaseDto purchaseDto);
        public Task<PaymentSessionViewModel> PerformPayment(string clientUserName, int sessionID, PaymentDto paymentDto);
        public Task<PaymentSession> LoadSession(string clientUserName, int sessionID, PaymentDto paymentDto);
    }
}