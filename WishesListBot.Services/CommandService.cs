using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Helpers;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WishesListBot.DAL.Repositories;
using WishesListBot.Domain;
using WishesListBot.Services.Attributes;
using WishesListBot.Services.Authorization;

namespace WishesListBot.Services
{
    [BotHandler]
    public class CommandService : ICommandService
    {
        private readonly IWishRepository _wishRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IAesEncryption _cryptService;

        public CommandService(IWishRepository wishRepository, IUserRepository userRepository, IUserService userService, IAesEncryption cryptService)
        {
            _wishRepository = wishRepository;
            _userRepository = userRepository;
            _userService = userService;
            _cryptService = cryptService;
        }


        //TODO: Атрибут некорректно работает

        //TODO: Добавить аватарку
        //TODO: Добавить хэширование желаний
        //TODO: Добавить Список адресатов 

        [ReplyMenuHandler("/start")]
        public async Task Welcome(ITelegramBotClient botClient, Update update)
        {
            var message = "Привет, я бот, который хранит список желаний!";

            var menuList = new List<KeyboardButton>();

            menuList.Add(new KeyboardButton("Авторизация"));
            menuList.Add(new KeyboardButton("Регистрация"));
            menuList.Add(new KeyboardButton("Добавить пожелание"));
            menuList.Add(new KeyboardButton("Посмотреть случайное пожелание"));

            var menu = MenuGenerator.ReplyKeyboard(1, menuList);
            var option = new OptionMessage();
            option.MenuReplyKeyboardMarkup = menu;

            var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, message, option);
        }


        [ReplyMenuHandler("Добавить пожелание")]
        public async Task AddWishLogin(ITelegramBotClient botClient, Update update)
        {
            if(_userService.GetCurrentUser(update.Message.From.Id.ToString()) != null)
            {
                string msg = "Введите логин адресата:";
                update.RegisterStepHandler(new StepTelegram(AddWishDescription, new WishCache()));
                await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
            }
            else
            {
                string msg = "Вы не авторизованы!";
                await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
            }
        }

        public async Task AddWishDescription(ITelegramBotClient botClient, Update update)
        {
            string msg = "Введите описание желания:";
            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<WishCache>().RecipientName = update.Message.Text;
            handler.RegisterNextStep(AddWishStop);
            await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
        }

        public async Task AddWishStop(ITelegramBotClient botClient, Update update)
        {
            var handler = update.GetStepHandler<StepTelegram>();
            handler!.GetCache<WishCache>().Description = update.Message.Text;
            update.ClearStepUserHandler();

            string msg;

            var user = await _userRepository.GetUserByNameAsync(handler.GetCache<WishCache>().RecipientName);

            if (user != null)
            {
                var wish = new Wish
                {
                    DateTime = DateTime.UtcNow,
                    Description = _cryptService.Encrypt(handler.GetCache<WishCache>().Description),
                    RecipientName = handler.GetCache<WishCache>().RecipientName,
                    UserId = _userService.GetCurrentUser(update.Message.From.Id.ToString()).Id
                };

                await _wishRepository.AddWishAsync(wish);
            }


            handler.GetCache<WishCache>().ClearData();
        }



        [ReplyMenuHandler("Посмотреть случайное пожелание")]
        public async Task GetRandomWish(ITelegramBotClient botClient, Update update)
        {
            if (_userService.GetCurrentUser(update.Message.From.Id.ToString()) != null)
            {
                var wishes = await _wishRepository.GetWishesAsync(_userService.GetCurrentUser(update.Message.From.Id.ToString()).Name);

                if(wishes.Count() == 0)
                {
                    string msg = "Ваша корзина желаний пуста!";
                    await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
                    return;
                }

                var randomWish = wishes[new Random().Next(0, wishes.Count)];

                await PRTelegramBot.Helpers.Message.Send(botClient, update, _cryptService.Decrypt(randomWish.Description));
                await _wishRepository.DeleteWishAsync(randomWish.Id);
            }
            else
            {
                string msg = "Вы не авторизованы!";
                await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
            }
        }
    }
}
