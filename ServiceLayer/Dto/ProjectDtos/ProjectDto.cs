using System.ComponentModel.DataAnnotations;
using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.ServiceLayer.Dto.TasksDtos;

namespace Task_Managment_API.ServiceLayer.Dto.ProjectDtos;

public class ProjectDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(5, ErrorMessage = "Name must be at least 5 characters long")]
    [MaxLength(50, ErrorMessage = "Description must be at most 50 characters long")] 
    public string Name { get; set; } = "";

    [Required]
    [MinLength(5, ErrorMessage = "Description must be at least 5 characters long")]
    [MaxLength(280, ErrorMessage = "Description must be at most 280 characters long")]
    public string Description { get; set; } = "";
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<TasksDto?> Tasks { get; set; } = new List<TasksDto>();

    public ICollection<UserProjectDto.UserProjectDto> UserProjects { get; set; } = new List<UserProjectDto.UserProjectDto>();
}