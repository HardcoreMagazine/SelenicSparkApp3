namespace PostsService.Models.DTO
{
    public record PostResponse(int id, string title, string author, string text, DateTimeOffset dateCreated);
}
