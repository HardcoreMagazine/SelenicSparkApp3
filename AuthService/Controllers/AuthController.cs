//using AuthService.Models;
using AuthService.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        [HttpGet]
        public void Get()
        {
            return;
        }
    }
}
