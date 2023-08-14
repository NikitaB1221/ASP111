using ASP111.Services.Validation;

namespace ASP111.Models.Forum.Theme
{
    public class CommentFormModel
    {
        [ValidationRules(ValidationRule.NotEmpty)]
        public String Content { get; set; } = null!;

        public Guid ThemeId { get; set; }
        public Guid? ReplyId { get; set; }
    }
}
