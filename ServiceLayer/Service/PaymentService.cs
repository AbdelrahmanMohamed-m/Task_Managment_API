using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Managment_API.ServiceLayer.Dto.PaymentDtos;
using Task_Managment_API.ServiceLayer.IService;

namespace Task_Managment_API.ServiceLayer.Service
{
    public class PaymentService : IPaymentService
    {

       
        public async Task<bool> CancelSubscriptionAsync(string subscriptionId)
        {
            var service = new Stripe.SubscriptionService();
            Stripe.Subscription subscription = await service.CancelAsync(subscriptionId);
            return subscription.Status == "canceled";

        }

        public async Task<string> CreateCustomerPortalSessionAsync(string customerId)
        {
            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = customerId,
                ReturnUrl = "http://localhost:3000/dashboard", // Your app's return URL
            };
            var service = new Stripe.BillingPortal.SessionService();
            var session = await service.CreateAsync(options);
            return session.Url;
        }

        public async Task<string> CreateCustomerAsync(CreateCustomerRequest Customer)
        {
            
            var options = new Stripe.CustomerCreateOptions
            {
                Email = Customer.Email,
                Name = Customer.Name,
            };
            var service = new Stripe.CustomerService();
            Stripe.Customer customer = await service.CreateAsync(options);
            return  customer.Id;
        }

        public async Task<string> CreateSubscriptionAsync(string customerId, string priceId)
        {
            // Use a test payment method token for testing
            var paymentMethodService = new Stripe.PaymentMethodService();
            Stripe.PaymentMethod paymentMethod = await paymentMethodService.CreateAsync(new Stripe.PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new Stripe.PaymentMethodCardOptions
                {
                    Token = "tok_visa", 
                },
            });

            // Attach to customer
            var attachOptions = new Stripe.PaymentMethodAttachOptions
            {
                Customer = customerId,
            };
            await paymentMethodService.AttachAsync(paymentMethod.Id, attachOptions);

            // Set as default
            var customerService = new Stripe.CustomerService();
            var customerUpdateOptions = new Stripe.CustomerUpdateOptions
            {
                InvoiceSettings = new Stripe.CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethod.Id,
                },
            };
            await customerService.UpdateAsync(customerId, customerUpdateOptions);

            // Now create subscription
            var subscriptionOptions = new Stripe.SubscriptionCreateOptions
            {
                Customer = customerId,
                Items = new List<Stripe.SubscriptionItemOptions>
                {
                    new Stripe.SubscriptionItemOptions
                    {
                        Price = priceId,
                    },
                },
                Expand = new List<string> { "latest_invoice.payment_intent" },
            };
            var subscriptionService = new Stripe.SubscriptionService();
            Stripe.Subscription subscription = await subscriptionService.CreateAsync(subscriptionOptions);
            return subscription.Id;
        }
    }
}