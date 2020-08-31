using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace game_market_API.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentSessionService _paymentSessionService;

        public PaymentController(IPaymentSessionService paymentSessionService)
        {
            _paymentSessionService = paymentSessionService;
        }

        // POST: api/Payment/Prepare
        [Authorize(Roles = Models.User.ClientRole)]
        [HttpPost("Prepare")]
        public async Task<ActionResult<PaymentSession>> PreparePaymentSession(PurchaseDto purchaseRequest)
        {
            var session = await _paymentSessionService.PreparePaymentSession(User.Identity.Name, purchaseRequest);
            return session;
        }
        // POST: api/Payment/Perform
        [Authorize(Roles = Models.User.ClientRole)]
        [HttpPost("Perform")]
        public async Task<ActionResult<PaymentSession>> PerformPayment(PaymentDto paymentDto)
        {
            var session = await _paymentSessionService.PerformPayment(paymentDto);
            return session;
        }
    }
}