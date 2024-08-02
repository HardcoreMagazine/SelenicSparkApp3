using System.ComponentModel.DataAnnotations;

namespace PostsService.Models
{
    public class Post : IPost
    {
        private const int MinPostTitleLen = 15;

        [Key]
        public int ID { get; set; }
        [Required, StringLength(maximumLength: 256, MinimumLength = MinPostTitleLen)]
        public string Title { get; set; }
        [Required, StringLength(64)]
        public string Author { get; set; }
        [StringLength(24000)]
        public string Text { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
        public bool Enabled { get; set; } = true; // if "true" then visible, else hidden ("deleted")

        public static bool Validate(Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Title) 
                || string.IsNullOrWhiteSpace(post.Author) 
                || post.Title.Length < MinPostTitleLen)
                return false;
            else
                return true;
        }
    }
}
