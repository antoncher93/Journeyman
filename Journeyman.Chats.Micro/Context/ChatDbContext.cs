using Journeyman.Chats.Micro.Models;
using Microsoft.EntityFrameworkCore;

namespace Journeyman.Chats.Micro.Context
{
    public class ChatDbContext : DbContext
    {
        public DbSet<ChatAgreement> ChatAgreements { get; set; }
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
