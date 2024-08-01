using Telegram.Bot.Types;

namespace TelegramBot.Services.Abstraction;

public interface IUpdateHandler
{
    Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);
}
