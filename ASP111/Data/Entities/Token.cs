namespace ASP111.Data.Entities
{
    public class Token
    {
        public String Id { get; set; }
        public Guid User{ get; set; }

        public DateTime Expires { get; set; }
    }
}
