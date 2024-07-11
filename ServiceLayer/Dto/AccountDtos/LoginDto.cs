using System.ComponentModel.DataAnnotations;

namespace Task_Managment_API.ServiceLayer.Dto.AccountDtos;

public class LoginDto
{

    [Required]
    public string? Username { get; set; } = "";
    [Required]
    public string? Password { get; set; } = "";

}