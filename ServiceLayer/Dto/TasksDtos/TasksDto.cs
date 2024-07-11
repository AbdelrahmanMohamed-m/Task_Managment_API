namespace Task_Managment_API.ServiceLayer.Dto.TasksDtos;

public class TasksDto
{
    public int Id { get; set; }

    public string Title { get; set; } = " ";

    public string Description { get; set; } = " ";

    public DateTime DueDate { get; set; }

    public string Priority { get; set; } = " ";

    public string Status { get; set; } = " ";

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int ProjectId { get; set; } // Foreign key to Project

}