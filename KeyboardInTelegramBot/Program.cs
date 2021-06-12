using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace KeyboardInTelegramBot
{
    class Program
    {
        private static ITelegramBotClient client;
        static void Main(string[] args)
        {
            client = new TelegramBotClient("1866647860:AAEYVRCXFzsA1CzppeSqz2QBpa5bl6T9J-E");
            client.StartReceiving();
            Console.WriteLine("Бот стартанул.");

            client.OnMessage += Client_OnMessage;
            client.OnCallbackQuery += BotOnCallbackQueryReceived;

            Console.ReadLine();
            client.StopReceiving();

        }

        //Кнопка в сообщении
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
            Console.WriteLine("tut 3");
            if(callbackQuery.Data == "callback1")
                await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Callback 1");
            if (callbackQuery.Data == "callback2")
                await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Callback 2");

            await client.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}"
            );



            //await client.SendTextMessageAsync(
            //    chatId: callbackQuery.Message.Chat.Id,
            //    text: $"Received {callbackQuery.Data}"
            //);
        }

        private static async void Client_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            if(message.Text != null)
            {
                switch (message.Text.Split(' ').First())
                {
                    // Send inline keyboard
                    case "/inline":
                        await SendInlineKeyboard(message);
                        break;

                    // send custom keyboard
                    case "/keyboard":
                        await SendReplyKeyboard(message);
                        break;
                    case "Один":
                        await client.SendTextMessageAsync(message.Chat.Id, "Один!");
                        break;

                    default:
                        //await Usage(message);
                        break;
                }
            }

            // Send inline keyboard
            // You can process responses in BotOnCallbackQueryReceived handler
            static async Task SendInlineKeyboard(Message message)
            {
                await client.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                // Simulate longer running task
                await Task.Delay(500);

                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Кнопка 1", "callback1"),
                        InlineKeyboardButton.WithCallbackData("Кнопка 2", "callback2"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    }
                });
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: inlineKeyboard
                );
            }

            static async Task SendReplyKeyboard(Message message)
            {
                var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[][]
                    {
                        new KeyboardButton[] { "Один", "Два" },
                        new KeyboardButton[] { "2.1", "2.2" },
                    },
                    resizeKeyboard: true
                );

                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: replyKeyboardMarkup

                );
            }

            
        }
    }
}
