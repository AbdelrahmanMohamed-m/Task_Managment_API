using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.ServiceLayer.Dto.ProjectDtos;

namespace Task_Managment_API.ServiceLayer.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectDto ProjectToProjectDto(this Project? project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                // when u make a TasksDto class u change  it here  to project.Tasks.Select(x => x.TaskToTaskDto()).ToList()
                Tasks = project.Tasks
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