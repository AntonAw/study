using Telegram.Bot.Types;

namespace Application.Services.Abstarctions;
public interface IReplyService
{
    Task HandleAsync(Update update, CancellationToken cancellationToken);
}