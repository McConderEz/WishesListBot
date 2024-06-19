using PRTelegramBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishesListBot.Services;

namespace WishesListBot.Presentation.Controllers
{
    [BotHandler]
    public class TelegramBotController
    {
        private readonly ITelegramBotService _service;

        public TelegramBotController(ITelegramBotService service)
        {
            _service = service;
        }

        public void Start()
        {
            _service.Start();
        }
    }
}
