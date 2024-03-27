namespace Gym.Models
{
    public class Client : Entity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Today;
        public required string Image { get; set; }
        public bool Presence { get; set; }
        public int? NumberOfShelf { get; set; }

        public List<Subscription>? Subscriptions { get; set; }
    }
}
