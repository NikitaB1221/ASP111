namespace ASP111.Models
{
    public class UserViewModel
    {
        public String Id { get; set; }
        public String? Name { get; set; } = null!;
        public String Email { get; set; } = null!;
        public String Login { get; set; } = null!;
        public String? Avatar { get; set; } = null!;
        public String CreatedDt { get; set; } = null!;
        public String LastUpdatedDt { get; set; } = null!;

        public UserViewModel()
        {
            
        }

        public UserViewModel(Data.Entities.User user)
        {
            Id = user.Id.ToString();
            Name = user.Name;
            Email = user.Email;
            Login = user.Login;
            Avatar = "/avatars/" + (user.Avatar ?? "no-photo.png");
            CreatedDt = user.CreatedDt?.ToShortDateString() ?? "";
            LastUpdatedDt = user.CreatedDt?.ToShortDateString() ?? "";

        }

    }
}
