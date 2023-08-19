namespace ASP111.Data.Entities
{
    public class Section
    {
        public Guid Id { get; set; }
        public String Title { get; set; } = null!;
        public String? Description { get; set; }
        public String? ImageUrl { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime CreateDt { get; set; }
        public DateTime? UpdateDt { get; set; }
        public DateTime? DeleteDt { get; set; }

        public User Author { get; set; }
        public IEnumerable<Rate> Rates { get; set; }

    }
}
