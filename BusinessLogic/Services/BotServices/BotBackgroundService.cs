using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Types;
using Telegram.Bot;
using BusinessLogic.Services.BotServices;
using Telegram.Bot.Types.Enums;
using BusinessLogic.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;
using BusinessLogic.Interfaces.BotInterfaces;
using System.Data.Entity;

public class BotBackgroundService : BackgroundService
{
    private readonly TelegramBotClient client;
    public readonly IServiceScopeFactory serviceScopeFactory;
    public string BotToken { get; private set; }
    public BotBackgroundService(IConfiguration config, 
        IServiceScopeFactory scopeFactory,
        IServiceScopeFactory serviceScopeFactory)
    {
        BotToken = config["TelegramBot:Token"]!;
        this.client = new TelegramBotClient(BotToken);
        this.serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.CallbackQuery != null)
        {
            if (update.CallbackQuery.Message != null)
            {
                await botClient.DeleteMessage(
                    chatId: update.CallbackQuery.Message.Chat.Id,
                    messageId: update.CallbackQuery.Message.MessageId,
                    cancellationToken: cancellationToken
                );
            }
            await HandleCallbackQuery(botClient, update.CallbackQuery);
            return;
        }
        if (update.Message != null)
        {
            var chatId = update.Message.Chat.Id;
            var message = update.Message;
            switch (message.Type)
            {
                case MessageType.Text:
                switch (message.Text)
                { 
                    case "/start":
                        await this.RequestPhone(botClient, update, cancellationToken);
                        break;
                    default:
                        await botClient.SendMessage(chatId, $"Ви надіслали: {message.Text}");
                        break;
                }
                    break;
                case MessageType.Contact:
                    await this.SaveBotUser(botClient, update, cancellationToken);
                    break;
            }
        }
    }

    private async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        var chatId = callbackQuery.Message.Chat.Id;
        var data = callbackQuery.Data; // Отримуємо значення callback_data

        switch (data)
        {
            case "add_purchase":
                await BotMenuService.SendAddPurchaseMenu(botClient, chatId);
                break;

            case "purchase_history":
                await BotMenuService.SendMyPurchaseMenu(botClient, chatId);
                break;

            case "main_menu":
                await BotMenuService.SendMainMenu(botClient, chatId);
                break;

            default:
                await botClient.SendMessage(chatId, "🔍 Невідома команда.");
                break;
        }

        //await botClient.EditMessageReplyMarkup(chatId, callbackQuery.Message.MessageId, replyMarkup: null);
    }


    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Помилка: {exception.Message}");
        return Task.CompletedTask;
    }
    public async Task<bool> IsUserExist(long id)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var userService = scope.ServiceProvider.GetService<IBotUserService>();
            return await userService!.GetByChatIdAsync(id) != null;
        }
    }
    public InlineKeyboardButton[][] CreateInlineButtons(Dictionary<string, string> data, int colums)
    {
        return data.AsParallel().Select(x => InlineKeyboardButton.WithCallbackData(text: x.Key, callbackData: x.Value))
                   .Select((button, index) => new { Button = button, Index = index })
                   .GroupBy(x => x.Index / colums)
                   .Select(g => g.Select(x => x.Button).ToArray())
                   .ToArray();
    }


}
