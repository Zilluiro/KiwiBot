using KiwiBot.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace KiwiBot.Data
{
    partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .Property(c => c.ChatId)
                .ValueGeneratedNever();
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Booru> Boorus { get; set; }
    }
}
