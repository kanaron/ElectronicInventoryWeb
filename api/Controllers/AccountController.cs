using API.Interfaces;
using API.Models;
using Domain.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<User> userManager;
    private readonly ITokenService tokenService;
    private readonly SignInManager<User> signInManager;

    public AccountController(UserManager<User> _userManager, ITokenService _tokenService, SignInManager<User> _signInManager)
    {
        userManager = _userManager;
        tokenService = _tokenService;
        signInManager = _signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User { UserName = model.UserName, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(user, "User");

                return Ok(new NewUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = tokenService.CreateToken(user)
                });
            }

            return BadRequest(result.Errors);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName.ToLower());

        if (user == null) return Unauthorized("Invalid user name");

        var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (result.Succeeded)
            return Ok(new NewUser
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = tokenService.CreateToken(user)
            });

        return Unauthorized(new { Message = "Invalid login attempt" });
    }
}
