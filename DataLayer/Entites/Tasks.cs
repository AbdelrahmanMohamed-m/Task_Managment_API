using Task_Managment_API.DataLayer.Entities;

namespace Task_Managment_API.DataLayer.Entites
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime DueDate { get; set; }
        public string Priority { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();

    }
}