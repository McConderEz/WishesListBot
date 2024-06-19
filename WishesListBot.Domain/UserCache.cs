using PRTelegramBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishesListBot.Domain
{
    public class UserCache : ITelegramCache
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public bool ClearData()
        {
            this.Name = string.Empty;
            this.Password = string.Empty;
            return true;
        }
    }
}
