using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.Extension;
using Task_Managment_API.ServiceLayer.Dto.ProjectDtos;
using Task_Managment_API.ServiceLayer.IService;
using Task_Managment_API.ServiceLayer.Mappers;

namespace Task_Managment_API.PresentationLayer.Controllers;

[ApiController]
[Route("api/project")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly UserManager<User> _userManager;

    public ProjectController(IProjectService projectService, UserManager<User> userManager)
    {
        _projectService = projectService;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateProject([FromBody] UpdateProjectDto updateProjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Retrieve the User ID from the User principal
        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);

        var createdProject =
            await _projectService.CreateProject(updateProjectDto.UpdateProjectDtoToProject(), appUser.Id);

        if (createdProject == null)
        {
            return BadRequest("Project could not be created.");
        }

        return Ok(createdProject);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult> GetProjectById(int id)
    {
        var project = await _projectService.GetProjectById(id);

        if (project == null)
        {
            return NotFound();
        }

        return Ok(project);
    }


    [HttpPut]
    [ResponseCache(Duration = 10)]
    [Authorize]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateProject([FromRoute] int id, UpdateProjectDto projectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updateProjectDtoToProject = projectDto.UpdateProjectDtoToProject();
        var result = await _projectService.UpdateProject(updateProjectDtoToProject, id);
        if (result == null)
        {
            return NotFound("Project not found");
        }

        return Ok(result);
    }

    [HttpDelete]
    [Authorize]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteProject([FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _projectService.DeleteProject(id);
        if (result == null)
        {
            return NotFound("there is no project with this id");
        }

        return Ok(result);
    }


    [HttpGet("All")]
    [Authorize]
    public async Task<IActionResult> GetAllUserProjects()
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);
        var results = await _projectService.GetAllUserProjects(appUser.Id);
        return Ok(results);
    }
}