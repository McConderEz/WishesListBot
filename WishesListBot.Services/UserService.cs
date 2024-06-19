using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WishesListBot.DAL;
using WishesListBot.DAL.Repositories;
using WishesListBot.Domain;


namespace WishesListBot.Services
{
    [BotHandler]
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [ReplyMenuHandler("Авторизация")]
        public async Task AuthorizationLoginEnter(ITelegramBotClient botClient, Update update)
        {
            string msg = "Введите логин:";
            update.RegisterStepHandler(new StepTelegram(AuthorizationPasswordEnter, new UserCache()));
            await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
        }

        private async Task AuthorizationPasswordEnter(ITelegramBotClient botClient, Update update)
        {
            string msg = "Введите пароль:";
            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<UserCache>().Name = update.Message.Text;
            handler.RegisterNextStep(AuthorizationStop);
            await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
        }

        private async Task AuthorizationStop(ITelegramBotClient botClient, Update update)
        {
            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<UserCache>().Password = update.Message.Text;
            update.ClearStepUserHandler();


            handler.GetCache<UserCache>().ClearData();
        }

        [ReplyMenuHandler("Регистрация")]
        public async Task RegistrationLoginEnter(ITelegramBotClient botClient, Update update)
        {
            string msg = "Введите логин:";
            update.RegisterStepHandler(new StepTelegram(RegistrationPasswordEnter, new UserCache()));
            await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
        }

        private async Task RegistrationPasswordEnter(ITelegramBotClient botClient, Update update)
        {
            string msg = "Введите пароль:";
            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<UserCache>().Name = update.Message.Text;
            handler.RegisterNextStep(RegistrationStop);
            await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
        }

        private async Task RegistrationStop(ITelegramBotClient botClient, Update update)
        {
            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<UserCache>().Password = update.Message.Text;
            update.ClearStepUserHandler();

            var cache = handler.GetCache<UserCache>();

            await _userRepository.AddUserAsync(new Domain.User
            {
                Name = cache.Name,
                Password = cache.Password
            });
            handler.GetCache<UserCache>().ClearData();
        }
    }
}
