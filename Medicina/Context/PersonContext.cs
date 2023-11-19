using Medicina.Models;
using Microsoft.EntityFrameworkCore;

namespace Medicina.Context
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {

        }

        public DbSet<Person> Persons { get; set; }
    }
}
