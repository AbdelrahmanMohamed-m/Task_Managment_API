using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.DomainLayer.IRepo;
using Task_Managment_API.ServiceLayer.Dto.ProjectDtos;
using Task_Managment_API.ServiceLayer.IService;
using Task_Managment_API.ServiceLayer.Mappers;

namespace Task_Managment_API.ServiceLayer.Service;

public class ProjectService(IProjectRepo projectRepo) : IProjectService
{
    public async Task<UpdateProjectDto> CreateProject(Project? project, string id)
    {
        await projectRepo.CreateProject(project, id);
        return project.ProjectToUpdateProject();
    }

    public async Task<UpdateProjectDto> UpdateProject(Project? project, int id)
    {
        await projectRepo.UpdateProject(project, id);
        return project.ProjectToUpdateProject();
    }

    public async Task<bool> DeleteProject(int id)
    {
        return await projectRepo.DeleteProject(id);
    }

    public async Task<ProjectDto> GetProjectById(int id)
    {
        var project = await projectRepo.GetProjectById(id);
        if (project == null)
        {
            return null;
        }

        return project.ProjectToProjectDto();
    }

    public async Task<List<ProjectDto>> GetAllUserProjects(string id)
    {
        var projects = await projectRepo.GetAllUserProjects(id);
        return projects.Select(p => p.ProjectToProjectDto()).ToList();
    }
}