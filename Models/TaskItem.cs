namespace TaskManagerCLI.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } // "string?": means nullable string (description not required)
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
