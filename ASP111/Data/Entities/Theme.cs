namespace ASP111.Data.Entities
{
    public class Theme
    {
        public Guid Id { get; set; }
        public String Title { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public Guid TopicId { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime? UpdateDt { get; set; }
        public DateTime? DeleteDt { get; set; }

        // Navigation properties

        public User Author { get; set; } = null!;
        public List<Comment> Comments { get; set; } = null!;

    }
}
