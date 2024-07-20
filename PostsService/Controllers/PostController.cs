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
                return _appDbContext.Posts
                    .Where(p => p.Enabled)
                    .OrderBy(p => p.ID)
                    .ToList();
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return new List<Post>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return new List<Post>();
            }
        }

        [HttpGet("{id:int}")]
        public Post GetID(int id)
        {
            try
            {
                var post = _appDbContext.Posts.FirstOrDefault(p => p.ID == id && p.Enabled) 
                    ?? throw new Exception($"Post id=\"{id}\" not found");
                return post;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return new Post();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex.Message} | SRC: {ex.StackTrace}");
                return new Post();
            }
        }

        [HttpPost]
        public int CreatePost(Post post)
        {
            if (!Post.Validate(post) || !post.Enabled)
                return (int)SharedLibCS.StatusCodes.ClientFail; // bad request body

            try
            {
                _appDbContext.Posts.Add(post);
                _appDbContext.SaveChanges();
                return post.ID; // id is returned after insertion
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
        public int Delete(int id)
        {
            var post = _appDbContext.Posts.FirstOrDefault(p => p.ID == id && p.Enabled);

            if (post != null && Post.Validate(post))
            {
                post.Enabled = false;
                try
                {
                    _appDbContext.Posts.Update(post);
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
        public int Update(Post post)
        {
            if (!Post.Validate(post) || !post.Enabled || post.ID <= 0)
            {
                return (int)SharedLibCS.StatusCodes.ClientFail;
            }
            else
            {
                try
                {
                    // safe update: automatically block all attempts to change all fields except "Text"
                    var postOld = _appDbContext.Posts.First(p => p.ID == post.ID);
                    postOld.Text = post.Text;
                    //postOld.Title = post.Title;

                    _appDbContext.Posts.Update(postOld);
                    _appDbContext.SaveChanges();
                    return postOld.ID;
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
    }
}
