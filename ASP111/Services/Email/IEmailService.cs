namespace ASP111.Services.Email
{
    public interface IEmailService
    {
        void Send(String Email, String Message, String Subject);
    }
}
