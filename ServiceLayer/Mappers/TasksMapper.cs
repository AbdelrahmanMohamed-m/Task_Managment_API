using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.ServiceLayer.Dto.TasksDtos;

namespace Task_Managment_API.ServiceLayer.Mappers;

public static class TasksMapper
{
    public static TasksDto? TasksMapToTasksDto(this Tasks? task)
    {
        return new TasksDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Priority = task.Priority,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            ProjectId = task.ProjectId,
        };
    }
    
    public static Tasks TasksDtoMapToTasksEntity(this TasksDto? taskDto)
    {
        return new Tasks
        {
            Id = taskDto.Id,
            Title = taskDto.Title,
            Description = taskDto.Description,
            DueDate = taskDto.DueDate,
            Priority = taskDto.Priority,
            Status = taskDto.Status,
            CreatedAt = taskDto.CreatedAt,
            UpdatedAt = taskDto.UpdatedAt,
        };
    }
}