using Application.Services.Abstarctions;
using Domain.Domain.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services;
public class UploadHelpService(IThemeRepository _themeRepository, ITelegramBotClient _client) : IUploadHelpService
{
    public Task DeleteDataInLoading(CallbackQuery query, string command, CancellationToken cancellationToken) => throw new NotImplementedException();

    public async Task EnterpriceDataLoading(CallbackQuery query, string command, CancellationToken cancellationToken)
    {
        await InternalSendMessage(query, command, cancellationToken);
    }

    public async Task EnterpriceDataLoadingErrorCancel(CallbackQuery query, string command, CancellationToken cancellationToken)
    {
        await InternalSendMessage(query, command, cancellationToken);
    }

    public async Task EnterpriceDataLoadingNoStart(CallbackQuery query, string command, CancellationToken cancellationToken)
    {
        await InternalSendMessage(query, command, cancellationToken);
    }

    public async Task UpdateDataInLoading(CallbackQuery query, string command, CancellationToken cancellationToken)
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