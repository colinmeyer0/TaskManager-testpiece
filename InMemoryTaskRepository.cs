class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public void Add(TaskItem task)
    {
        task.Id = _nextId++;
        _tasks.Add(task);
    }

    public IEnumerable<TaskItem> GetAll() => _tasks;

    public TaskItem? GetById(int id) =>
        _tasks.FirstOrDefault(t => t.Id == id);

    public void Update(TaskItem task)
    {
        var existing = GetById(task.Id)
            ?? throw new KeyNotFoundException($"Task {task.Id} not found.");
        
        existing.Title = task.Title;
        existing.Status = task.Status;
    }

    public void Delete(int id)
    {
        var task = GetById(id)
            ?? throw new KeyNotFoundException($"Task {id} not found.");

        _tasks.Remove(task);
    }
}