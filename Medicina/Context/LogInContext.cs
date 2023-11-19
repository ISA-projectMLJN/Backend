using Medicina.Models;
using Microsoft.EntityFrameworkCore;

namespace Medicina.Context
{
    public class LogInContext : DbContext
    {
        public LogInContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<JwtService> Logins { get; set; }
    }
}

