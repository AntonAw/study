using Telegram.Bot.Types;

namespace Application.Services.Abstarctions;
public interface IEstimateHelpService
{
    Task HelpAsync(CallbackQuery query, string command, CancellationToken cancellationToken);
}