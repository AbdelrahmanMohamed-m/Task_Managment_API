using System.ComponentModel.DataAnnotations;

namespace Task_Managment_API.ServiceLayer.Dto.AccountDtos
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; }
       
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}