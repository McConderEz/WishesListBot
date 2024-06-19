using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishesListBot.Domain;

namespace WishesListBot.DAL.Repositories
{
    public class WishRepository : IWishRepository
    {
        private readonly BotDbContext _dbContext;

        public WishRepository(BotDbContext botDbContext)
        {
            _dbContext = botDbContext;
        }

        public async Task<List<Wish>> GetWishesAsync(string name)
        {
            return await _dbContext.Wishes
                .AsNoTracking()
                .Include(w => w.User)
                .Where(w => w.RecipientName.Equals(name))
                .ToListAsync();
        }

        public async Task<List<Wish>> GetWishesByDateAsync(string name, DateTime dateTime)
        {
            return await _dbContext.Wishes
                .AsNoTracking()
                .Include(w => w.User)
                .Where(w => w.RecipientName.Equals(name) && w.DateTime.Equals(dateTime))
                .ToListAsync();
        }

        public async Task AddWishAsync(Wish wish)
        {
            await _dbContext.Wishes.AddAsync(wish);
            await _dbContext.SaveChangesAsync();
        }

    }
}
