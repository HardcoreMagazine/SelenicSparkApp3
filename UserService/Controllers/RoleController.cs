using Generics.Models;
using Microsoft.AspNetCore.Mvc;
using UserService.Models.Data;
using UserService.Models.SharedDictionary;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRepository<Role> _roleManager;
        private readonly IUserRoleRepository<UserRole> _userRolesManager;

        public RoleController(ILogger<RoleController> logger, IRepository<Role> roleMgr, IUserRoleRepository<UserRole> userRoleMgr)
        {
            _logger = logger;
            _roleManager = roleMgr;
            _userRolesManager = userRoleMgr;
        }

        #region Roles

        [HttpPost]
        public async Task<ActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest();

            try
            {
                var role = new Role { Name = roleName };
                var result = await _roleManager.CreateAsync(role);
                if (result == (int)EntityCreateResponses.Success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Exists");
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception)// ex)
            {
                //_logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleManager.GetAllAsync();
                return Ok(roles.ToList());
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return Ok(new List<Role>());
            }
            catch (Exception)// ex)
            {
                //_logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex}");
                return Ok(new List<Role>());
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            try
            {
                var result = await _roleManager.DeleteAsync(id);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception)// ex)
            {
                //_logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion

        #region UserRoles

        [HttpPost]
        public async Task<ActionResult> GrantRoleToUser([FromBody] UserRole userRole)
        {
            try
            {
                // user-role validation performed on data Manager side
                var result = await _userRolesManager.GrantRoleToUserAsync(userRole);
                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception)// ex)
            {
                //_logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetUserRoles(string publicID)
        {
            try
            {
                var userRoles = await _userRolesManager.GetAllUserRolesAsync(publicID);
                if (userRoles.Count > 0)
                    return Ok(userRoles.ToList());
                else
                    return NotFound();
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception)// ex)
            {
                //_logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> RevokeRoleFromUser([FromBody] UserRole userRole)
        {
            var result = await _userRolesManager
                .RevokeRoleFromUserAsync(userRole.UserID.ToString(), userRole.RoleID);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        #endregion
    }
}
