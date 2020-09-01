using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.Services;
using game_market_API.Services.NotifyingService;
using game_market_API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace game_market_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentSessionService _paymentSessionService;
        private readonly IEnumerable<INotifyingService> _notifyingServices;

        public PaymentController(IPaymentSessionService paymentSessionService,
            IEnumerable<INotifyingService> notifyingServices)
        {
            _paymentSessionService = paymentSessionService;
            _notifyingServices = notifyingServices;
        }

        // POST: api/Payment/Prepare
        [Authorize(Roles = Models.User.ClientRole)]
        [HttpPost("Prepare")]
        public async Task<ActionResult<PaymentSessionViewModel>> PreparePaymentSession([FromBody] PurchaseDto purchaseRequest)
        {
            var session = await _paymentSessionService.PreparePaymentSession(User.Identity.Name, purchaseRequest);
            return session;
        }

        // POST: api/Payment/Perform
        [Authorize(Roles = Models.User.ClientRole)]
        [HttpPost("Perform/{id}")]
        public async Task<ActionResult<PaymentSessionViewModel>> PerformPayment(int id, [FromBody] PaymentDto paymentDto)
        {
            var sessionViewModel = await _paymentSessionService.PerformPayment(User.Identity.Name, id, paymentDto);
            var session =  await _paymentSessionService.LoadSession(User.Identity.Name, id, paymentDto);
            foreach (INotifyingService service in _notifyingServices)
            {
                await service.Notify(session);
            }
            return sessionViewModel;
        }
    }
}