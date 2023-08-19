using ASP111.Data.Entities;

namespace ASP111.Models.Forum.Index
{
    public class ForumSectionViewModel
    {
        public String Id { get; set; }
        public String Title { get; set; } = null!;
        public String? Description { get; set; }
        public String? ImageUrl { get; set; }
        public String CreateDt { get; set; }
        public UserViewModel Author { get; set; } = null!;

        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public Rate? UserRate { get; set; }

    }
}
