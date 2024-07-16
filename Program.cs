using System.Text;
using api.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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

// 

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

builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseResponseCaching();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

