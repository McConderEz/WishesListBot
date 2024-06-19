using Telegram.Bot;
using Telegram.Bot.Types;

namespace WishesListBot.Services
{
    public interface IUserService
    {
        Task AuthorizationLoginEnter(ITelegramBotClient botClient, Update update);
        Task RegistrationLoginEnter(ITelegramBotClient botClient, Update update);
    }
}