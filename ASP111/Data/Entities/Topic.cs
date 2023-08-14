namespace ASP111.Data.Entities
{
    public class Topic
    {
        public Guid Id { get; set; }
        public String Title { get; set; } = null!;
        public String? Description { get; set; }
        public String? ImageUrl { get; set; }
        public Guid AuthorId { get; set; }
        public Guid SectionId { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime? UpdateDt { get; set; }
        public DateTime? DeleteDt { get; set; }

        public User Author { get; set; }
    }
}
