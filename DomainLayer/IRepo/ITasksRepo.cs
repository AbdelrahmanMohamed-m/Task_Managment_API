using Task_Managment_API.DataLayer.Entites;

namespace Task_Managment_API.DomainLayer.IRepo;

public interface ITasksRepo
{
    public Task<List<Tasks>> GetAllUserTasks(string userId);

    public Task<Tasks?> AddTask(Tasks? task, int projectId, string userId);

    public Task<Tasks> GetTaskById(int id);

    public Task<Tasks> UpdateTask(int id, Tasks? task);

    public Task<bool> DeleteTask(int id);
}