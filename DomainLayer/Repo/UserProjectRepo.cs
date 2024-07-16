using Microsoft.EntityFrameworkCore;
using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.DomainLayer.IRepo;

namespace Task_Managment_API.DomainLayer.Repo;

public class UserProjectRepo : IUserProjectRepo
{
    private readonly ApplicationDbContext _context;

    public UserProjectRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectsCollaborators> AssignUserToProject(ProjectsCollaborators projectsCollaborators,
        int projectId, string userId)
    {
        var existingAssignment = await _context.UserProjects
            .AnyAsync(up => up.ProjectId == projectId && up.UserId == userId);

        if (existingAssignment)
        {
            Console.WriteLine($"User {userId} is already assigned to project {projectId}.");
            return null; // Or handle as needed
        }

        var projectResult = await _context.Projects.FindAsync(projectId);

        var userResult = await _context.Users.FindAsync(userId);

        if (projectResult == null || userResult == null)
        {
            Console.WriteLine($"Project or User not found for ProjectId: {projectId}, UserId: {userId}");
            return null;
        }

        projectsCollaborators.ProjectId = projectResult.Id;
        projectsCollaborators.UserId = userResult.Id;


        await _context.UserProjects.AddAsync(projectsCollaborators);
        await _context.SaveChangesAsync();
        return projectsCollaborators;
    }

    public async Task<bool> RemoveUserFromProject(int projectId, string userId)
    {
        var projectsCollaborator = await _context.UserProjects
            .FirstOrDefaultAsync(pc => pc.ProjectId == projectId && pc.UserId == userId);

        if (projectsCollaborator == null)
        {
            Console.WriteLine($"Project collaborator not found for ProjectId: {projectId}, UserId: {userId}");
            return false;
        }

        _context.UserProjects.Remove(projectsCollaborator);
        await _context.SaveChangesAsync();
        Console.WriteLine($"Project collaborator removed for ProjectId: {projectId}, UserId: {userId}");
        return true;
    }
    
    public async Task<bool> CheckAssignmentExists(int projectId, string userId)
    {
        return await _context.UserProjects.AnyAsync(up => up.ProjectId == projectId && up.UserId == userId);
    }
}