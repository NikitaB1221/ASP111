namespace ASP111.Services.TopicDateService
{
    public class TopicDateTimeFormatterV1 : ITopicDateTimeFormatter
    {
        public string FormatDateTime(DateTime dateTime)
        {
            DateTime now = DateTime.Now.Date;
            TimeSpan timeDifference = now - dateTime.Date;

            if (timeDifference.TotalDays < 1)
            {
                return dateTime.ToString("HH:mm:ss");
            }
            else if (timeDifference.TotalDays < 2)
            {
                return "Вчера " + dateTime.ToString("HH:mm");
            }
            else if (timeDifference.TotalDays < 10)
            {
                int daysAgo = (int)timeDifference.TotalDays;
                return $"{daysAgo} дней назад";
            }
            else
            {
                return dateTime.ToString("dd.MM.yyyy");
            }
        }
    }
}
