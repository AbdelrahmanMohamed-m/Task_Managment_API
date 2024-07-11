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

    public async Task<bool> RemoveUserFromProject(ProjectsCollaborators projectsCollaborators, int userProjectId)
    {
        var findEntityById = await _context.UserProjects.FindAsync(userProjectId);
        if (findEntityById == null)
        {
            return false;
        }

        _context.UserProjects.Remove(findEntityById);
        await _context.SaveChangesAsync();
        return true;
    }

   
}