using System.ComponentModel.DataAnnotations;

namespace Task_Managment_API.ServiceLayer.Dto.AccountDtos;

public class UpdateProfileDto
{
    public  string UserName { get; set; }

     public  string Email { get; set; }
}