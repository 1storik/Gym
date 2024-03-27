using Gym.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}
