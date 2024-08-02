namespace PostsService.Models
{

    public interface IPost
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
    }
}
