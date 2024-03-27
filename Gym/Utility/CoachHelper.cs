using Gym.Models;

namespace Gym.Utility
{
    public static class CoachHelper
    {
        public static string GetFullName(Coach coach)
        {
            return $"{coach.FirstName} {coach.LastName}";
        }
    }
}
