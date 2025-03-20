using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Microsoft.Extensions.Hosting;
using BusinessLogic.Enums.ButtonTags;

namespace BusinessLogic.Services.BotServices
{
    public static class BotMenuService
    {
        public static async Task SendMainMenu(ITelegramBotClient botClient, long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("📌 Мої покупки", nameof(MainMenuButtonTag.purchase_history)) },
                new[] { InlineKeyboardButton.WithCallbackData("📜 Додати покупку", nameof(MainMenuButtonTag.add_purchase)) }
            });

            await botClient.SendMessage(chatId, "📋 Головне меню:", replyMarkup: keyboard);
        }
        public static async Task SendMyPurchaseMenu(ITelegramBotClient botClient, long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("📜 Головне меню", nameof(MainMenuButtonTag.main_menu)) }
            });

            await botClient.SendMessage(chatId, "📋 ПОКУПКИ", replyMarkup: keyboard);
        }
        public static async Task SendAddPurchaseMenu(ITelegramBotClient botClient, long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("📜 Головне меню", nameof(MainMenuButtonTag.main_menu)) }
            });

            await botClient.SendMessage(chatId, "📋 Напишіть ціну та опис вашої покупки одним повідомленням\n[100.00 - Смачні спагетті]", replyMarkup: keyboard);
        }
        public static async Task SendOneButtonMenu(ITelegramBotClient botClient, long chatId, string buttonText, MainMenuButtonTag buttonTag, string message)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData(buttonText, buttonTag.ToString()) }
            });

            await botClient.SendMessage(chatId, message, replyMarkup: keyboard);
        }
        public static async Task SendChooseCurrencyMenu(ITelegramBotClient botClient, long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData(nameof(CurrencyButtonTag.Euro), nameof(CurrencyButtonTag.Euro)) },
                new[] { InlineKeyboardButton.WithCallbackData(nameof(CurrencyButtonTag.USD), nameof(CurrencyButtonTag.USD)) },
                new[] { InlineKeyboardButton.WithCallbackData(nameof(CurrencyButtonTag.UAN), nameof(CurrencyButtonTag.UAN)) },
            });

            await botClient.SendMessage(chatId, "Виберіть вашу валюту", replyMarkup: keyboard);
        }
    }
}
