using Telegram.Bot;
using Telegram.Bot.Types;

namespace WishesListBot.Services
{
    public interface ICommandService
    {
        Task Welcome(ITelegramBotClient botClient, Update update);
    }
}