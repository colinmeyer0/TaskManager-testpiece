using TaskManagerCLI.Models;

namespace TaskManagerCLI.Managers;

public interface ITaskManager
{
    /// <summary>
    /// Adds a new task with the given title and optional description.
    /// </summary>
    /// <param name="title">Title of the task</param>
    /// <param name="description">Optional description</param>
    void AddTask(string title, string? description = null);

    /// <summary>
    /// Returns all tasks grouped by status.
    /// </summary>
    /// <returns>Grouped task enumerable</returns>
    IEnumerable<IGrouping<TaskItemStatus, TaskItem>> GetGroupedByStatus();

    /// <summary>
    /// Marks the task with the given ID as completed.
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <exception cref="KeyNotFoundException">No task with that ID exists</exception>
    void CompleteTask(int id);

    /// <summary>
    /// Deletes the task with the given ID.
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <exception cref="KeyNotFoundException">No task with that ID exists</exception>
    void DeleteTask(int id);

    /// <summary>
    /// Main REPL loop
    /// </summary>
    void Run();
}
