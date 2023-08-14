namespace ASP111.Services.AuthUser
{
    public interface IAuthUserService
    {
        Guid? GetUserId(HttpContext context);
    }
}
