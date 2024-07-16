using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.DomainLayer.IRepo;
using Task_Managment_API.ServiceLayer.Dto.UserTaskDto;
using Task_Managment_API.ServiceLayer.IService;
using Task_Managment_API.ServiceLayer.Mappers;

namespace Task_Managment_API.ServiceLayer.Service;

public class UserTaskService : IUserTaskService
{
    private readonly IUserTaskRepo _userTaskRepo;

    public UserTaskService( IUserTaskRepo userTaskRepo)
    {
         
        _userTaskRepo = userTaskRepo;
    }

    public async Task<UserTaskDto> AssignUserToTask(int taskId, string userId, UserTask userTask)
    {
        await _userTaskRepo.AssignUserToTask(userTask, taskId, userId);
        return userTask.MapToDto();
    }


    public async Task<bool> RemoveUserFromTask(int taskId, string userId)
    {
        return await _userTaskRepo.RemoveUserFromTask(taskId, userId);
    }
}