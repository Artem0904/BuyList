using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Types;
using Telegram.Bot;
using BusinessLogic.Services.BotServices;
using Telegram.Bot.Types.Enums;

public class BotBackgroundService : BackgroundService
{
    private readonly TelegramBotClient _client;
    private readonly IServiceScopeFactory _scopeFactory;

    public BotBackgroundService(IConfiguration config, IServiceScopeFactory scopeFactory)
    {
        _client = new TelegramBotClient(config["TelegramBot:Token"]!);
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is { Text: { } text, Chat: { Id: var chatId } })
        {
            var message = update.Message;
            switch (message.Type)
            {
                case MessageType.Text:
                switch (message.Text)
                { 
                    case "/start":
                        await this.RequestPhone(botClient, update, cancellationToken);
                        break;
                }
                    break;
                case MessageType.Contact:
                    await this.SaveBotUser(botClient, update, cancellationToken);
                    break;
            }
            await botClient.SendTextMessageAsync(chatId, "Введіть опис покупки:");
        }
    }
    public async Task<bool> IsUserExist(long chatId)
    {

        return true;
    }
    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Помилка: {exception.Message}");
        return Task.CompletedTask;
    }


}
