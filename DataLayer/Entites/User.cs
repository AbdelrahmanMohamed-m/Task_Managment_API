using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Task_Managment_API.DataLayer.Entities
{
    public class User : IdentityUser
    {
        public ICollection<ProjectsCollaborators> UserProjects { get; set; }
        public ICollection<UserTask> UserTasks { get; set; }
    }
}