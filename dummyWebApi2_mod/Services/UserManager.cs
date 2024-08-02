using dummyWebApi2.Models.Data;
using dummyWebApi2.Models.DTO;

namespace dummyWebApi2.Services
{
    public class UserManager : IUser
    {
        private static readonly List<ApplicationUser> ApplicationUsersRepo = new()
        {
            new ApplicationUser(1, "ADMIN", "admin", "1234"),
            new ApplicationUser(2, "TESTER", "tester", "1234"),
            new ApplicationUser(3, "NOOB", "noob", "1234"),
        };

        public async Task<ApplicationUser?> FindUserByD(int id) =>
            await Task.FromResult(ApplicationUsersRepo.FirstOrDefault(u => u.ID == id));

        public async Task<ApplicationUser?> FindUserByPublicID(string publicID) =>
            await Task.FromResult(ApplicationUsersRepo.FirstOrDefault(u => u.PublicID == Guid.Parse(publicID)));

        public async Task<ApplicationUser?> FindUserByName(string name) =>
            await Task.FromResult(ApplicationUsersRepo.FirstOrDefault(u => u.Name == name));

        public async Task<ApplicationUser?> FindUserByEmail(string email) =>
            await Task.FromResult(ApplicationUsersRepo.FirstOrDefault(u => u.Email == email));

        public async Task<SigninUserResponse> LoginUserAsync(SigninUserRequest request)
        {
            var user = await FindUserByEmail(request.Login);
            if (user != null && BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                string token = TokenManager.GenerateJwtToken(user);

                return new(true, "Sucesssfully signed in", token);
            }
            else
            {
                return new(false, "Sign-in failed: bad credentials");
            }
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
        {
            var existsByMail = await FindUserByEmail(request.Email);
            if (existsByMail != null)
            {
                return new(false, "Registration failed: selected email is being used");
            }

            var existsByName = await FindUserByName(request.Name);
            if (existsByName != null)
            {
                return new(false, "Registration failed: selected nickname is being used");
            }

            // 'nextMaxUserID' only used because we don't have DB context and data is being stored inside program memory
            var nextMaxUserID = ApplicationUsersRepo.Max(u => u.ID) + 1; 
            var normilizedEmail = request.Email.ToLower();

            // password will be hashed inside secondary constructor of "ApplicationUser" (on obj creation)
            ApplicationUsersRepo.Add(new(nextMaxUserID, request.Name, normilizedEmail, request.Password));

            return new(true, "Successfully registered");
        }

        public Task<List<ApplicationUser>> GetUsers()
        {
            return Task.FromResult(ApplicationUsersRepo);
        }
    }
}
