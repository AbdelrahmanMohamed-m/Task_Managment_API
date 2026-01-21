using Task_Managment_API.ServiceLayer.Dto.PaymentDtos; // Create this folder/DTOs

namespace Task_Managment_API.ServiceLayer.IService;

public interface IPaymentService
{
    Task<string> CreateCustomerAsync(CreateCustomerRequest Customer);
    Task<string> CreateSubscriptionAsync(string customerId, string priceId);
    Task<bool> CancelSubscriptionAsync(string subscriptionId);
    Task<string> CreateCustomerPortalSessionAsync(string customerId);
}
