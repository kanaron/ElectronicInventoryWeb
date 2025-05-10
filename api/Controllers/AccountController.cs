using API.Models;
using Application.Account;
using Application.Interfaces;
using Domain.Data;
using Domain.Dto;
using Infrastructure.TmeTokenEncryptionService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : BaseApiController
{
    private readonly UserManager<User> userManager;
    private readonly ITokenService tokenService;
    private readonly ITmeTokenEncryptionService tmeTokenEncryptionService;
    private readonly SignInManager<User> signInManager;

    public AccountController(UserManager<User> _userManager, ITokenService _tokenService, ITmeTokenEncryptionService _tmeTokenEncryptionService, SignInManager<User> _signInManager)
    {
        userManager = _userManager;
        tokenService = _tokenService;
        tmeTokenEncryptionService = _tmeTokenEncryptionService;
        signInManager = _signInManager;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<LoginModel>> Register([FromBody] RegisterModel model)
    {
        try
        {
            if (await userManager.Users.AnyAsync(x => x.UserName == model.UserName))
            {
                return BadRequest("User already taken");
            }

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

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginModel>> Login([FromBody] LoginModel model)
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

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<LoginModel>> GetCurrentUser()
    {
        var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

        if (user == null) return NotFound();

        var decrypted = tmeTokenEncryptionService.Decrypt(user.TmeToken);

        return Ok(new UserDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = tokenService.CreateToken(user),
            TmeToken = decrypted
        });
    }

    [Authorize]
    [HttpPut]
    [Route("UpdateTmeToken")]
    public async Task<ActionResult<UserDto>> UpdateUpdateTmeToken([FromBody] UserDto user, CancellationToken cancellationToken)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        await Mediator.Send(new UpdateTmeToken.Command { UserDto = user, Id = userId! }, cancellationToken);

        return Ok();
    }
}
