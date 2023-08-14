namespace ASP111.Services
{
    public class DateService : IDateService
    {
        public DateTime GetDate() => DateTime.Today;
    }
}
