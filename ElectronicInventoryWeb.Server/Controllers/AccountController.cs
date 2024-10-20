using ElectronicInventoryWeb.Server.Data;
using ElectronicInventoryWeb.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicInventoryWeb.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    public AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager)
    {
        userManager = _userManager;
        signInManager = _signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new User { UserName = model.UserName, Email = model.Email };
        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
            return Ok(new { Message = "Registration successful" });

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

        if (result.Succeeded)
            return Ok(new { Message = "Login successful" });

        return Unauthorized(new { Message = "Invalid login attempt" });
    }
}
