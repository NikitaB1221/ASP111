namespace ASP111.Data.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public String Content { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public Guid ThemeId { get; set; }
        public Guid? ReplyId { get; set; }
        public String? ImageUrl { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime? UpdateDt { get; set; }
        public DateTime? DeleteDt { get; set; }

        // Navigation properties

        public User Author { get; set; } = null!;
        public Theme Theme { get; set; } = null!;
    }
}
