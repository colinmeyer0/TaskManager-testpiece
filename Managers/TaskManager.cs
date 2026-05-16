using TaskManagerCLI.Models;
using TaskManagerCLI.Services;

namespace TaskManagerCLI.Managers;

public class TaskManager : ITaskManager
{
    private readonly ITaskRepository _repo;

    public TaskManager(ITaskRepository repo)
    {
        _repo = repo;
    }

    public void AddTask(string title, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title));
        TaskItem task = new() { Title = title, Description = description };
        _repo.Add(task);
    }

    public IEnumerable<IGrouping<TaskItemStatus, TaskItem>> GetGroupedByStatus() =>
        _repo.GetAll().GroupBy(t => t.Status);

    public void CompleteTask(int id)
    {
        TaskItem task =
            _repo.GetById(id) ?? throw new KeyNotFoundException($"No task with ID {id}.");
        task.Status = TaskItemStatus.Completed;
        _repo.Update(task);
    }

    public void DeleteTask(int id)
    {
        _ = _repo.GetById(id) ?? throw new KeyNotFoundException($"No task with ID {id}.");
        _repo.Delete(id);
    }

    public void Run()
    {
        Console.WriteLine("Task Manager — type 'help' for available commands.");

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine()?.Trim();
            if (line is null or { Length: 0 })
                continue;
            string[] parts = InputService.ParseArgs(line);
            string command = parts[0].ToLower();

            try
            {
                switch (command)
                {
                    case "add":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Usage: add <title> [description]");
                            break;
                        }
                        AddTask(parts[1], parts.Length >= 3 ? parts[2] : null);
                        Console.WriteLine("Task added.");
                        break;

                    case "list":
                        List<IGrouping<TaskItemStatus, TaskItem>> groups = GetGroupedByStatus()
                            .ToList();

                        if (!groups.Any())
                        {
                            Console.WriteLine("No tasks.");
                            break;
                        }
                        foreach (var group in groups)
                        {
                            Console.WriteLine($"\n{group.Key}:");

                            foreach (TaskItem task in group)
                            {
                                OutputService.PrintTask(task);
                            }
                        }
                        break;

                    case "complete":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Usage: complete <id>");
                            break;
                        }
                        CompleteTask(int.Parse(parts[1]));
                        Console.WriteLine($"Task {parts[1]} marked complete.");
                        break;

                    case "delete":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Usage: delete <id>");
                            break;
                        }
                        DeleteTask(int.Parse(parts[1]));
                        Console.WriteLine($"Task {parts[1]} deleted.");
                        break;

                    case "help":
                        Console.WriteLine("Commands:");
                        Console.WriteLine("  add <title> [description]");
                        Console.WriteLine("  list");
                        Console.WriteLine("  complete <id>");
                        Console.WriteLine("  delete <id>");
                        Console.WriteLine("  exit");
                        break;

                    case "exit":
                    case "quit":
                        return;

                    default:
                        Console.WriteLine(
                            $"Unknown command '{command}'. Type 'help' for commands."
                        );
                        break;
                }
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: ID must be a valid integer.");
            }
        }
    }
}
