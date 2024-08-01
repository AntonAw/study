using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBot.Services.Abstraction;

namespace TelegramBot.Controllers;

[Route("bot")]
[ApiController]
public class BotController : ControllerBase
{
    public async Task<IActionResult> Push([FromBody] Update update,
        [FromServices] IUpdateHandler handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}