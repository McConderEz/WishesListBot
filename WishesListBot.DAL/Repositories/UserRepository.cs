using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishesListBot.Domain;

namespace WishesListBot.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BotDbContext _dbContext;

        public UserRepository(BotDbContext botDbContext)
        {
            _dbContext = botDbContext;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Wishes)
                .ToListAsync();
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await _dbContext.Users
                .Include(u => u.Wishes)
                .FirstOrDefaultAsync(u => u.Name.Equals(name));
        }

        public async void AddUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
