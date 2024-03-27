using Gym.Models;

namespace Gym.VM
{
    public class HomeVM
    {
        public List<Membership> Memberships { get; set; } = new List<Membership>();
        public List<Coach> Coachs { get; set; } = new List<Coach>();
    }
}
