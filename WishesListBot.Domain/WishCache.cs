using PRTelegramBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishesListBot.Domain
{
    public class WishCache : ITelegramCache
    {
        public string Description { get; set; }
        public string RecipientName { get; set; }

        public bool ClearData()
        {
            this.Description = string.Empty;
            this.RecipientName = string.Empty;
            return true;
        }
    }
}
