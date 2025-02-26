namespace ConstructionPMS.Application.DTOs
{
    public class ProjectTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public Guid ProjectId { get; set; }
    }
}