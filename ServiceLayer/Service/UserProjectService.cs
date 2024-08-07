﻿using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.DomainLayer.IRepo;
using Task_Managment_API.ServiceLayer.Dto.UserProjectDto;
using Task_Managment_API.ServiceLayer.IService;
using Task_Managment_API.ServiceLayer.Mappers;

namespace Task_Managment_API.ServiceLayer.Service;

public class UserProjectService : IUserProjectsService
{
    private readonly IUserProjectRepo _userProjectRepo;

    public UserProjectService(IUserProjectRepo userProjectRepo)
    {
        _userProjectRepo = userProjectRepo;
    }

    public async Task<UserProjectDto> AssignUserToProject(ProjectsCollaborators projectsCollaborators, int projectId,
        string userId)
    {
        var result = await _userProjectRepo.AssignUserToProject(projectsCollaborators, projectId, userId);

        if (result == null)
        {
            return null;
        }

        return result.MapToDto();
    }

    public async Task<bool> RemoveUserFromProject(int projectId, string userId)
    {
        return await _userProjectRepo.RemoveUserFromProject(projectId, userId);
    }

    public async Task<bool> CheckUserProjectAssignmentExists(int projectId, string userId)
    {
        return await _userProjectRepo.CheckAssignmentExists(projectId, userId);
    }
}