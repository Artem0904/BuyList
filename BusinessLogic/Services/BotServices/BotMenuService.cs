using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Microsoft.Extensions.Hosting;
using BusinessLogic.Services.BotServices.Enums;

namespace BusinessLogic.Services.BotServices
{
    public static class BotMenuService
    {

        public static async Task SendMainMenu(ITelegramBotClient botClient, long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("📌 Мої покупки", nameof(ButtonTag.purchase_history)) },
                new[] { InlineKeyboardButton.WithCallbackData("📜 Додати покупку", nameof(ButtonTag.add_purchase)) }
            });

            await botClient.SendMessage(chatId, "📋 Головне меню:", replyMarkup: keyboard);
        }
        public static async Task SendMyPurchaseMenu(ITelegramBotClient botClient, long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("📜 Головне меню", nameof(ButtonTag.main_menu)) }
            });

            await botClient.SendMessage(chatId, "📋 ПОКУПКИ", replyMarkup: keyboard);
        }
        public static async Task SendAddPurchaseMenu(ITelegramBotClient botClient, long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("📜 Головне меню", nameof(ButtonTag.main_menu)) }
            });

            await botClient.SendMessage(chatId, "📋 Напишіть ціну та опис вашої покупки одним повідомленням\n[100.00 - Смачні спагетті]", replyMarkup: keyboard);
        }
        public static async Task SendOneButtonMenu(ITelegramBotClient botClient, long chatId, string buttonText, ButtonTag buttonTag, string message)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData(buttonText, nameof(buttonTag)) }
            });

            await botClient.SendMessage(chatId, message, replyMarkup: keyboard);
        }
    }
}
