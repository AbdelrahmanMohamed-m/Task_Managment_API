using Microsoft.EntityFrameworkCore;
using Task_Managment_API.DataLayer.Entites;

namespace Task_Managment_API.DataLayer.Entities
{
    public class ProjectsCollaborators
    {
        public int Id { get; set; } // PK
        public string UserId { get; set; }
        public User User { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}