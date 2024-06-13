using Microsoft.EntityFrameworkCore;
using WishesListBot.Domain;

namespace WishesListBot.DAL
{
    public class BotDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source=(localdb)\\MSSQLLocalDB;Initial Catalog=wishesList;Integrated Security=True;");
        }


        public DbSet<Wish> Wishes { get; set; }
        public DbSet<User> Users { get; set; }  
    }
}
