﻿using Microsoft.EntityFrameworkCore;
using WishesListBot.Domain;

namespace WishesListBot.DAL
{
    public class BotDbContext: DbContext
    {

        public BotDbContext(DbContextOptions<BotDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=wishesList;Username=postgres;Password=345890");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BotDbContext).Assembly);
        }


        public DbSet<Wish> Wishes { get; set; }
        public DbSet<User> Users { get; set; }  
    }
}
