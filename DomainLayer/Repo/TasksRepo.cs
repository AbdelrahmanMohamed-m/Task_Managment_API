using Microsoft.EntityFrameworkCore;
using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.DomainLayer.IRepo;

namespace Task_Managment_API.DomainLayer.Repo;

public class TasksRepo : ITasksRepo
{
    private readonly ApplicationDbContext _context;

    public TasksRepo(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<List<Tasks>> GetAllUserTasks(string userId)
    {
        return await _context.Tasks
            .Include(t => t.UserTasks)
            .ThenInclude(ut => ut.User)
            .Where(t => t.UserTasks.Any(ut => ut.UserId == userId))
            .ToListAsync();
    }

    public async Task<Tasks?> AddTask(Tasks? task, int projectId, string userId)
    {
        await _context.Projects.FindAsync(projectId);

        task.ProjectId = projectId;
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        await _context.UserTasks.AddAsync(new UserTask { UserId = userId, TaskId = task.Id });
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<Tasks> GetTaskById(int id)
    {
        return await _context.Tasks.FindAsync(id) ?? throw new KeyNotFoundException("no such task found.");
    }

    public async Task<Tasks> UpdateTask(int id, Tasks? task)
    {
        var updateTask = await _context.Tasks
                             .FirstOrDefaultAsync(t => t.Id == id)
                         ?? throw new KeyNotFoundException("no such task found.");
        if (task != null)
        {
            updateTask.Title = task.Title;
            updateTask.Description = task.Description;
            updateTask.UpdatedAt = DateTime.UtcNow;
            updateTask.Status = task.Status;
            updateTask.Priority = task.Priority;
            updateTask.DueDate = task.DueDate;
        }

        await _context.SaveChangesAsync();

        return updateTask;
    }

    public async Task<bool> DeleteTask(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null)
        {
            return false;
        }

        // Assuming UserTask has a composite key of TaskId and UserId, you need to find all UserTask entries related to the task
        var userTasks = await _context.UserTasks
            .Where(ut => ut.TaskId == taskId)
            .ToListAsync();

        if (userTasks.Any())
        {
            _context.UserTasks.RemoveRange(userTasks);
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}