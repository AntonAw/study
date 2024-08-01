using Application.Services.Abstarctions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Services.Abstraction;

namespace TelegramBot.Services;

public class UpdateHandler(IThemeService _themeService,
                           IUploadHelpService _uploadService,
                           ITelegramBotClient _client,
                           IUserService _userService,
                           IReplyService _replyService,
                           ISalaryHelpService _salaryService,
                           IReportingHelpService _reportingService,
                           IEstimateHelpService _estimateService) : IUpdateHandler
{
    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { Chat.Type: ChatType.Private } } and { Message: { Text: "/set_phone" } } => StartHandle(update, cancellationToken),
            { Message: { Chat.Type: ChatType.Private } } and { Message: { Text: "/start" } } => StartHandle(update, cancellationToken),
            { Message: { Chat.Type: ChatType.Private } } and { Message: { Text: "/theme" } } => ThemeHandle(update, cancellationToken),
            { Type: UpdateType.CallbackQuery } => CallbackHandleInternalAsync(update, cancellationToken),
            { Message.ReplyToMessage.Text: not null } => ReplyHandleAsync(update, cancellationToken),
            _ => UnknownHandle(update.Message.Chat, cancellationToken),
        };

        await handler;
    }

    public async Task StartHandle(Update update, CancellationToken cancellationToken)
    {
        var replyMarkup = new ForceReplyMarkup() { Selective = true };
        await _client.SendTextMessageAsync(chatId: update.Message!.Chat,
                                           text: "Добрый день! Чтобы я мог вам помочь отправьте мне свой рабочий номер телефона ответом на данное сообщение",
                                           replyMarkup: replyMarkup,
                                           cancellationToken: cancellationToken);
    }

    private async Task ThemeHandle(Update update, CancellationToken cancellationToken)
    {
        await _themeService.SendTopics(update, cancellationToken);
    }

    private async Task CallbackHandleInternalAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { CallbackQuery.Data: "/upload" } => _themeService.SendCurrentTopicThemes(update.CallbackQuery.Message.Chat, "/upload", cancellationToken),
            { CallbackQuery.Data: "/reporting" } => _themeService.SendCurrentTopicThemes(update.CallbackQuery.Message.Chat, "/reporting", cancellationToken),

            { CallbackQuery.Data: "/estimate" } => _estimateService.HelpAsync(update.CallbackQuery, "/estimate", cancellationToken),
            { CallbackQuery.Data: "/salary" } => _salaryService.HelpAsync(update.CallbackQuery, "/salary", cancellationToken),

            { CallbackQuery.Data: "/upload/1" } => _uploadService.EnterpriceDataLoading(update.CallbackQuery, "/upload/1", cancellationToken),
            { CallbackQuery.Data: "/upload/2" } => _uploadService.DeleteDataInLoading(update.CallbackQuery, "/upload/2", cancellationToken),
            { CallbackQuery.Data: "/upload/3" } => _uploadService.UpdateDataInLoading(update.CallbackQuery, "/upload/3", cancellationToken),
            { CallbackQuery.Data: "/upload/4" } => _uploadService.EnterpriceDataLoadingNoStart(update.CallbackQuery, "/upload/4", cancellationToken),
            { CallbackQuery.Data: "/upload/5" } => _uploadService.EnterpriceDataLoadingErrorCancel(update.CallbackQuery, "/upload/5", cancellationToken),

            { CallbackQuery.Data: "/reporting/1" } => _reportingService.ErrorInReportAsync(update.CallbackQuery, "/reporting/1", cancellationToken),
            { CallbackQuery.Data: "/reporting/2" } => _reportingService.СlarificationAsync(update.CallbackQuery, "/reporting/2", cancellationToken),
            { CallbackQuery.Data: "/reporting/3" } => _reportingService.ExcelErrorAsync(update.CallbackQuery, "/reporting/3", cancellationToken),
            _ => UnknownHandle(update.CallbackQuery.Message.Chat, cancellationToken)
        }; ;

        await handler;
    }

    private async Task ReplyHandleAsync(Update update, CancellationToken cancellationToken)
    {
        if (update.Message!.ReplyToMessage!.Text == "Добрый день! Чтобы я мог вам помочь отправьте мне свой рабочий номер телефона ответом на данное сообщение")
        {
            await SetPhone(update, cancellationToken);
            return;
        }

        await _replyService.HandleAsync(update, cancellationToken);
    }

    private async Task SetPhone(Update update, CancellationToken cancellationToken)
    {
        await _userService.AddUserAsync(update, cancellationToken);
        return;
    }

    private async Task UnknownHandle(ChatId chatId, CancellationToken cancellationToken)
    {
        await _client.SendTextMessageAsync(chatId: chatId, text: "Я вас не понял :(", cancellationToken: cancellationToken);
    }
}
