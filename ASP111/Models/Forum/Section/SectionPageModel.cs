using ASP111.Models.Forum.Index;
namespace ASP111.Models.Forum.Section
{
    public class SectionPageModel
    {
        public Dictionary<String, String?>? ErrorMessages { get; set; } 
        public List<TopicViewModel> Topics { get; set; } = null!;
        public ForumSectionViewModel Section { get; set; } = null!;
    }
}
