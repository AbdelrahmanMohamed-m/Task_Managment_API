using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.ServiceLayer.Dto.ProjectDtos;
using Task_Managment_API.ServiceLayer.Dto.TasksDtos;
using Task_Managment_API.ServiceLayer.Dto.UserProjectDto;

namespace Task_Managment_API.ServiceLayer.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectDto ProjectToProjectDto(this Project? project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                Tasks = project.Tasks?.Select(x => x.TasksMapToTasksDto()).ToList() ?? new List<TasksDto>(),
                UserProjects = project.UserProject?.Select(x => x.MapToDto()).ToList() ?? new List<UserProjectDto>()
            };
        }


        public static UpdateProjectDto ProjectToUpdateProject(this Project? project)
        {
            return new UpdateProjectDto
            {
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt
            };
        }


        public static Project ProjectDtoToProject(this ProjectDto projectDto)
        {
            return new Project
            {
                Id = projectDto.Id,
                Name = projectDto.Name,
                Description = projectDto.Description,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                CreatedAt = projectDto.CreatedAt,
                UpdatedAt = projectDto.UpdatedAt
            };
        }

        public static Project? UpdateProjectDtoToProject(this UpdateProjectDto updateProjectDto)
        {
            return new Project
            {
                Name = updateProjectDto.Name,
                Description = updateProjectDto.Description,
                StartDate = updateProjectDto.StartDate,
                EndDate = updateProjectDto.EndDate,
                CreatedAt = updateProjectDto.CreatedAt,
                UpdatedAt = updateProjectDto.UpdatedAt
            };
        }
    }
}