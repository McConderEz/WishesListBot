using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WishesListBot.DAL;
using WishesListBot.DAL.Repositories;
using WishesListBot.Domain;
using WishesListBot.Services.Authorization;


namespace WishesListBot.Services
{
    [BotHandler]
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMemoryCache _cache;
        private Domain.User _currentUser;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher,IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _cache = memoryCache;
        }

        public Domain.User GetCurrentUser(string userId)
        {
            _cache.TryGetValue(userId, out Domain.User currentUser);
            return currentUser;
        }
        private void SetCurrentUser(string userId, Domain.User user)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));
            _cache.Set(userId, user,cacheEntryOptions);
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

            string msg;

            var user = await _userRepository.GetUserByNameAsync(handler.GetCache<UserCache>().Name);

            if (user != null)
            {
                var result = _passwordHasher.Verify(handler.GetCache<UserCache>().Password, user.Password);

                if (result)
                {
                    msg = "Вы успешно авторизованы!";
                    SetCurrentUser(update.Message.From.Id.ToString(), user);

                    await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
                }
                else
                {
                    msg = "Неверные данные пользователя!";
                    await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
                }
            }
            else
            {
                msg = "Пользователь не найден!";
                await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
            }

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
            try
            {
                var handler = update.GetStepHandler<StepTelegram>();
                handler!.GetCache<UserCache>().Password = update.Message.Text;
                update.ClearStepUserHandler();

                string msg;

                var cache = handler.GetCache<UserCache>();

                var user = await _userRepository.GetUserByNameAsync(handler.GetCache<UserCache>().Name);

                if (user == null)
                {
                    var hashedPassword = _passwordHasher.Generate(handler.GetCache<UserCache>().Password);

                    user = new Domain.User
                    {
                        Name = handler.GetCache<UserCache>().Name,
                        Password = hashedPassword
                    };
                    await _userRepository.AddUserAsync(user);

                    msg = "Вы успешно зарегистрировались!";

                    await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
                }
                else
                {
                    msg = "Пользователь с таким логином уже зарегистрирован!";
                    await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
                }

                handler.GetCache<UserCache>().ClearData();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

    }
}
