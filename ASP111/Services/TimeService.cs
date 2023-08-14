namespace ASP111.Services
{
    public class TimeService
    {
        public TimeOnly GetTime() => TimeOnly.FromDateTime(DateTime.Now);
    }
}
