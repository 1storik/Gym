namespace Gym.Models
{
    public class Subscription : Entity
    {
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }

        public Guid MembershipId { get; set; }
        public Membership? Membership { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Freeze { get; set; }
        public DateTime FreezeDate { get; set; }
    }
}
