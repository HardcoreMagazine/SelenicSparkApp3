using dummyWebApi2.Models.Data;
using dummyWebApi2.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dummyWebApi2.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IUser AppUser;
        public AuthController(IUser user)
        {
            AppUser = user;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<SigninUserResponse>> SignIn(SigninUserRequest req)
        {
            var result = await AppUser.LoginUserAsync(req);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/api/[controller]/SignOut")]
        public async Task<ActionResult> SignOff()
        {
            return await Task.FromResult(Ok());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterUserResponse>> Register(RegisterUserRequest req)
        {
            var result = await AppUser.RegisterUserAsync(req);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<List<ApplicationUser>> GetUsers()
        {
            return await AppUser.GetUsers();
        }
    }
}
