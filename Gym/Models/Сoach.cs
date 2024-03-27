namespace Gym.Models
{
    public class Coach : Entity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public DateTime EmploymentDate { get; set; }
        public required string Specialization { get; set; }
        public required string Image { get; set; }

    }
}
