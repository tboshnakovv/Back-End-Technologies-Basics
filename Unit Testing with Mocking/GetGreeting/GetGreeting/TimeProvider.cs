namespace GetGreeting
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }

}
