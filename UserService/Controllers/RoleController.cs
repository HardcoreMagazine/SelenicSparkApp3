using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<RoleController> _logger;

        public RoleController(ILogger<RoleController> logger, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Role> Get()
        {
            try
            {
                return _appDbContext.Roles.Where(r => r.Enabled).ToList();
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return new List<Role>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return new List<Role>();
            }
        }

        [HttpPost]
        public int CreateRole(Role role)
        {
            if (!Role.Validate(role) || !role.Enabled)
                return (int)SharedLib.StatusCodes.ClientFail; // bad request body

            try
            {
                _appDbContext.Roles.Add(role);
                _appDbContext.SaveChanges();
                return (int)SharedLib.StatusCodes.Ok;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return (int)SharedLib.StatusCodes.ServerFail;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return (int)SharedLib.StatusCodes.ServerFail;
            }
        }

        [HttpDelete]
        public int Delete(int id)
        {
            var role = _appDbContext.Roles.FirstOrDefault(r => r.ID == id && r.Enabled);

            if (role != null && Role.Validate(role))
            {
                role.Enabled = false;
                try
                {
                    _appDbContext.Roles.Update(role);
                    _appDbContext.SaveChanges();
                    return (int)SharedLib.StatusCodes.Ok;
                }
                catch (Npgsql.PostgresException ex)
                {
                    _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                    return (int)SharedLib.StatusCodes.ServerFail;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                    return (int)SharedLib.StatusCodes.ServerFail;
                }
            }
            else
            {
                return (int)SharedLib.StatusCodes.ClientFail;
            }
        }
    }
}
