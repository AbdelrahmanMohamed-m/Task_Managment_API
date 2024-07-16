using Task_Managment_API.DataLayer.Entities;

namespace Task_Managment_API.DomainLayer.IRepo;

public interface IUserProjectRepo
{
    public Task<ProjectsCollaborators> AssignUserToProject(
        ProjectsCollaborators projectsCollaborators,
        int projectId,
        string userId
    );

    public Task<bool> RemoveUserFromProject(int projectId, string userId);
}