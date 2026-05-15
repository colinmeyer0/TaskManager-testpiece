using TaskManagerCLI.Models;

namespace TaskManagerCLI.Services;

public interface ITaskRepository
{
    void Add(TaskItem task);
    IEnumerable<TaskItem> GetAll();
    TaskItem? GetById(int id);
    void Update(TaskItem task);
    void Delete(int id);
}
