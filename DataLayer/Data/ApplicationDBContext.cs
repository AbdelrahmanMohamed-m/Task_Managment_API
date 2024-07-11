using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.DataLayer.Entities;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<ProjectsCollaborators> UserProjects { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectsCollaborators>().HasKey(up => up.Id);
        modelBuilder.Entity<ProjectsCollaborators>().Property(e => e.Id).ValueGeneratedOnAdd();
        // UserProject relationship
        modelBuilder.Entity<ProjectsCollaborators>()
            .HasKey(up => new { up.UserId, up.ProjectId });

        modelBuilder.Entity<ProjectsCollaborators>()
            .HasOne(up => up.User)
            .WithMany(u => u.UserProjects)
            .HasForeignKey(up => up.UserId);

        modelBuilder.Entity<ProjectsCollaborators>()
            .HasOne(up => up.Project)
            .WithMany(p => p.UserProject)
            .HasForeignKey(up => up.ProjectId);


        modelBuilder.Entity<UserTask>().HasKey(ut => ut.Id);
        modelBuilder.Entity<UserTask>().Property(e => e.Id).ValueGeneratedOnAdd();
        // UserTask relationship
        modelBuilder.Entity<UserTask>()
            .HasKey(ut => new { ut.UserId, ut.TaskId});

        modelBuilder.Entity<UserTask>()
            .HasOne(ut => ut.User)
            .WithMany(u => u.UserTasks)
            .HasForeignKey(ut => ut.UserId);

        modelBuilder.Entity<UserTask>()
            .HasOne(ut => ut.Task)
            .WithMany(t => t.UserTasks)
            .HasForeignKey(ut => ut.TaskId);

        // Task and Project relationship
        modelBuilder.Entity<Tasks>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}