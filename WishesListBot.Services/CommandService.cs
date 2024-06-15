using PRTelegramBot.Attributes;
using System.Runtime.CompilerServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using WishesListBot.DAL.Repositories;

namespace WishesListBot.Services
{
    public class CommandService : ICommandService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWishRepository _wishRepository;

        public CommandService(IUserRepository userRepository, IWishRepository wishRepository)
        {
            _userRepository = userRepository;
            _wishRepository = wishRepository;
        }

        [ReplyMenuHandler("/start")]
        public async Task Welcome(ITelegramBotClient botClient, Update update)
        {
            var message = "Привет, я бот, который хранит список желаний!";
            var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }
    }
}
