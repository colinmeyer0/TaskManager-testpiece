using TaskManagerCLI.Managers;
using TaskManagerCLI.Services;

namespace TaskManagerCLI;

public class Program
{
    public static void Main(string[] args)
    {
        ITaskRepository repo = new InMemoryTaskRepository();
        ITaskManager manager = new TaskManager(repo);
        manager.Run();
    }
}
