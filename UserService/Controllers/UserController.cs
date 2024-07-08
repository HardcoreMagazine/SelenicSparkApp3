using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        // v1: all at once;
        // v2: pagination
        [HttpGet]
        public IEnumerable<User> Get()
        {
            try
            {
                // .ToList() is used on DbSet<T> and/or IQueryable<T> to MATERIALIZE collection
                // IQueryable is just a promise
                return _appDbContext.Users.ToList();
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return new List<User>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return new List<User>();
            }
        }

        [HttpGet("{id:guid}")]
        public User GetID(Guid id)
        {
            try
            {
                var user = _appDbContext.Users.FirstOrDefault(u => u.GUID == id && u.Enabled) 
                    ?? throw new Exception($"User id=\"{id}\" not found");
                return user;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return new User();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return new User();
            }
        }

        [HttpPost(Name = "register")]
        public int CreateUser(User user)
        {
            if (!Models.User.Validate(user) || !user.Enabled)
                return (int)SharedLibCS.StatusCodes.ClientFail; // bad request body

            try
            {
                _appDbContext.Users.Add(user);
                _appDbContext.SaveChanges();
                return (int)SharedLibCS.StatusCodes.Ok;
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

        [HttpDelete("{id:guid}")]
        public int Delete(Guid id)
        {
            var user = _appDbContext.Users.FirstOrDefault(u => u.GUID == id && u.Enabled);

            if (user != null && Models.User.Validate(user))
            {
                user.Enabled = false;
                try
                {
                    _appDbContext.Users.Update(user);
                    _appDbContext.SaveChanges();
                    return (int)SharedLibCS.StatusCodes.Ok;
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
            else
            {
                return (int)SharedLibCS.StatusCodes.ClientFail;
            }
        }

        [HttpPut]
        public int Update(string oldPwd, User user)
        {
            if (string.IsNullOrWhiteSpace(oldPwd) || !Models.User.Validate(user) || !user.Enabled)
            {
                return (int)SharedLibCS.StatusCodes.ClientFail;
            }
            else
            {
                try
                {
                    // safe update: automatically block all attempts to change "Enabled", "Username" fields
                    var userOld = _appDbContext.Users.First(p => p.GUID == user.GUID);

                    // verify user identity (temporal solution, hopefully)
                    if (userOld.Password != oldPwd)
                    {
                        return (int)SharedLibCS.StatusCodes.BadCredentials;
                    }

                    if (userOld.Email != user.Email)
                    {
                        userOld.Email = user.Email;
                        userOld.EmailConfirmed = false;
                    }
                    userOld.Password = user.Password;

                    _appDbContext.Users.Update(userOld);
                    _appDbContext.SaveChanges();
                    return (int)SharedLibCS.StatusCodes.Ok;
                }
                catch (Npgsql.PostgresException ex)
                {
                    _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                    return (int)SharedLibCS.StatusCodes.ServerFail;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"{DateTimeOffset.Now} - ERROR: {ex.Message} | SRC: {ex.StackTrace}");
                    return (int)SharedLibCS.StatusCodes.ServerFail;
                }
            }
        }

        //placeholder: AddTFAuth, RemoveTFAuth - perhaps move it to AuthService instead?
    }
}
