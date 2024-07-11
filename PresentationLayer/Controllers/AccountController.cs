using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Managment_API.DataLayer.Entities;
using Task_Managment_API.ServiceLayer.Dto.AccountDtos;
using Task_Managment_API.ServiceLayer.IService;

namespace Task_Managment_API.PresentationLayer.Controllers;

[ApiController]
[Route("api/user")]
public class AccountController(
    UserManager<User> userManager,
    IAccountService accountService,
    RoleManager<IdentityRole> roleManager,
    SignInManager<User> signInManager) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var appUser = new User
        {
            UserName = registerDto.Username,
            Email = registerDto.Email
        };
        var createUser = await userManager.CreateAsync(appUser, registerDto.Password);
        if (!createUser.Succeeded)
        {
            return StatusCode(500, createUser.Errors);
        }

        const string roleName = "User";
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var roleResult = await userManager.AddToRoleAsync(appUser, roleName);
        if (!roleResult.Succeeded)
        {
            return StatusCode(500, roleResult.Errors);
        }

        return Ok(new UserDto
        {
            UserName = appUser.UserName,
            Email = appUser.Email,
            Token = accountService.CreateToken(appUser)
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);
        if (user == null) return Unauthorized("Invalid username!");

        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized("Invalid username or password!");
        return Ok(new UserDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = accountService.CreateToken(user)
        });
    }

    [HttpGet("Profile")]
    public async Task<IActionResult> Profile(string username)
    {
        // 3shan law el data not valid zay username fih special characters m3ady 16 kdah  3shan kda msh htrg3 200  w htrg3 bad request
        if (!ModelState.IsValid) return BadRequest(ModelState);
        //want to get  user by id 
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if (user == null) return Unauthorized("Invalid username!");
        return Ok(new UserDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = accountService.CreateToken(user)
        });
    }

    [HttpPut("UpdateProfile")]
    public async Task<IActionResult> UpdateProfile(string username, [FromBody] UpdateProfileDto updateProfileDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if (user == null) return Unauthorized("Invalid username!");

        // Check and update username if a new one is provided and it's different
        if (!string.IsNullOrWhiteSpace(updateProfileDto.UserName) && user.UserName != updateProfileDto.UserName)
        {
            user.UserName = updateProfileDto.UserName;
        }

        // Check and update email if a new one is provided and it's different
        if (!string.IsNullOrWhiteSpace(updateProfileDto.Email) && user.Email != updateProfileDto.Email)
        {
            user.Email = updateProfileDto.Email;
        }

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded) return StatusCode(500, result.Errors);

        return Ok(new UpdateProfileDto
        {
            UserName = user.UserName,
            Email = user.Email,
        });
    }
}