using Microsoft.Extensions.DependencyInjection;
using PRTelegramBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WishesListBot.DAL.Repositories;
using WishesListBot.Services;

namespace WishesListBot.Presentation.Controllers
{
    [BotHandler]
    public class TelegramBotController
    {
        private readonly IUserRepository repository;
        private readonly IServiceProvider serviceProvider;

        public TelegramBotController(IUserRepository repository, IServiceProvider serviceProvider)
        {
            this.repository = repository;
            this.serviceProvider = serviceProvider;
        }


        [ReplyMenuHandler("Test")]
        public async Task TestMethodWithDependency(ITelegramBotClient botClient, Update update)
        {
            await PRTelegramBot.Helpers.Message.Send(botClient, update, $"{nameof(TestMethodWithDependency)}");
        }
    }
}
