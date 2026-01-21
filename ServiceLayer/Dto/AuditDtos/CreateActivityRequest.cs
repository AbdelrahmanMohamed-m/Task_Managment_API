namespace Task_Managment_API.ServiceLayer.Dto.AuditDtos
{
    public class CreateActivityRequest
    {
        public Guid TaskId { get; set; }
        public required string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public required string PerformedBy { get; set; }
        public Dictionary<string, object>? Details { get; set; }
    }

}