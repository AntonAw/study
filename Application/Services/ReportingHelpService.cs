﻿using Application.Services.Abstarctions;
using Domain.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.Services;
public class ReportingHelpService(IThemeRepository _themeRepository, ITelegramBotClient _client) : IReportingHelpService
{
    public async Task ErrorInReportAsync(CallbackQuery query, string command, CancellationToken cancellationToken)
    {
        await InternalSendMessage(query, command, cancellationToken);
    }

    public async Task ExcelErrorAsync(CallbackQuery query, string command, CancellationToken cancellationToken)
    {
        await InternalSendMessage(query, command, cancellationToken);
    }

    public async Task СlarificationAsync(CallbackQuery query, string command, CancellationToken cancellationToken)
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