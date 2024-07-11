using Task_Managment_API.DataLayer.Entites;

namespace Task_Managment_API.DomainLayer.IRepo;

public interface IProjectRepo
{
    Task<Project?> CreateProject(Project project, string userId);
    Task<Project?> UpdateProject(Project project, int id);
    Task<bool> DeleteProject(int id);

    Task<Project?> GetProjectById(int id);
    Task<List<Project>> GetAllUserProjects(string id);
}