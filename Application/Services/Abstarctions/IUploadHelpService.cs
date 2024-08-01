using Telegram.Bot.Types;

namespace Application.Services.Abstarctions;
public interface IUploadHelpService
{
    Task EnterpriceDataLoading(CallbackQuery query, string command, CancellationToken cancellationToken);
    Task DeleteDataInLoading(CallbackQuery query, string command, CancellationToken cancellationToken);
    Task UpdateDataInLoading(CallbackQuery query, string command, CancellationToken cancellationToken);
    Task EnterpriceDataLoadingNoStart(CallbackQuery query, string command, CancellationToken cancellationToken);
    Task EnterpriceDataLoadingErrorCancel(CallbackQuery query, string command, CancellationToken cancellationToken);
}