using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Models.Data;
using UserService.Models.DTO;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository<User> _userManager;

        public UserController(ILogger<UserController> logger, IUserRepository<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterUserRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.username) || string.IsNullOrWhiteSpace(req.email) 
                || string.IsNullOrWhiteSpace(req.password))
            {
                return BadRequest();
            }

            try
            {
                var result = await _userManager.RegisterAsync(req.username, req.email, req.password);
                if (result == null)
                    return Ok();
                else
                    return BadRequest(result);
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);    
            }
            catch (Exception)// ex)
            {
                //_logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginUserRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.login) || string.IsNullOrWhiteSpace(req.password))
            {
                return BadRequest();
            }

            try
            {
                var token = await _userManager.LoginAsync(req.login, req.password);
                if (token != null)
                {
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Bad credentials");
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception)// ex)
            {
                //_logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var res = await _userManager.GetAllUsersAsync();
                return Ok(res.ToList());
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception)// ex)
            {
                //_logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetUser(string publicID)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public async Task<ActionResult> UpdateUsername(string publicID, string password, string username)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateEmail(string publicID, string password, string email)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePassword(string publicID, string currentPassword, string newPassword)
        {
            throw new NotImplementedException(); 
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(string publicID, string password)
        {
            throw new NotImplementedException(); 
        }
    }
}
