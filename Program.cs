using System.Text;
using System.Threading.RateLimiting;
using api.Config;
using Bogus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Task_Managment_API.DataLayer.Entites;
using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.DomainLayer.ExceptionHandler;
using Task_Managment_API.DomainLayer.IRepo;
using Task_Managment_API.DomainLayer.Repo;
using Task_Managment_API.ServiceLayer.IService;
using Task_Managment_API.ServiceLayer.Service;

var builder = WebApplication.CreateBuilder(args);


//Database Configuration starts here
var dbConfig = new DataBaseConfig();
builder.Configuration.GetSection("DataBaseConfig").Bind(dbConfig);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        action => { action.CommandTimeout(dbConfig.TimeoutTime); });
    // only in development
    options.EnableDetailedErrors(dbConfig.DetailedError);
    options.EnableSensitiveDataLogging(dbConfig.SensitiveDataLogging);
});
// Database Configuration ends here

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IProjectRepo, ProjectRepo>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<ITasksRepo, TasksRepo>();
builder.Services.AddScoped<ITasksService, TasksService>();

builder.Services.AddScoped<IUserTaskRepo, UserTaskRepo>();
builder.Services.AddScoped<IUserTaskService, UserTaskService>();
builder.Services.AddScoped<IUserProjectRepo, UserProjectRepo>();
builder.Services.AddScoped<IUserProjectsService, UserProjectService>();


// Error Handling init in DI
 builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
 builder.Services.AddProblemDetails();

//  Rate Limiter
 builder.Services.AddRateLimiter(options =>
 {
     options.AddConcurrencyLimiter("ConcurrencyRateLimiter", opt =>
     {
         opt.PermitLimit = 20;
         opt.QueueLimit = 5;
         opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
     }).RejectionStatusCode = 429;
 });
 builder.Services.AddResponseCaching();


// Registering Repositories and Services
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredLength = 6;
    }).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
                options.DefaultScheme =
                    options.DefaultSignInScheme =
                        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:signingKey"])),
        };
    }
);


// Install: dotnet add package Bogus

builder.Services.AddResponseCaching();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

//     context.Database.EnsureCreated();

//     // Create test user if doesn't exist
//     var testUser = await userManager.FindByNameAsync("testuser");
//     if (testUser == null)
//     {
//         testUser = new User
//         {
//             UserName = "testuser",
//             Email = "test@example.com"
//         };
//         await userManager.CreateAsync(testUser, "password123");
//     }

//     // Add fake projects if none exist
//     if (!context.Projects.Any())
//     {
//         var projectFaker = new Faker<Project>()
//             .RuleFor(p => p.Name, f => f.Company.CompanyName() + " Project")
//             .RuleFor(p => p.Description, f => f.Lorem.Sentences(2))
//             .RuleFor(p => p.StartDate, f => f.Date.Past(1))
//             .RuleFor(p => p.EndDate, f => f.Date.Future(1))
//             .RuleFor(p => p.CreatedAt, f => f.Date.Past(2))
//             .RuleFor(p => p.UpdatedAt, f => f.Date.Recent());

//         var fakeProjects = projectFaker.Generate(5);
//         context.Projects.AddRange(fakeProjects);
//         await context.SaveChangesAsync();

//         // Add user-project relationships
//         foreach (var project in fakeProjects)
//         {
//             var userProject = new ProjectsCollaborators
//             {
//                 UserId = testUser.Id,
//                 ProjectId = project.Id
//             };
//             context.UserProjects.Add(userProject);
//         }
//         await context.SaveChangesAsync();
//     }
//     // Add fake tasks if none exist
// if (!context.Tasks.Any())
// {
//     // Get the projects we just created
//     var existingProjects = context.Projects.ToList();
    
//     var taskFaker = new Faker<Tasks>()
//         .RuleFor(t => t.Title, f => f.Hacker.Phrase())
//         .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
//         .RuleFor(t => t.Priority, f => f.PickRandom("Low", "Medium", "High", "Critical"))
//         .RuleFor(t => t.Status, f => f.PickRandom("Not Started", "In Progress", "Completed", "On Hold"))
//         .RuleFor(t => t.DueDate, f => f.Date.Future(2))
//         .RuleFor(t => t.CreatedAt, f => f.Date.Past(1))
//         .RuleFor(t => t.UpdatedAt, f => f.Date.Recent());

//     var fakeTasks = new List<Tasks>();
    
//     // Create 3-5 tasks per project
//     foreach (var project in existingProjects)
//     {
//         var tasksForProject = taskFaker.Generate(Random.Shared.Next(3, 6));
//         foreach (var task in tasksForProject)
//         {
//             task.ProjectId = project.Id;
//         }
//         fakeTasks.AddRange(tasksForProject);
//     }
    
//     context.Tasks.AddRange(fakeTasks);
//     await context.SaveChangesAsync();
    
//     // Add user-task relationships
//     foreach (var task in fakeTasks)
//     {
//         var userTask = new UserTask
//         {
//             UserId = testUser.Id,
//             TaskId = task.Id
//         };
//         context.UserTasks.Add(userTask);
//     }
//     await context.SaveChangesAsync();
// }
// }



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");
app.UseRateLimiter();
app.UseExceptionHandler();
app.UseResponseCaching();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

