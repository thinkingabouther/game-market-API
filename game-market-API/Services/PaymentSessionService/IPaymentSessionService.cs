using System.Collections.Generic;
using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game_market_API.Services
{
    public interface IPaymentSessionService
    {
        public Task<PaymentSession> PreparePaymentSession(string clientUserName, PurchaseDto purchaseDto);
        public Task<PaymentSession> PerformPayment(PaymentDto paymentDto);

    }
}