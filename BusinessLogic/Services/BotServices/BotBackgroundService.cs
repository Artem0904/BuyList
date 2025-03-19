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
using System.Collections.Concurrent;
using BusinessLogic.Models.PurchaseModels;
using BusinessLogic.Enums;
namespace BusinessLogic.Services.BotServices
{
    public class BotBackgroundService : BackgroundService
    {
        private readonly TelegramBotClient client;
        public readonly IServiceScopeFactory serviceScopeFactory;
        public string BotToken { get; private set; }
        public  ConcurrentDictionary<long, BotState> userStates = new();
        public BasePurchaseModel newPurchase = new BasePurchaseModel();

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
            int lastCallBackQueryMessageId = 0;

            if (update.CallbackQuery != null)
            {
                if (update.CallbackQuery.Message != null)
                {
                    lastCallBackQueryMessageId = update.CallbackQuery.Message.MessageId;
                    await DeleteMessage(botClient, update.CallbackQuery.Message.Chat.Id, lastCallBackQueryMessageId, cancellationToken);
                }
                await HandleCallbackQuery(botClient, update.CallbackQuery);
                return;
            }
            if (update.Message != null)
            {
                var chatId = update.Message.Chat.Id;
                var userId = chatId;
                var message = update.Message;
                if (userStates.Keys.Contains(userId))
                {
                    await this.HandleStates(botClient, update, cancellationToken);
                    return;
                }

                
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
                                await BotMenuService.SendMainMenu(botClient, chatId);
                            break;
                        }
                    break;
                    case MessageType.Contact:
                        await this.SaveBotUser(botClient, update, cancellationToken);
                        var removeKeyboard = new ReplyKeyboardRemove();
                        await botClient.SendMessage(chatId, "Реєстрацію завершено!", replyMarkup: removeKeyboard);
                        await BotMenuService.SendMainMenu(botClient, chatId);
                        break;
                }
            }
        }

        private async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if(callbackQuery.Message != null)
            {
                var chatId = callbackQuery.Message.Chat.Id;
                var userId = chatId;
                var data = callbackQuery.Data;

                switch (data)
                {
                    case nameof(ButtonTag.add_purchase):
                        userStates[userId] = BotState.WaitingForPrice;
                        await BotMenuService.SendOneButtonMenu(botClient, chatId, "Відміна", ButtonTag.main_menu, "💰 Введіть ціну покупки:");
                        break;

                    case nameof(ButtonTag.purchase_history):
                        await BotMenuService.SendMyPurchaseMenu(botClient, chatId);
                        break;

                    case nameof(ButtonTag.main_menu):
                        userStates.TryRemove(userId, out _);
                        await BotMenuService.SendMainMenu(botClient, chatId);
                        break;

                    default:
                        await botClient.SendMessage(chatId, "🔍 Невідома команда.");
                        break;
                }
            }

            //await botClient.EditMessageReplyMarkup(chatId, callbackQuery.Message.MessageId, replyMarkup: null);
        }


        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Помилка: {exception.Message}");
            return Task.CompletedTask;
        }
        private async Task DeleteMessage(ITelegramBotClient botClient, long chatId, int messageId, CancellationToken cancellationToken)
        {
            await botClient.DeleteMessage(
                        chatId: chatId,
                        messageId: messageId,
                        cancellationToken: cancellationToken
                    );
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
}

