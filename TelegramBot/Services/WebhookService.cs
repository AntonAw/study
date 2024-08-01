using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace TelegramBot.Services;

public class WebhookService : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public WebhookService(
        IConfiguration configuration,
        IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var webhook = $"{_configuration["TelegramBot:WebhookUrl"]}/{_configuration["TelegramBot:BotRoute"]}";
        await client.SetWebhookAsync(url: webhook,
            allowedUpdates: new[] { UpdateType.Message, UpdateType.CallbackQuery, UpdateType.Unknown },
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}