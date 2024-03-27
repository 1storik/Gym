using Gym.Data;
using Gym.Models;
using Gym.Repository.IRepository;

namespace Gym.Repository
{
    public class CoachRepository : Repository<Coach>, ICoachRepository
    {
        public CoachRepository(GymDbContext db) : base(db)
        { }
    }
}
