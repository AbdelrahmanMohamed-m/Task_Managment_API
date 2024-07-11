using Task_Managment_API.DataLayer.Entities;

namespace Task_Managment_API.DataLayer.Entites
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
        public ICollection<ProjectsCollaborators> UserProject { get; set; }
    }
}