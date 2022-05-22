namespace Application.Utils
{
    public class DateUtil
    {
        public static DateTime GetCurrentDate()
        {
            return DateTime.UtcNow.AddHours(2);
        }
    }
}
