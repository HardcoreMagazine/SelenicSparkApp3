
namespace PostsService.Models.DTO
{
    public record PostRequest(string title, string author, string text) : IPost
    {
        public string Title { get; set; } = title;
        public string Author { get; set; } = author;
        public string Text { get; set; } = text;
    }
}
