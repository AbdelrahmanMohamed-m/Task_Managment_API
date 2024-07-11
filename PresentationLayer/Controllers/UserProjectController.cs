﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.Extension;
using Task_Managment_API.ServiceLayer.IService;

namespace Task_Managment_API.PresentationLayer.Controllers;

[Route("api/ProjectUsers")]
[ApiController]
public class UserProjectController : ControllerBase
{
    private readonly IUserProjectsService _userProjectsService;
    private readonly UserManager<User> _userManager;

    public UserProjectController(UserManager<User> userManager, IUserProjectsService userProjectsService)
    {
        _userManager = userManager;
        _userProjectsService = userProjectsService;
    }

    [HttpPost]
    [Route("AssignUserToProject")]
    [Authorize]
    public async Task<IActionResult> AssignUserToProject(int projectId, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var userProject = new ProjectsCollaborators
        {
            UserId = userId,
            ProjectId = projectId
        };
        var result = await _userProjectsService.AssignUserToProject(userProject, projectId, userId);
        return Ok(result);
    }

    [HttpDelete]
    [Route("RemoveUserFromProject")]
    [Authorize]
    public async Task<IActionResult> RemoveUserFromProject(int userProjectId)
    {
        var userProject = new ProjectsCollaborators
        {
            Id = userProjectId
        };
        var result = await _userProjectsService.RemoveUserFromProject(userProject, userProjectId);
        return Ok(result);
    }
    
}