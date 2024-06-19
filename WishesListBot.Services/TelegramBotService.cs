using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PRTelegramBot.Attributes;
using PRTelegramBot.Configs;
using PRTelegramBot.Core;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Models.EventsArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WishesListBot.Domain;

namespace WishesListBot.Services
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly PRBot _telegram;

        public TelegramBotService(IOptions<TelegramBotOptions> options)
        {

            var token = options.Value.Token;

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token is null");



            _telegram = new PRBot(config =>
            {
                config.Token = token;
                config.ClearUpdatesOnStart = true;
                config.BotId = 0;
            });

            _telegram.Events.OnCommonLog += Telegram_OnLogCommon;
            _telegram.Events.OnErrorLog += Telegram_OnLogError;
        }

        private async Task Telegram_OnLogError(ErrorLogEventArgs args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string errorMsg = $"{DateTime.Now}:{args.Exception.Message}";
            Console.WriteLine(errorMsg);
            Console.ResetColor();
        }


        private async Task Telegram_OnLogCommon(CommonLogEventArgs args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string message = $"{DateTime.Now}:{args.Message}";
            Console.WriteLine(message);
            Console.ResetColor();
        }


        public async void Start()
        {
            try
            {
                await _telegram.Start();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }


    }


}
