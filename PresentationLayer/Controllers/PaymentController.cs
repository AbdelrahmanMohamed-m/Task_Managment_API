using Microsoft.AspNetCore.Mvc;
using Stripe;
using Task_Managment_API.ServiceLayer.IService;
using Task_Managment_API.ServiceLayer.Dto.PaymentDtos;

namespace Task_Managment_API.PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }



        [HttpPost("customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            var customerId = await _paymentService.CreateCustomerAsync(request);
            return Ok(new { CustomerId = customerId });
        }

        [HttpPost("subscription")]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequest request)
        {
            var subscriptionId = await _paymentService.CreateSubscriptionAsync(request.CustomerId, request.PriceId);
            return Ok(new { SubscriptionId = subscriptionId });
        }

        [HttpPost("portal")]
        public async Task<IActionResult> CreateCustomerPortal([FromBody] CreateCustomerPortalRequest request)
        {
            var portalUrl = await _paymentService.CreateCustomerPortalSessionAsync(request.CustomerId);
            return Ok(new { PortalUrl = portalUrl });
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], "your_webhook_secret");

            if (stripeEvent.Type == "customer.subscription.created")
            {
                // Handle subscription creation
            }
            return Ok();
        }
    }
}