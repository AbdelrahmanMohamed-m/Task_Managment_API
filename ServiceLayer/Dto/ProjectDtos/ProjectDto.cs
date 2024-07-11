using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.DataLayer.Entities;

namespace Task_Managment_API.ServiceLayer.Dto.ProjectDtos;

public class ProjectDto
{
    
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    
    public ICollection<ProjectsCollaborators> UserProjects { get; set; } = new List<ProjectsCollaborators>();
}