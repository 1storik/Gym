using Gym.Data;
using Gym.Models;
using Gym.Repository.IRepository;

namespace Gym.Repository
{
    public class MembershipRepository : Repository<Membership>, IMembershipRepository
    {
        public MembershipRepository(GymDbContext db) : base(db)
        { }
    }
}
