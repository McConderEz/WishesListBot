using PRTelegramBot.Attributes;
using PRTelegramBot.Models;
using PRTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WishesListBot.DAL.Repositories;

namespace WishesListBot.Services
{
    [BotHandler]
    public class CommandService : ICommandService
    {
        private readonly IWishRepository _wishRepository;

        public CommandService(IWishRepository wishRepository)
        {
            _wishRepository = wishRepository;
        }


        //TODO: Сделать аккаунт сервис с регистрацией и авторизацией, а также проверкой существующего аккаунта
        //TODO: Сделать функции добавления и просмотра рандомных желаний

        [ReplyMenuHandler("/start")]
        public async Task Welcome(ITelegramBotClient botClient, Update update)
        {
            var message = "Привет, я бот, который хранит список желаний!";
            var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }

        [ReplyMenuHandler("/menu")]
        public async Task Menu(ITelegramBotClient botClient, Update update)
        {

            var menuList = new List<KeyboardButton>();

            menuList.Add(new KeyboardButton("Авторизация"));
            menuList.Add(new KeyboardButton("Регистрация"));
            menuList.Add(new KeyboardButton("Добавить пожелание"));
            menuList.Add(new KeyboardButton("Посмотреть случайное пожелание"));

            var menu = MenuGenerator.ReplyKeyboard(1, menuList);
            var option = new OptionMessage();
            option.MenuReplyKeyboardMarkup = menu;

            var message = "Список функций";

            var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update,message, option);
        }

        
        

    }
}
