using Microsoft.EntityFrameworkCore;
using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.DomainLayer.IRepo;
using Task_Managment_API.ServiceLayer.Dto.AuditDtos;
using Task_Managment_API.ServiceLayer.Dto.TasksDtos;
using Task_Managment_API.ServiceLayer.IService;
using Task_Managment_API.ServiceLayer.Mappers;

namespace Task_Managment_API.ServiceLayer.Service;

public class TasksService : ITasksService
{
    private readonly ITasksRepo _tasksRepo;
    private readonly IAuditClient _auditClient;

    public TasksService(ITasksRepo tasksRepo, IAuditClient auditClient)
    {
        _tasksRepo = tasksRepo;
        _auditClient = auditClient;
    }


    public async Task<List<TasksDto?>> GetAllUserTasks(string userId)
    {
        var tasks = await _tasksRepo.GetAllUserTasks(userId);
        return tasks.Select(t => t.TasksMapToTasksDto()).ToList();
    }

    public async Task<TasksDto?> AddTask(Tasks? task, int projectId, string userId)
    {
        var taskEntity = await _tasksRepo.AddTask(task, projectId, userId);
        if (taskEntity == null)
        {
            return null;
        }
        await _auditClient.LogAsync(new CreateActivityRequest
        {
            TaskId = Guid.NewGuid(),
            Action = "TaskCreated",
            PerformedBy = userId,
            Details = new Dictionary<string, object>
            {
                ["Title"] = taskEntity.Title,
                ["Status"] = taskEntity.Status
            }
        });
        return taskEntity.TasksMapToTasksDto();
    }

    public async Task<TasksDto?> GetTaskById(int id)
    {
        var task = await _tasksRepo.GetTaskById(id);
        return task?.TasksMapToTasksDto();
    }

    public async Task<TasksDto?> UpdateTask(int id, Tasks? task)
    {
        var taskUpdate = await _tasksRepo.UpdateTask(id, task);
        return taskUpdate?.TasksMapToTasksDto();
    }

    public async Task<bool> DeleteTask(int id)
    {
        var task = await _tasksRepo.DeleteTask(id);
        return task;
    }
}