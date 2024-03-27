namespace Gym.Models
{
    public class Membership : Entity
    {
        public required string MembershipType { get; set; }
        public required string Text { get; set; }
        public required string ShortText { get; set; }
        public decimal Price { get; set; }
        public required string Image { get; set; }

        public List<Subscription>? Subscription { get; set; }
        public Coach? Coach { get; set; }
    }
}
