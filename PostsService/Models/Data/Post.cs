using Generics.Models;
using System.ComponentModel.DataAnnotations;

namespace PostsService.Models.Data
{
    public class Post : IEntity
    {
        private const int MinPostTitleLen = 15;
        private const int MaxPostTitleLen = 256;

        [Key]
        public int ID { get; set; }
        [Required, StringLength(maximumLength: MaxPostTitleLen, MinimumLength = MinPostTitleLen)]
        public string Title { get; set; }
        [Required, StringLength(64)]
        public string Author { get; set; }
        [StringLength(24000)]
        public string Text { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
        public bool Enabled { get; set; } = true; // if "true" then visible, else hidden ("deleted")

        /// <summary>
        /// Checks if selected Post object corresponds to following rules: 
        /// Post.Title and Post.Author is not: null, empty, whitespace; 
        /// Post.Title.Length value between <paramref name="MinPostTitleLen"/> and 
        /// <paramref name="MaxPostTitleLen"/> (defaults to 15, 256)
        /// </summary>
        /// <param name="post">Post object</param>
        /// <returns>True if Post object corresponds to said rules</returns>
        public static bool Validate(Post post)
        {
            return !string.IsNullOrWhiteSpace(post.Title)
                && !string.IsNullOrWhiteSpace(post.Author)
                && post.Title.Length >= MinPostTitleLen
                && post.Title.Length <= MaxPostTitleLen;
        }
    }
}
