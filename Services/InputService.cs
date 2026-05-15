using System.Text;
using TaskManagerCLI.Models;

namespace TaskManagerCLI.Services;

public class InputService
{
    public static string[] ParseArgs(string line)
    {
        List<string> args = new();
        StringBuilder current = new();
        bool inQuotes = false;

        foreach (char ch in line)
        {
            if (ch == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (ch == ' ' && !inQuotes)
            {
                if (current.Length > 0)
                {
                    args.Add(current.ToString());
                    current.Clear();
                }
            }
            else
            {
                current.Append(ch);
            }
        }

        if (current.Length > 0)
            args.Add(current.ToString());

        return args.ToArray();
    }

    public static void PrintTask(TaskItem task)
    {
        string desc = task.Description is not null ? $" - {task.Description}" : string.Empty;
        Console.WriteLine(
            $"  [{task.Id}] {task.Title}{desc}  (created {task.CreatedAt:yyyy-MM-dd HH:mm})"
        );
    }
}
