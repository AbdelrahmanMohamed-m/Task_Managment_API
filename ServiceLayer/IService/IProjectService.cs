using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.ServiceLayer.Dto.ProjectDtos;

namespace Task_Managment_API.ServiceLayer.IService;

public interface IProjectService
{
     public Task<UpdateProjectDto> CreateProject(Project? project, string id); 
     public Task<UpdateProjectDto> UpdateProject(Project? project, int id);
      
     public Task<bool> DeleteProject(int id);
    
     public Task<ProjectDto> GetProjectById(int id);
    
      public Task<List<ProjectDto>> GetAllUserProjects(string id);
}