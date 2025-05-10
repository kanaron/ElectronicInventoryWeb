using Application.PurchaseSuggestion;
using Infrastructure.TmeTokenEncryptionService;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers;

public class PurchaseController : BaseApiController
{
    private readonly AppDbContext _context;
    private readonly ITmeTokenEncryptionService _tokenEncryptionService;

    public PurchaseController(AppDbContext context, ITmeTokenEncryptionService tokenEncryptionService)
    {
        _context = context;
        _tokenEncryptionService = tokenEncryptionService;
    }


    [HttpGet("[action]")]
    public async Task<IActionResult> PurchaseSuggestions(CancellationToken ct)
    {
        var userId = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token");

        var user = await _context.Users.FindAsync(userId);

        if (user == null || string.IsNullOrEmpty(user.TmeToken))
            return Unauthorized("No TME token found for the user");

        var decryptedToken = _tokenEncryptionService.Decrypt(user.TmeToken);

        var suggestions = await Mediator.Send(new GetPurchaseSuggestions.Query
        {
            UserId = userId,
            Token = decryptedToken
        }, ct);

        return Ok(suggestions);
    }
}
