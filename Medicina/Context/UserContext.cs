using Medicina.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Medicina.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }


    }
}
