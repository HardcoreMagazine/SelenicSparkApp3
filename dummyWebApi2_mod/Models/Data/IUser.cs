using dummyWebApi2.Models.DTO;

namespace dummyWebApi2.Models.Data
{
    public interface IUser
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);
        Task<SigninUserResponse> LoginUserAsync(SigninUserRequest request);
        Task<List<ApplicationUser>> GetUsers();
    }
}
