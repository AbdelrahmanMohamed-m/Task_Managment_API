using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.ServiceLayer.Dto.UserProjectDto;

namespace Task_Managment_API.ServiceLayer.IService;

public interface IUserProjectsService
{
     
    public Task<UserProjectDto> AssignUserToProject(ProjectsCollaborators projectsCollaborators, int projectId, string userId);
    public Task<UserProjectDto> RemoveUserFromProject(ProjectsCollaborators projectsCollaborators, int userProjectId);
}