namespace BackendTestTask.Models;

public class ExceptionLog
{
    public int Id { get; set; }
    public Guid EventId { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string QueryParameters { get; set; }
    public string BodyParameters { get; set; }
    public string StackTrace { get; set; }
}