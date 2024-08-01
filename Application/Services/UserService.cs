using Application.Helper;
using Application.Services.Abstarctions;
using Domain.Domain;
using Domain.Domain.Abstractions;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Application.Services;
public class UserService(IUserRepository _userRepository, ITelegramBotClient _client) : IUserService
{
    public static readonly Regex PhoneNumberRegex = new(
        @"^(?:\+7|8)\s?\(?\d{3}\)?[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1.5));

    public async Task AddUserAsync(Update update, CancellationToken cancellationToken)
    {
        var phone = update.Message.Text;
        if (!PhoneNumberRegex.IsMatch(phone))
        {
            await _client.SendTextMessageAsync(chatId: update.Message.Chat,
                                               text: "Введенный вами ответ не прошел валидацию. Введите телефон в формате: +7/8 (999) 999 99-99 ответов на сообщение выше",
                                               cancellationToken: cancellationToken);
            return;
        }

        var user = new Domain.Domain.User()
        {
            Id = Guid.NewGuid(),
            ChatId = update.Message!.Chat.Id,
            PhoneNumber = Convert.ToBase64String(AesHelper.Encrypt(Convert.FromBase64String(phone)))
        };

        await _userRepository.AddUserAsync(user, cancellationToken);

        await _client.SendTextMessageAsync(chatId: update.Message.Chat,
                                           text: "Ассоциация создана, для дальнейшей работы введите команду: /theme",
                                           cancellationToken: cancellationToken);
    }
}