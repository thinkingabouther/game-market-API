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
using Microsoft.AspNetCore.Http;
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
        /// <summary>
        /// Prepares a session to buy some games
        /// </summary>
        /// <param name="purchaseRequest">game ID and a number of games to buy</param>
        /// <param name="JWT">Token of the current user. Starts with "Bearer .."</param>
        /// <response code="200">Returns in case of success</response>
        /// <response code="400">Returns in case something went wrong. Check message for details</response>
        /// <response code="404">Returns in case there is no game with given id</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // POST: api/Payment/Prepare
        [Authorize(Roles = Models.User.ClientRole)]
        [HttpPost("Prepare")]
        public async Task<ActionResult<PaymentSessionViewModel>> PreparePaymentSession([FromHeader(Name = "Authorization")]string JWT, [FromBody] PurchaseDto purchaseRequest)
        {
            var session = await _paymentSessionService.PreparePaymentSession(User.Identity.Name, purchaseRequest);
            return session;
        }

        // POST: api/Payment/Perform/{id}
        /// <summary>
        /// Performs a payment for session with given id
        /// </summary>
        /// <param name="paymentDto">Card number to pay with</param>
        /// <param name="JWT">Token of the current user. Starts with "Bearer .."</param>
        /// <response code="200">Returns in case of success</response>
        /// <response code="400">Returns in case something went wrong. Check message for details</response>
        /// <response code="404">Returns in case there is no session with given id</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Models.User.ClientRole)]
        [HttpPost("Perform/{id}")]
        public async Task<ActionResult<PaymentSessionViewModel>> PerformPayment([FromHeader(Name = "Authorization")]string JWT, int id, [FromBody] PaymentDto paymentDto)
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