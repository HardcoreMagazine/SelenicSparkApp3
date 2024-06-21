using System.ComponentModel.DataAnnotations;

namespace PostsService.Models
{
    public class Post
    {
        public int ID { get; set; }
        [StringLength(256)]
        public required string Title { get; set; }
        [StringLength(64)]
        public required string Author { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
