namespace PostsService.Models.DTO
{
    public record PostResponse(int id, string title, string author, string text, DateTimeOffset dateCreated) : IPost
    {
        public int ID { get; set; } = id;
        public string Title { get; set; } = title;
        public string Author { get; set; } = author;
        public string Text { get; set; } = text;
        public DateTimeOffset DateCreated { get; set; } = dateCreated;
    }
}
