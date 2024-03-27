using Gym.Data;
using Gym.Models;
using Gym.Repository.IRepository;

namespace Gym.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(GymDbContext db) : base(db)
        { }
    }
}
