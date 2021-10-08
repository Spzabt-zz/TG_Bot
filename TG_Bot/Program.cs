using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TG_Bot
{
    class Program
    {
        private static string Token { get; set; } = "str";
        private static ITelegramBotClient _botClient;
        //private static TelegramBotClient _botClient;
        private static Random _random;
        private static CurrencyPairsParser _currencyPairs;

        static void Main(string[] args)
        {
            _botClient = new TelegramBotClient(Token);
            _currencyPairs = new CurrencyPairsParser();
            _botClient.StartReceiving();
            _botClient.OnMessage += OnMessageHandler;
            Console.ReadLine();
            _botClient.StopReceiving();
        }

        private static async void OnMessageHandler(object? sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg.Text == null) return;
            string valuePairInput = string.Empty;
            // await _botClient.SendTextMessageAsync(
            //     msg.Chat.Id,
            //     msg.Text,
            //     parseMode: ParseMode.Markdown,
            //     disableNotification: true,
            //     replyMarkup: GetButtons());
            //Console.WriteLine($"Msg: {msg.Text}");
            Console.WriteLine($"{msg.From.FirstName} sent message {msg.MessageId}" +
                              $"to chat {msg.Chat.Id} at {msg.Date.ToLocalTime()}. " +
                              $"It is a reply to message {msg.MessageId} " +
                              $"and has {msg.Text.Length} message entities.");
            switch (msg.Text)
            {
                case "Send sticker pls!":
                    _random = new Random();
                    var stickers = new[]
                    {
                        $"https://cdn.tlgrm.app/stickers/c62/4a8/c624a88d-1fe3-403a-b41a-3cdb9bf05b8a/192/{_random.Next(1, 49)}.webp",
                        $"https://cdn.tlgrm.app/stickers/c62/4a8/c624a88d-1fe3-403a-b41a-3cdb9bf05b8a/192/{_random.Next(1, 49)}.webp",
                        $"https://cdn.tlgrm.app/stickers/c62/4a8/c624a88d-1fe3-403a-b41a-3cdb9bf05b8a/192/{_random.Next(1, 49)}.webp"
                    };
                    foreach (var sticker in stickers)
                        await _botClient.SendStickerAsync(
                            chatId: msg.Chat.Id,
                            sticker: sticker,
                            //replyToMessageId: msg.MessageId,
                            replyMarkup: GetButtons()
                        );

                    break;
                case "Send pic":
                    await _botClient.SendPhotoAsync(
                        msg.Chat.Id,
                        photo:
                        "https://www.komar.de/ru/media/catalog/product/cache/7/image/9df78eab33525d08d6e5fb8d27136e95/D/1/D1-017_1583159159.jpg",
                        //replyToMessageId: msg.MessageId,
                        replyMarkup: GetButtons());
                    break;
                case "Currency pairs":
                    var currencies = new[]
                    {
                        "USD", "EUR", "RUR", "BTC"
                    };
                    await _botClient.SendTextMessageAsync(
                        msg.Chat.Id,
                        text: "From privat24 API");
                    foreach (var currency in currencies)
                        await _botClient.SendTextMessageAsync(
                            msg.Chat.Id,
                            text: _currencyPairs.GetValuePairs(currency),
                            replyMarkup: GetButtons());
                    break;
                default:
                    await _botClient.SendTextMessageAsync(
                        msg.Chat.Id,
                        text: "Choose command!",
                        replyMarkup: GetButtons());
                    break;
            }
        }

        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>
                    {
                        new KeyboardButton {Text = "Send audio"},
                        new KeyboardButton {Text = "Send voice"},
                        new KeyboardButton {Text = "Send vid"}
                    },
                    new List<KeyboardButton>
                    {
                        new KeyboardButton {Text = "Send sticker pls!"},
                        new KeyboardButton {Text = "Send pic"},
                        new KeyboardButton {Text = "Currency pairs"}
                    }
                },
                OneTimeKeyboard = true, ResizeKeyboard = true
            };
        }
    }
}