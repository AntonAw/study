using Application.Services.Abstarctions;
using Domain.Domain;
using Domain.Domain.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services;
public class ThemeService(IThemeRepository _themeRepository, ITelegramBotClient _client) : IThemeService
{
    public async Task SendTopics(Update update, CancellationToken cancellationToken)
    {
        var themes = await _themeRepository.GetAllAsync(cancellationToken);

        await _client.SendTextMessageAsync(chatId: update.Message!.Chat,
                                           text: "Инструкции по системе АФОТ расположены на сайте ЕК АСУТР аналитическая отчетность по фонду оплаты труда ЕК АСУТР." +
                                           "Выберите тему связанную с вашим вопросом:",
                                           replyMarkup: GetThemesButtonsAsync(themes.Where(t => t.ThemeCommand.Split("/").Count() == 2), cancellationToken),
                                           cancellationToken: cancellationToken);
    }

    public async Task SendCurrentTopicThemes(ChatId chatId, string pattern, CancellationToken cancellationToken)
    {
        var currentTheme = await _themeRepository.GetByCommandAsync(pattern, cancellationToken);
        var themes = await _themeRepository.GetAllAsync(cancellationToken);

        await _client.SendTextMessageAsync(chatId: chatId,
                                           text: currentTheme.Message,
                                           cancellationToken: cancellationToken);

        await _client.SendTextMessageAsync(chatId: chatId,
                                           text: "Выберите подходящую тему:",
                                           replyMarkup: GetThemesButtonsAsync(themes.Where(t => t.ThemeCommand.StartsWith(pattern + "/")), cancellationToken),
                                           cancellationToken: cancellationToken);
    }

    private static IReplyMarkup GetThemesButtonsAsync(IEnumerable<Theme> themes, CancellationToken cancellationToken)
    {
        var buttons = new List<InlineKeyboardButton[]>();
        for (int i = 0; i < (uint)themes.Count(); i++)
        {
            var theme = themes.ElementAt(i);

            buttons.Add(new[] { InlineKeyboardButton.WithCallbackData(theme.Name, theme.ThemeCommand) });
        }

        return new InlineKeyboardMarkup(buttons);
    }
}