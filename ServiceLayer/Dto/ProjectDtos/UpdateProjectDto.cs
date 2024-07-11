namespace Task_Managment_API.ServiceLayer.Dto.ProjectDtos;

public class UpdateProjectDto
{
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
}