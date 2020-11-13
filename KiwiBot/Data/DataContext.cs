using KiwiBot.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace KiwiBot.Data
{
    partial class DataContext : DbContext, IDisposable
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            Console.WriteLine("In dataContext ctr");
        }

        public override void Dispose() {
            base.Dispose();
            Console.WriteLine("dataContext disposed");
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
