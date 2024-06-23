using System.ComponentModel.DataAnnotations;

namespace PostsService.Models
{
    public class Post
    {
        public int ID { get; set; }
        [StringLength(256)]
        public string Title { get; set; } // not nullable
        [StringLength(64)]
        public string Author { get; set; } // not nullable
        [StringLength(24000)]
        public string? Text {  get; set; } // nullable
        public DateTimeOffset DateCreated { get; set; }

        public static bool Validate(Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Title) || string.IsNullOrWhiteSpace(post.Author))
                return false;
            else
                return true;
        }
    }
}
