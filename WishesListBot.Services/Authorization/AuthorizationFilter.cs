using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WishesListBot.Services.Attributes;

namespace WishesListBot.Services.Authorization
{
    public class AuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationFilter(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<bool> ExecuteWithAuthorizationCheck(object target, MethodInfo method, ITelegramBotClient botClient, Update update, object[] parameters)
        {
            var authorizeAttribute = method.GetCustomAttributes(typeof(AuthorizeAttribute), true).FirstOrDefault();
            if (authorizeAttribute != null)
            {
                if (!_authorizationService.IsUserAuthorized(update.Message.From.Id.ToString()))
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Вы не авторизованы для выполнения этой команды.");
                    return false;
                }
            }

            return true;
        }

    }
}
