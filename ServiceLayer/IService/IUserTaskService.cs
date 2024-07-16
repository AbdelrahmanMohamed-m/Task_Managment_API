using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.ServiceLayer.Dto.UserTaskDto;

namespace Task_Managment_API.ServiceLayer.IService;

public interface IUserTaskService
{
    public Task<UserTaskDto> AssignUserToTask(int taskId, string userId, UserTask userTask);

    public Task<bool> RemoveUserFromTask(int taskId, string userId);
}