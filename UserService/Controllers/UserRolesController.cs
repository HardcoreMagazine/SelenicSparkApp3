using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Models.Data;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRolesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<UserRolesController> _logger;

        public UserRolesController(ILogger<UserRolesController> logger, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        public IEnumerable<UserRole> GetID(Guid id)
        {
            try
            {
                var userRoles = _appDbContext.UserRoles.Where(ur => ur.UsertID == id).ToList() 
                    ?? throw new Exception($"UserRole UserID=\"{id}\" not found");
                return userRoles;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return new List<UserRole>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return new List<UserRole>();
            }
        }

        [HttpPost]
        public int InscribeRole(Guid userid, int roleid)
        {
            // minimuzing error possibility
            if (string.IsNullOrWhiteSpace(userid.ToString()) || roleid <= 0)
                return (int)SharedLibCS.StatusCodes.ClientFail; // bad request body

            try
            {
                var user = _appDbContext.Users.FirstOrDefault(u => u.PublicID == userid);
                if (user == null)
                    return (int)SharedLibCS.StatusCodes.ClientFail;

                var role = _appDbContext.Roles.FirstOrDefault(r => r.ID == roleid);

                if (role == null)
                    return (int)SharedLibCS.StatusCodes.ClientFail;

                var userRoles = _appDbContext.UserRoles.Where(ur => ur.UsertID == userid);

                if (!userRoles.Select(ur => ur.RoleID).Contains(roleid))
                {
                    var newUserRole = new UserRole() { UsertID = userid, RoleID = roleid };
                    _appDbContext.UserRoles.Add(newUserRole);
                    _appDbContext.SaveChanges();
                    return (int)SharedLibCS.StatusCodes.Ok;
                }
                else
                {
                    return (int)SharedLibCS.StatusCodes.ClientFail;
                }    
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return (int)SharedLibCS.StatusCodes.ServerFail;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return (int)SharedLibCS.StatusCodes.ServerFail;
            }
        }

        [HttpDelete]
        public int EraseRole(Guid userid, int roleid)
        {
            if (string.IsNullOrWhiteSpace(userid.ToString()) || roleid <= 0)
                return (int)SharedLibCS.StatusCodes.ClientFail; // bad request body

            try
            {
                var userRole = _appDbContext.UserRoles.FirstOrDefault(ur => ur.UsertID == userid && ur.RoleID == roleid);

                if (userRole != null)
                {
                    _appDbContext.UserRoles.Remove(userRole);
                    _appDbContext.SaveChanges();
                    return (int)SharedLibCS.StatusCodes.Ok;
                }
                else
                {
                    return (int)SharedLibCS.StatusCodes.ClientFail;
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return (int)SharedLibCS.StatusCodes.ServerFail;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return (int)SharedLibCS.StatusCodes.ServerFail;
            }
        }
    }
}
