namespace Gym.Utility
{
    public static class DateHelper
    {
        public static string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("yyyy-MM-dd") : string.Empty;
        }
    }
}
