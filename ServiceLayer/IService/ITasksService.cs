using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.ServiceLayer.Dto.TasksDtos;

namespace Task_Managment_API.ServiceLayer.IService;

public interface ITasksService
{
    public Task<List<TasksDto?>> GetAllUserTasks(string userId);
    public Task<TasksDto?> AddTask(Tasks? task, int projectId, string userId);

    public Task<TasksDto?> GetTaskById(int id);

    public Task<TasksDto?> UpdateTask(int id, Tasks? task);

    public Task<bool> DeleteTask(int id);
}