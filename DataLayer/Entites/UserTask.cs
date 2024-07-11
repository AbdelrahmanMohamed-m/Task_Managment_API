using Task_Managment_API.DataLayer.Entites;

namespace Task_Managment_API.DataLayer.Entities
{
    public class UserTask
    {
        public int Id { get; set; } // PK
        public string UserId { get; set; }
        public User User { get; set; }

        public int TaskId { get; set; }
        public Tasks Task { get; set; } 
    }
}