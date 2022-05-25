using Journeyman.Persons.Micro.Models;
using Microsoft.EntityFrameworkCore;

namespace Journeyman.Persons.Micro.Context
{
    public class PersonsDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public PersonsDbContext(DbContextOptions<PersonsDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
