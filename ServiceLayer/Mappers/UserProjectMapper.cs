using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.ServiceLayer.Dto.UserProjectDto;

namespace Task_Managment_API.ServiceLayer.Mappers;

public static class UserProjectMapper
{
    public static UserProjectDto MapToDto(this ProjectsCollaborators projectsCollaborators)
    {
        return new UserProjectDto
        {
            UserId = projectsCollaborators.UserId,
            ProjectId = projectsCollaborators.ProjectId
        };
    }

    public static ProjectsCollaborators MapToEntity(this UserProjectDto userProjectDto)
    {
        return new ProjectsCollaborators
        {
            UserId = userProjectDto.UserId,
            ProjectId = userProjectDto.ProjectId
        };
    }
}