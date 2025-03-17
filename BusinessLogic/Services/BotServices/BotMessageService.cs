using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Types.ReplyMarkups;

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
                    
                }
                else
                {
                    
                }
            }
        }
    }
}
