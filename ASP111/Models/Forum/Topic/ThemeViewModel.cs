﻿namespace ASP111.Models.Forum.Topic
{
    public class ThemeViewModel
    {
        public String Id { get; set; } = null!;
        public String Title { get; set; } = null!;
        public String CreatedDt { get; set; } = null!;
        public UserViewModel Author { get; set; } = null!;
        public String? FirstCommentText { get; set; } = null!;

        public ThemeViewModel()
        {
            
        }
        public ThemeViewModel(Data.Entities.Theme theme)
        {
            Id = theme.Id.ToString();
            Title = theme.Title;
            CreatedDt = theme.CreatedDt.ToShortDateString();
            FirstCommentText = theme.Comments?.OrderBy(c => c.CreatedDt).First().Content;
            Author = new(theme.Author);
        }
    }
}
