using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishesListBot.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Method,Inherited = true, AllowMultiple = false)]
    public class AuthorizeAttribute : Attribute
    {
    }
}
