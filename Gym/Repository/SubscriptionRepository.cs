using Gym.Data;
using Gym.Models;
using Gym.Repository.IRepository;

namespace Gym.Repository
{
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(GymDbContext db) : base(db)
        { }
    }
}
