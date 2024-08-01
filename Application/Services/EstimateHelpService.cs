using Application.Services.Abstarctions;
using Domain.Domain.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services;
public class EstimateHelpService(IThemeRepository _themeRepository, ITelegramBotClient _client) : IEstimateHelpService
{
    public async Task HelpAsync(CallbackQuery query, string command, CancellationToken cancellationToken)
    {
        await InternalSendMessage(query, command, cancellationToken);
    }

    private async Task InternalSendMessage(CallbackQuery query, string command, CancellationToken cancellationToken)
    {
        var form = await _themeRepository.GetReportFormByThemeCommandAsync(command, cancellationToken);

        var replyMarkup = new ForceReplyMarkup() { Selective = true };
        await _client.SendTextMessageAsync(chatId: query.Message!.Chat,
                                           text: form.Form,
                                           replyMarkup: replyMarkup,
                                           cancellationToken: cancellationToken);
    }
}