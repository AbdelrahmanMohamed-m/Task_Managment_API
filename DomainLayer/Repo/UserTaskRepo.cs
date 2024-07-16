using Microsoft.EntityFrameworkCore;
using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.DomainLayer.IRepo;

namespace Task_Managment_API.DomainLayer.Repo;

public class UserTaskRepo : IUserTaskRepo
{
    private readonly ApplicationDbContext _context;

    public UserTaskRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserTask> AssignUserToTask(UserTask userTasks, int taskId, string userId)
    {
        userTasks.UserId = userId;
        userTasks.TaskId = taskId;

        await _context.UserTasks.AddAsync(userTasks);
        await _context.SaveChangesAsync();
        return userTasks;
    }

    public async Task<bool> RemoveUserFromTask(int taskId, string userId)
    {
        var userTask = await _context.UserTasks
            .FirstOrDefaultAsync(ut => ut.TaskId == taskId && ut.UserId == userId);

        if (userTask == null)
        {
            Console.WriteLine($"User task not found for TaskId: {taskId}, UserId: {userId}");
            return false;
        }

        _context.UserTasks.Remove(userTask);
        await _context.SaveChangesAsync();
        Console.WriteLine($"User task removed for TaskId: {taskId}, UserId: {userId}");
        return true;
    }
}