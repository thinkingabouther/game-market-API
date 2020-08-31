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
    public class AcquiringService : IAcquiringService
    {
        public AcquiringService() {}
        public async Task CreditMarket(string cardNumber, double sum)
        {
            // Some magic happens here
            await Task.Delay(1000);
            Console.WriteLine("Market was credited");
        }

        public async Task CreditVendor(string cardNumber, double sum)
        {
            // Some magic happens here
            await Task.Delay(1000);
            Console.WriteLine("Vendor was credited");
        }
    }
}