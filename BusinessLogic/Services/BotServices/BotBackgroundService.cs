using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Types;
using Telegram.Bot;
using BusinessLogic.Services.BotServices;
using Telegram.Bot.Types.Enums;
using BusinessLogic.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

public class BotBackgroundService : BackgroundService
{
    private readonly TelegramBotClient client;
    private readonly IBotUserService userService;
    public readonly IAccountService accountService;
    private readonly IServiceScopeFactory scopeFactory;
    public string BotToken { get; private set; }
    public BotBackgroundService(IConfiguration config, 
        IServiceScopeFactory scopeFactory,
        IBotUserService userService,
        IAccountService accountService)
    {
        BotToken = config["TelegramBot:Token"]!;
        this.scopeFactory = scopeFactory;
        this.client = new TelegramBotClient(BotToken);
        this.userService = userService;
        this.accountService = accountService;
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
            await HandleCallbackQuery(botClient, update.CallbackQuery);
            return;
        }

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
            await botClient.SendMessage(chatId, "Введіть опис покупки:");
        }
    }

    private async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        var chatId = callbackQuery.Message.Chat.Id;
        var data = callbackQuery.Data; // Отримуємо значення callback_data

        switch (data)
        {
            case "add_purchase":
                await botClient.SendMessage(chatId, "📝 Введіть опис покупки:");
                break;

            case "purchase_history":
                await botClient.SendMessage(chatId, "📜 Ось ваша історія покупок...");
                break;

            case "main_menu":
                await botClient.SendMessage(chatId, "❌ Ви вийшли з меню.");
                break;

            default:
                await botClient.SendMessage(chatId, "🔍 Невідома команда.");
                break;
        }

        // Видаляємо старе меню, щоб уникнути повторних натискань
        await botClient.EditMessageReplyMarkup(chatId, callbackQuery.Message.MessageId, replyMarkup: null);
    }


    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Помилка: {exception.Message}");
        return Task.CompletedTask;
    }
    public async Task<bool> IsUserExist(long id) => await userService.GetByChatIdAsync(id) != null;
    public InlineKeyboardButton[][] CreateInlineButtons(Dictionary<string, string> data, int colums)
    {
        return data.AsParallel().Select(x => InlineKeyboardButton.WithCallbackData(text: x.Key, callbackData: x.Value))
                   .Select((button, index) => new { Button = button, Index = index })
                   .GroupBy(x => x.Index / colums)
                   .Select(g => g.Select(x => x.Button).ToArray())
                   .ToArray();
    }


}
