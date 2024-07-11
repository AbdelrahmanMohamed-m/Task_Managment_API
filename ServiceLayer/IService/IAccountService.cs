using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.DataLayer.Entities;

namespace Task_Managment_API.ServiceLayer.IService;

public interface IAccountService
{
    
    public string CreateToken(User user);
}