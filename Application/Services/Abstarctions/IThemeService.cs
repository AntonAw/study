using Telegram.Bot.Types;

namespace Application.Services.Abstarctions;
public interface IThemeService
{
    Task SendTopics(Update update, CancellationToken cancellationToken);
    Task SendCurrentTopicThemes(ChatId chatId, string pattern, CancellationToken cancellationToken);
}