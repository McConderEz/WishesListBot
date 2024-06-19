using WishesListBot.Domain;

namespace WishesListBot.DAL.Repositories
{
    public interface IWishRepository
    {
        Task AddWishAsync(Wish wish);
        Task<List<Wish>> GetWishesAsync(string name);
        Task<List<Wish>> GetWishesByDateAsync(string name, DateTime dateTime);
    }
}