using Gym.Models;

namespace Gym.VM
{
    public class ClientMembershipVM
    {
        public Client? Client { get; set; }
        public List<Membership>? ClientMemberships { get; set; }
        public List<Membership>? OtherMemberships { get; set; } 
    }
}
