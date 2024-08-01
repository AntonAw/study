using Telegram.Bot.Types;

namespace Application.Services.Abstarctions;
public interface IUserService
{
    Task AddUserAsync(Update update, CancellationToken cancellationToken);
}