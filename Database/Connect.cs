using dotnet_messenger.Database.Models;
using Microsoft.EntityFrameworkCore;


namespace dotnet_messenger.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<MessengerUser> User { get; set; }
        public DbSet<Message> Message { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var options = new
            {
                Server = "127.0.0.1:5432",
                Database = "mvc_messenger",
                User = "admin",
                Password = "admin",
            };

            optionsBuilder.UseNpgsql($"Server = {options.Server}; Database = {options.Database}; Uid = {options.User}; Pwd = {options.Password};");
        }
    }
}