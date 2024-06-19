using WishesListBot.Domain;

namespace WishesListBot.DAL.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User> GetUserByNameAsync(string name);
        Task<List<User>> GetUsersAsync();
    }
}