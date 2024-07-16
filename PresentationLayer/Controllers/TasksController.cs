using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.Extension;
using Task_Managment_API.ServiceLayer.Dto.TasksDtos;
using Task_Managment_API.ServiceLayer.IService;
using Task_Managment_API.ServiceLayer.Mappers;

namespace Task_Managment_API.PresentationLayer.Controllers;

[Route("api/Task")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITasksService _tasksService;
    private readonly UserManager<User> _userManager;

    public TasksController(ITasksService tasksService, UserManager<User> userManager)
    {
        _tasksService = tasksService;
        _userManager = userManager;
    }

    [HttpGet("GetAllUserTasks")]
    [Authorize]
    public async Task<IActionResult> GetAllUserTasks()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Retrieve the User ID from the User principal
        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);
        var result = await _tasksService.GetAllUserTasks(appUser.Id);
        return Ok(result);
    }


    [HttpPost]
    [Authorize]
    [Route("{projectId:int}")]
    public async Task<IActionResult> AddTask(TasksDto task, [FromRoute] int projectId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);
        var createTask = task.TasksDtoMapToTasksEntity();
        var result = await _tasksService.AddTask(createTask, projectId, appUser.Id);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize]
    public async Task<IActionResult> GetTaskById(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _tasksService.GetTaskById(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateTask(int id, TasksDto task)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _tasksService.UpdateTask(id, task.TasksDtoMapToTasksEntity());
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteTask(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _tasksService.DeleteTask(id);
        if (!result)
        {
            return NotFound();
        }

        return Ok(result);
    }
}