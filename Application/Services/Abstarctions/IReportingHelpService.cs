using Telegram.Bot.Types;

namespace Application.Services.Abstarctions;
public interface IReportingHelpService
{
    Task ErrorInReportAsync(CallbackQuery query, string command, CancellationToken cancellationToken);
    Task СlarificationAsync(CallbackQuery query, string command, CancellationToken cancellationToken);
    Task ExcelErrorAsync(CallbackQuery query, string command, CancellationToken cancellationToken);
}