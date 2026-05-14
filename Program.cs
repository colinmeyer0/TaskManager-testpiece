using System.Text;

var repo = new InMemoryTaskRepository();
var service = new TaskService(repo);

Console.WriteLine("Task Manager — type 'help' for available commands.");

while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine()?.Trim();
    if (line is null or { Length: 0 })
        continue;
    var parts = ParseArgs(line);
    var command = parts[0].ToLower();

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
                service.AddTask(parts[1], parts.Length >= 3 ? parts[2] : null);
                Console.WriteLine("Task added.");
                break;

            case "list":
                var groups = service.GetGroupedByStatus().ToList();

                if (!groups.Any())
                {
                    Console.WriteLine("No tasks.");
                    break;
                }
                foreach (var group in groups)
                {
                    Console.WriteLine($"\n{group.Key}:");

                    foreach (var task in group)
                    {
                        PrintTask(task);
                    }
                }
                break;

            case "complete":
                if (parts.Length < 2)
                {
                    Console.WriteLine("Usage: complete <id>");
                    break;
                }
                service.CompleteTask(int.Parse(parts[1]));
                Console.WriteLine($"Task {parts[1]} marked complete.");
                break;

            case "delete":
                if (parts.Length < 2)
                {
                    Console.WriteLine("Usage: delete <id>");
                    break;
                }
                service.DeleteTask(int.Parse(parts[1]));
                Console.WriteLine($"Task {parts[1]} deleted.");
                break;

            case "help" or "--help" or "-h":
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
                Console.WriteLine($"Unknown command '{command}'. Type 'help' for commands.");
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

static string[] ParseArgs(string line)
{
    var args = new List<string>();
    var current = new StringBuilder();
    bool inQuotes = false;

    foreach (var ch in line)
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

static void PrintTask(TaskItem task)
{
    var desc = task.Description is not null ? $" - {task.Description}" : string.Empty;
    Console.WriteLine($"  [{task.Id}] {task.Title}{desc}  (created {task.CreatedAt:yyyy-MM-dd HH:mm})");
}
