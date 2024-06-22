namespace WishesListBot.Services.Authorization
{
    public interface IAuthorizationService
    {
        bool IsUserAuthorized(string userId);
    }
}