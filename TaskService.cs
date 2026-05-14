public class TaskService
{
    private readonly ITaskRepository _repo;

    public TaskService(ITaskRepository repo)
    {
        _repo = repo;
    }

    public void AddTask(string title, string? description = null)
    {
        var task = new TaskItem { Title = title, Description = description };
        _repo.Add(task);
    }

    // GetPendingTasks
    public IEnumerable<TaskItem> GetPendingTasks() =>
        _repo.GetAll().Where(t => t.Status == TaskStatus.Pending);

    public IEnumerable<TaskItem> GetCompletedTasks() =>
        _repo.GetAll().Where(t => t.Status == TaskStatus.Completed);

    public IEnumerable<IGrouping<TaskStatus, TaskItem>> GetGroupedByStatus() =>
        _repo.GetAll().GroupBy(t => t.Status);

    public void CompleteTask(int id)
    {
        var task = _repo.GetById(id) ??
            throw new KeyNotFoundException($"No task with ID {id}.");
        task.Status = TaskStatus.Completed;
        _repo.Update(task);
    }
}