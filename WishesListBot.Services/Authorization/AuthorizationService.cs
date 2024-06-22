using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishesListBot.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService _userService;

        public AuthorizationService(IUserService userService)
        {
            _userService = userService;
        }

        public bool IsUserAuthorized(string userId)
        {
            return _userService.GetCurrentUser(userId) != null;
        }

    }
}
