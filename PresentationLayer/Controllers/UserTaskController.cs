using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.ServiceLayer.IService;

namespace Task_Managment_API.PresentationLayer.Controllers;

[ApiController]
[Route("api/UserTask")]
public class UserTaskController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IUserTaskService _userTaskService;

    public UserTaskController(IUserTaskService userTaskService, UserManager<User> userManager)
    {
        _userTaskService = userTaskService;
        _userManager = userManager;
    }

    [HttpPost("AssignUserToTask")]
    [Authorize]
    public async Task<IActionResult> AssignUserToTask(int taskId, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var existingAssignment = await _userTaskService.CheckAssignmentExists(taskId, userId);

        if (existingAssignment)
        {
            return StatusCode(StatusCodes.Status409Conflict, "User is already assigned to the Task.");
        }

        var userProject = new UserTask
        {
            UserId = userId,
            TaskId = taskId
        };

        var task = await _userTaskService.AssignUserToTask(taskId, userId, userProject);
        return Ok(task);
    }

    [HttpDelete("RemoveUserFromTask")]
    [Authorize]
    public async Task<IActionResult> RemoveUserFromTask(int taskId, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var result = await _userTaskService.RemoveUserFromTask(taskId, userId);

        if (!result)
        {
            return NotFound("Task Does not exist.");
        }

        return Ok(result);
    }
}