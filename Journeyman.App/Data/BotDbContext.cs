using Microsoft.EntityFrameworkCore;
using Journeyman.Services.Entities;

namespace Journeyman.App.Data
{
    public class BotDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<ChatAgreement> ChatAgreements { get; set; }
        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
