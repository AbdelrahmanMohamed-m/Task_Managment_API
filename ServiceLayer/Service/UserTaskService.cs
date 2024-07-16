using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.DomainLayer.IRepo;
using Task_Managment_API.ServiceLayer.Dto.UserTaskDto;
using Task_Managment_API.ServiceLayer.IService;
using Task_Managment_API.ServiceLayer.Mappers;

namespace Task_Managment_API.ServiceLayer.Service;

public class UserTaskService : IUserTaskService
{
    private readonly IUserTaskRepo _userTaskRepo;

    public UserTaskService(IUserTaskRepo userTaskRepo)
    {
        _userTaskRepo = userTaskRepo;
    }

    public async Task<UserTaskDto> AssignUserToTask(int taskId, string userId, UserTask userTask)
    {
        var result = await _userTaskRepo.AssignUserToTask(userTask, taskId, userId);


        if (result == null)
        {
            return null;
        }

        return result.MapToDto();
    }


    public async Task<bool> RemoveUserFromTask(int taskId, string userId)
    {
        return await _userTaskRepo.RemoveUserFromTask(taskId, userId);
    }

    public async Task<bool> CheckAssignmentExists(int taskId, string userId)
    {
        return await _userTaskRepo.CheckAssignmentExists(taskId, userId);
    }
}