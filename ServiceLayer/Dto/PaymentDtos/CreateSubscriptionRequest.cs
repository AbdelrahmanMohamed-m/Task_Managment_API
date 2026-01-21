namespace Task_Managment_API.ServiceLayer.Dto.PaymentDtos;

public class CreateSubscriptionRequest
{
    public string CustomerId { get; set; }
    public string PriceId { get; set; }
}