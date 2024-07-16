using Task_Managment_API.DataLayer.Entities;

namespace Task_Managment_API.DomainLayer.IRepo;

public interface IUserTaskRepo
{
    public Task<UserTask> AssignUserToTask(
        UserTask userTasks,
        int taskId,
        string userId
    );

    public Task<bool> RemoveUserFromTask(
        int taskId,
        string userId);
}