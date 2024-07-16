using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.ServiceLayer.Dto.UserTaskDto;

namespace Task_Managment_API.ServiceLayer.Mappers;

public static class UserTaskMapper
{
    public static UserTaskDto MapToDto(this UserTask userTask)
    {
        return new UserTaskDto
        {
            UserId = userTask.UserId,
            TaskId = userTask.TaskId
        };
    }

    public static UserTask MapToEntity(this UserTaskDto userTaskDto)
    {
        return new UserTask
        {
            UserId = userTaskDto.UserId,
            TaskId = userTaskDto.TaskId
        };
    }
}