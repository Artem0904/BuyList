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

namespace BusinessLogic.Services.BotServices
{
    public static class BotMessageService
    {
        public static async Task RequestPhone(this BotBackgroundService botBackgroundService, ITelegramBotClient botClient,  Update update, CancellationToken cancellationToken)
        {
            if (update.Message != null)
            {
                var chatId = update.Message.Chat.Id;
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
                    if (contact == null)
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
                        await botBackgroundService.accountService.AddAsync(newBotUser);
                        await BotMenuService.SendMainMenu(botClient, chatId);
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
      
    }
}
