using Application.Services.Abstarctions;
using Domain.Domain;
using Domain.Domain.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Application.Services;
public class ReplyService(IThemeRepository _themeRepository, ITelegramBotClient _client, IUserRepository _userRepository, IReportRepository _reportRepository) : IReplyService
{
    public async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        var themes = await _themeRepository.GetAllAsync(cancellationToken);

        var current = themes.Where(t => t.ReportForm != null)
                            .FirstOrDefault(t => t.ReportForm.Form == update.Message.ReplyToMessage.Text);

        if (current == null)
        {
            await _client.SendTextMessageAsync(chatId: update.Message.Chat,
                                               text: "Что-то пошло не так, перепроверьте следующие пункты: \nВы отправили заполненную форму \"ответом\" на не заполненную.",
                                               cancellationToken: cancellationToken);
            return;
        }

        var savedUser = await _userRepository.GetUserByChatIdAsync(update.Message.Chat.Id, cancellationToken);

        if (savedUser == null)
        {
            await _client.SendTextMessageAsync(chatId: update.Message.Chat,
                                   text: "Для создания обращения необходимо добавить свой номер телефона, это можно сделать при помощи команды: /set_phone.",
                                   cancellationToken: cancellationToken);

            return;
        }

        var id = await CreateReportAsync(update, savedUser, current, cancellationToken);

        await _client.SendTextMessageAsync(chatId: update.Message.Chat,
                                   text: $"Вами создано обращение: {id}. В течении рабочего дня Вам поступит звонок.",
                                   cancellationToken: cancellationToken);
    }

    private async Task<Guid> CreateReportAsync(Update update, Domain.Domain.User user, Theme theme, CancellationToken cancellationToken)
    {
        var report = new Report()
        {
            Id = Guid.NewGuid(),
            ReportTheme = theme,
            Message = update.Message.Text,
            User = user,
            Created = DateTime.UtcNow,
            ReportStatus = Domain.Domain.Enums.ReportStatus.New,
        };

        await _reportRepository.AddReportAsync(report, cancellationToken);

        return report.Id;
    }
}