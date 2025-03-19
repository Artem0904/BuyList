using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Types.ReplyMarkups;
using BusinessLogic.Models.UserModels;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Entity;
using BusinessLogic.Interfaces;
using Telegram.Bot.Types.Enums;
using BusinessLogic.Models.OrderModels;
using BusinessLogic.Services.BotServices.Enums;

namespace BusinessLogic.Services.BotServices
{
    public static class BotMessageService
    {
        public static async Task RequestPhone(this BotBackgroundService botBackgroundService, ITelegramBotClient botClient,  Update update, CancellationToken cancellationToken)
        {
            if (update.Message != null)
            {
                var chatId = update.Message.Chat.Id;
                if (update.Message.Chat.Type == ChatType.Private)
                {
                    var message = update.Message;
                    if (!await botBackgroundService.IsUserExist(chatId))
                    {
                        var shareContactButton = new KeyboardButton("Поділитися номером телефону") { RequestContact = true };

                        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[] { new[] { shareContactButton } })
                        {
                            ResizeKeyboard = true,
                            OneTimeKeyboard = true
                        };
                        await botClient.SendMessage(
                            chatId,
                            "Натисніть кнопку, щоб поділитися своїм номером телефону",
                            replyMarkup: replyKeyboardMarkup,
                            cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await botClient.SendMessage(
                        chatId,
                            $"Вітаємо: {message.Chat.FirstName} {message.Chat.LastName} !!!",
                            cancellationToken: cancellationToken);
                        await BotMenuService.SendMainMenu(botClient, chatId);

                    }
                }
                else
                {
                    await BotMenuService.SendMainMenu(botClient, chatId);
                }
            }
        }
        public static async Task SaveBotUser(this BotBackgroundService botBackgroundService, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message != null)
            {
                var chatId = update.Message.Chat.Id;
                var message = update.Message;
                if (!await botBackgroundService.IsUserExist(chatId))
                {
                    var contact = message.Contact;
                    if (contact != null)
                    {
                        string? userPhotoUrl = null;
                        var userPhotos = await botClient.GetUserProfilePhotos(
                            contact.UserId ?? chatId,
                            cancellationToken: cancellationToken);
                        if (userPhotos.TotalCount > 0)
                        {
                            var fileId = userPhotos.Photos[0][^1].FileId;
                            var file = await botClient.GetFile(fileId, cancellationToken: cancellationToken);
                            userPhotoUrl = $"https://api.telegram.org/file/bot{botBackgroundService.BotToken}/{file.FilePath}";
                        }
                        var newBotUser = new BaseBotUserModel()
                        {
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            UserName = message.Chat.Username,
                            ChatId = chatId,
                            ImageUrl = userPhotoUrl,
                            PhoneNumber = contact.PhoneNumber
                        };
                        using (var scope = botBackgroundService.serviceScopeFactory.CreateScope())
                        {
                            var accountService = scope.ServiceProvider.GetService<IAccountService>();
                            await accountService!.AddAsync(newBotUser);
                        }
                    }
                    else
                    {
                        await botClient.SendMessage(
                                chatId,
                                "Натисніть кнопку, щоб поділитися своїм номером телефону.Якщо кнопка відсутня - перезавантажте бот або відправте \"/start\"",
                                cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    await botClient.SendMessage(
                                chatId,
                                "Дякуємо.Але ви вже зареєстровані",
                                cancellationToken: cancellationToken);
                    await BotMenuService.SendMainMenu(botClient, chatId);
                }
            }
            
        }

        public static async Task AddPurchase(this BotBackgroundService botBackgroundService, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, BaseOrderModel createModel)
        {
            if (update.Message != null)
            {
                var chatId = update.Message.Chat.Id;
                var message = update.Message;
                using (var scope = botBackgroundService.serviceScopeFactory.CreateScope())
                {
                    var purchaseSevice = scope.ServiceProvider.GetService<IOrderService>();
                    var botUserService = scope.ServiceProvider.GetService<IBotUserService>();

                    var botUser = await botUserService.GetByChatIdAsync(chatId); 
                    await purchaseSevice!.CreateAsync(createModel, botUser.Id);
                }

            }
        }
        public static async Task HandleStates(this BotBackgroundService botBackgroundService, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var chatId = update.Message.Chat.Id;
            var userId = chatId;
            var message = update.Message;
            if (botBackgroundService.userStates.TryGetValue(userId, out var state))
            {
                if (state == BotState.WaitingForPrice)
                {
                    if (decimal.TryParse(message.Text, out decimal price))
                    {
                        botBackgroundService.newPurchase.Price = price;
                        botBackgroundService.userStates[userId] = BotState.WaitingForDescription; // Переходимо до наступного кроку
                        await botClient.SendMessage(chatId, "✅ Ціну прийнято! Тепер введіть опис покупки.");
                    }
                    else
                    {
                        await botClient.SendMessage(chatId, "❌ Будь ласка, введіть коректну ціну (число).");
                    }
                    return;
                }
                else if (state == BotState.WaitingForDescription)
                {
                    if (!String.IsNullOrEmpty(message.Text))
                    {
                        string description = message.Text;
                        botBackgroundService.newPurchase.Description = description;

                        await AddPurchase(botBackgroundService, botClient, update, cancellationToken, botBackgroundService.newPurchase);
                        botBackgroundService.newPurchase = new BaseOrderModel();
                        await botClient.SendMessage(chatId, "✅ Покупку успішно додано!");
                        botBackgroundService.userStates.TryRemove(userId, out _);
                        await BotMenuService.SendMainMenu(botClient, chatId);
                        return;
                    }
                }
            }
        }
    }
}
