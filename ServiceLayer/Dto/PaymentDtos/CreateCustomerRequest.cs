using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_Managment_API.ServiceLayer.Dto.PaymentDtos
{
    public class CreateCustomerRequest
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
    }
}