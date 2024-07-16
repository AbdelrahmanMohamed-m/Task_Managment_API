using Microsoft.EntityFrameworkCore;
using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.DomainLayer.IRepo;

namespace Task_Managment_API.DomainLayer.Repo;

public class ProjectRepo : IProjectRepo
{
    private readonly ApplicationDbContext _context;

    public ProjectRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> UpdateProject(Project? project, int id)
    {
        var existingProject = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
        if (existingProject != null)
        {
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return existingProject;
    }

    public async Task<List<Project>> GetAllUserProjects(string id)
    {
        return await _context.Projects
            .Include(p => p.UserProject).Include(project => project.Tasks)
            .Where(p => p.UserProject.Any(up => up.UserId == id))
            .ToListAsync();
    }

    public async Task<Project?> CreateProject(Project project, string userId)
    {
        // Validate input
        if (project == null || string.IsNullOrEmpty(userId))
        {
            return null;
        }

        // Check if user exists
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return null; // or throw an exception indicating user not found
        }

        // Set timestamps
        project.CreatedAt = DateTime.UtcNow;
        project.UpdatedAt = DateTime.UtcNow;

        // Add project
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Add user-project relationship
        var userProject = new ProjectsCollaborators
        {
            UserId = userId,
            ProjectId = project.Id
        };
        _context.UserProjects.Add(userProject);

        try
        {
            // Save changes asynchronously
            await _context.SaveChangesAsync();
            return project;
        }
        catch (Exception ex)
        {
            // Handle any exceptions (e.g., concurrency issues, database errors)
            // Log the exception or handle it appropriately
            Console.WriteLine($"Error creating project: {ex.Message}");
            return null; // or throw an exception
        }
    }


    public async Task<bool> DeleteProject(int projectId)
    {
        // Retrieve the project
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
        {
            return false; // Project not found
        }

        // Retrieve related UserProjects and UserTasks
        var userProjects = _context.UserProjects.Where(up => up.ProjectId == projectId);
        var tasks = _context.Tasks.Where(t => t.ProjectId == projectId).ToList();

        foreach (var task in tasks)
        {
            var userTasks = _context.UserTasks.Where(ut => ut.TaskId == task.Id);
            _context.UserTasks.RemoveRange(userTasks);
        }

        // Remove UserProjects and UserTasks
        _context.UserProjects.RemoveRange(userProjects);
        _context.Tasks.RemoveRange(tasks);

        // Remove the project itself
        _context.Projects.Remove(project);

        try
        {
            // Save changes to the database
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Handle any exceptions (e.g., concurrency issues, database errors)
            // Log the exception or handle it appropriately
            Console.WriteLine($"Error deleting project: {ex.Message}");
            return false;
        }
    }

    
    
    public async Task<Project?> GetProjectById(int id)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .Include(p => p.UserProject)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}