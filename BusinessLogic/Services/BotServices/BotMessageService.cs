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
using BusinessLogic.Interfaces;
using Telegram.Bot.Types.Enums;
using BusinessLogic.Models.PurchaseModels;
using BusinessLogic.Enums.ButtonTags;
using BusinessLogic.Enums.BotStates;
using BusinessLogic.Models.BalanceModels;

namespace BusinessLogic.Services.BotServices
{
    public static class BotMessageService
    {
        public static async Task RequestPhone(this BotBackgroundService botBackgroundService, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                        await botClient.SendMessage(chatId, $"Ваш баланс: {await botBackgroundService.GetUserBalance(chatId)}");
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
                    await botClient.SendMessage(chatId, $"Ваш баланс: {await botBackgroundService.GetUserBalance(chatId)}");
                    await BotMenuService.SendMainMenu(botClient, chatId);
                }
            }

        }

        public static async Task AddPurchase(this BotBackgroundService botBackgroundService, ITelegramBotClient botClient, Update update, BasePurchaseModel createModel)
        {
            if (update.Message != null)
            {
                var chatId = update.Message.Chat.Id;
                var message = update.Message;
                using (var scope = botBackgroundService.serviceScopeFactory.CreateScope())
                {
                    var purchaseSevice = scope.ServiceProvider.GetService<IPurchaseService>();
                    var botUserService = scope.ServiceProvider.GetService<IBotUserService>();
                    var balanceService = scope.ServiceProvider.GetService<IBalanceService>();

                    var botUser = await botUserService!.GetByChatIdAsync(chatId);
                    var bal = await balanceService!.GetByUserIdAsync(botUser.Id);
                    await balanceService.CreateAsync(new BaseBalanceModel{ Money = botUser.Balance - createModel.Price, Currency = bal.Currency, UserId = botUser.Id});
                    await purchaseSevice!.CreateAsync(createModel, botUser.Id);
                }
            }
        }
        public static async Task HandleStates(this BotBackgroundService botBackgroundService, ITelegramBotClient botClient, Update update)
        {
            if (update.Message != null)
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
                            await BotMenuService.SendOneButtonMenu(botClient, chatId, "Відміна", MainMenuButtonTag.main_menu, "✅ Ціну прийнято! Тепер введіть опис покупки.");
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

                            await AddPurchase(botBackgroundService, botClient, update, botBackgroundService.newPurchase);
                            botBackgroundService.newPurchase = new BasePurchaseModel();
                            await botClient.SendMessage(chatId, "✅ Покупку успішно додано!");
                            botBackgroundService.userStates.TryRemove(userId, out _);
                            await botClient.SendMessage(chatId, $"Ваш баланс: {await botBackgroundService.GetUserBalance(chatId)}");
                            await BotMenuService.SendMainMenu(botClient, chatId);
                            return;
                        }
                    }
                    else if (state == BotState.WaitingForCurrency)
                    {
                        if (!String.IsNullOrEmpty(message.Text))
                        {
                            botBackgroundService.newBalance.Currency = message.Text;
                            botBackgroundService.userStates[userId] = BotState.WaitingForBalance; // Переходимо до наступного кроку
                            await botClient.SendMessage(chatId, "Тепер введіть ваш баланс (число):");
                            return;
                        }
                    }
                    else if (state == BotState.WaitingForBalance)
                    {
                        if (decimal.TryParse(message.Text, out decimal money))
                        {
                            botBackgroundService.newBalance.Money = money;
                            using (var scope = botBackgroundService.serviceScopeFactory.CreateScope())
                            {
                                var balanceSevice = scope.ServiceProvider.GetService<IBalanceService>();
                                var botUserService = scope.ServiceProvider.GetService<IBotUserService>();

                                var botUser = await botUserService!.GetByChatIdAsync(chatId);
                                botBackgroundService.newBalance.UserId = botUser.Id;
                                await balanceSevice!.CreateAsync(botBackgroundService.newBalance);
                            }
                            botBackgroundService.userStates.TryRemove(userId, out _);    
                            await botClient.SendMessage(chatId, "Реєстрацію завершено!");
                            await botClient.SendMessage(chatId, $"Ваш баланс: {await botBackgroundService.GetUserBalance(chatId)}");
                            await BotMenuService.SendMainMenu(botClient, chatId);
                            botBackgroundService.newBalance = new BaseBalanceModel();
                        }
                        else
                        {
                            await botClient.SendMessage(chatId, "❌ Будь ласка, введіть коректний баланс (число).");
                        }
                        return;
                    }
                }
            }
        }
        public static async Task SetBalance(this BotBackgroundService botBackgroundService, ITelegramBotClient botClient, Message message, CurrencyButtonTag currencyButtonTag)
        {
            if (message != null)
            {
                var chatId = message.Chat.Id;
                var userId = chatId;
                if (botBackgroundService.userStates.TryGetValue(userId, out var state))
                {
                    if (state == BotState.WaitingForCurrency)
                    {
                        if (!String.IsNullOrEmpty(message.Text))
                        {
                            botBackgroundService.newBalance.Currency = currencyButtonTag.ToString();
                            botBackgroundService.userStates[userId] = BotState.WaitingForBalance; // Переходимо до наступного кроку
                            await botClient.SendMessage(chatId, "Тепер введіть ваш баланс (число):");
                            return;
                        }
                    }
                    else if (state == BotState.WaitingForBalance)
                    {
                        if (decimal.TryParse(message.Text, out decimal money))
                        {
                            botBackgroundService.newBalance.Money = money;
                            using (var scope = botBackgroundService.serviceScopeFactory.CreateScope())
                            {
                                var balanceSevice = scope.ServiceProvider.GetService<IBalanceService>();
                                var botUserService = scope.ServiceProvider.GetService<IBotUserService>();

                                var botUser = await botUserService!.GetByChatIdAsync(chatId);
                                botBackgroundService.newBalance.UserId = botUser.Id;
                                await balanceSevice!.CreateAsync(botBackgroundService.newBalance);
                            }
                            botBackgroundService.userStates.TryRemove(userId, out _);
                            await botClient.SendMessage(chatId, "Баланс успішно прйнято!");
                            botBackgroundService.newBalance = new BaseBalanceModel();
                        }
                        else
                        {
                            await botClient.SendMessage(chatId, "❌ Будь ласка, введіть коректний баланс (число).");
                        }
                        return;
                    }
                }
            }
        }
    }
}
