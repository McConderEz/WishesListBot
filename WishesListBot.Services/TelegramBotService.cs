using PRTelegramBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishesListBot.Services
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly PRBot _telegram;

        public TelegramBotService(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token is null");

            _telegram = new PRBot(option =>
            {
                option.Token = token;
                option.ClearUpdatesOnStart = true;
                option.WhiteListUsers = new List<long>();
                option.Admins = new List<long>();
                option.BotId = 0;
            });

            _telegram.OnLogCommon += Telegram_OnLogCommon;
            _telegram.OnLogError += Telegram_OnLogError;
        }

        private void Telegram_OnLogCommon(string msg, Enum typeEvent, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string message = $"{DateTime.Now}:{msg}";
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void Telegram_OnLogError(Exception ex, long? id)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string errorMsg = $"{DateTime.Now}:{ex}";
            Console.WriteLine(errorMsg);
            Console.ResetColor();
        }

        public async void Start()
        {
            await _telegram.Start();
        }


    }


}
