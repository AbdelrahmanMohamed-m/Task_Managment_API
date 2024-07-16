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
        projectsCollaborators.ProjectId = projectId;
        projectsCollaborators.UserId = userId;


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
}