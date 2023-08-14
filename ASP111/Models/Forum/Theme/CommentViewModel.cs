namespace ASP111.Models.Forum.Theme
{
    public class CommentViewModel
    {
        public String Id { get; set; } = null!;
        public String Content { get; set; } = null!;
        public String CreatedDt{ get; set; } = null!;
        public UserViewModel Author { get; set; } = null!;

        public CommentViewModel()
        {
            
        }

        public CommentViewModel( Data.Entities.Comment comment)
        {
            Id = comment.Id.ToString();
            Content = comment.Content;
            CreatedDt = comment.CreatedDt.ToShortDateString();
            Author = new(comment.Author);
        }
    }
}
