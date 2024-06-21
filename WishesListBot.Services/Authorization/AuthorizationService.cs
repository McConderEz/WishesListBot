using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishesListBot.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserService _userService;

        public AuthorizationService(UserService userService)
        {
            _userService = userService;
        }

        public bool IsUserAuthorized()
        {
            return _userService.GetCurrentUser() != null;
        }

    }
}
