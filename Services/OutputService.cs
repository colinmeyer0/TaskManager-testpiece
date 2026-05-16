using TaskManagerCLI.Models;

namespace TaskManagerCLI.Services;

public class OutputService
{
    public static void PrintTask(TaskItem task)
    {
        string desc = task.Description is not null ? $" - {task.Description}" : string.Empty;
        Console.WriteLine(
            $"  [{task.Id}] {task.Title}{desc}  (created {task.CreatedAt:yyyy-MM-dd HH:mm})"
        );
    }
}
