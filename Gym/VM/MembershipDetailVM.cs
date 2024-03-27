using Gym.Models;

namespace Gym.VM
{
    public class MembershipDetailVM
    {
        public Membership Membership { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}
