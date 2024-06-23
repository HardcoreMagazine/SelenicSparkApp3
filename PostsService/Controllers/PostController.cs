using Microsoft.AspNetCore.Mvc;
using PostsService.Data;
using PostsService.Models;

namespace PostsService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<PostController> _logger;

        public PostController(ILogger<PostController> logger, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        // v1: all at once;
        // v2: pagination
        [HttpGet]
        public IEnumerable<Post> Get()
        {
            try
            {
                return _appDbContext.Posts.ToList();
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return new List<Post>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - ERROR: {ex.Message} | SRC: {ex.Source}");
                return new List<Post>();
            }
        }

        [HttpGet("{id:int}")]
        public Post Get(int id)
        {
            try
            {
                var post = _appDbContext.Posts.FirstOrDefault(p => p.ID == id) ?? throw new Exception($"Post Not Found: {id}");
                return post;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return new Post();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - ERROR: {ex.Message} | SRC: {ex.Source}");
                return new Post();
            }
        }

        [HttpPost]
        public int CreatePost(Post post)
        {
            if (!Post.Validate(post))
                return (int)SharedLib.ErrorTypes.ClientFail; // bad request body

            try
            {
                _appDbContext.Posts.Add(post);
                _appDbContext.SaveChanges();
                return post.ID; // id is returned on insertion
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return (int)SharedLib.ErrorTypes.ServerFail;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - ERROR: {ex.Message} | SRC: {ex.Source}");
                return (int)SharedLib.ErrorTypes.ServerFail;
            }
        }
    }
}
