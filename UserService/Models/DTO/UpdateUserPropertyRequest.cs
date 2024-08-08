namespace UserService.Models.DTO
{
    public record UpdateUserPropertyRequest(string publicID, string password, string newPropertyValue);
}
