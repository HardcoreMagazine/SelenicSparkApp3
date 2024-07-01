using System.ComponentModel.DataAnnotations;

namespace PostsService.Models
{
    public class Post
    {
        private const int MinPostTitleLen = 15;

        public int ID { get; set; }
        [StringLength(maximumLength: 256, MinimumLength = MinPostTitleLen)]
        public string Title { get; set; } // not nullable
        [StringLength(64)]
        public string Author { get; set; } // not nullable
        [StringLength(24000)]
        public string? Text {  get; set; } // nullable
        public DateTimeOffset DateCreated { get; set; }
        public bool Enabled { get; set; } = true; // if "true" then visible, else hidden ("deleted")

        public static bool Validate(Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Title) || string.IsNullOrWhiteSpace(post.Author) 
                || post.Title.Length < MinPostTitleLen)
                return false;
            else
                return true;
        }
    }
}
