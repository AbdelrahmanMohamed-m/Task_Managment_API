using System.ComponentModel.DataAnnotations;

namespace Task_Managment_API.ServiceLayer.Dto.TasksDtos;

public class TasksDto
{
    
    public int Id { get; set; }

    [Required]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]
    [MaxLength(50, ErrorMessage = "Title must be at most 50 characters long")]
    public string Title { get; set; } = " ";

    [Required]
    [MinLength(5, ErrorMessage = "Description must be at least 5 characters long")]
    [MaxLength(280, ErrorMessage = "Description must be at most 280 characters long")]
    public string Description { get; set; } = " ";

    public DateTime DueDate { get; set; }

    public string Priority { get; set; } = " ";

    public string Status { get; set; } = " ";

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int ProjectId { get; set; } // Foreign key to Project

}