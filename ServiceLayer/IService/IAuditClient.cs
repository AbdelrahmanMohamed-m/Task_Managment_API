using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Managment_API.ServiceLayer.Dto.AuditDtos;

namespace Task_Managment_API.ServiceLayer.IService
{
    public interface IAuditClient
    {
        Task LogAsync(CreateActivityRequest request);
    }
}