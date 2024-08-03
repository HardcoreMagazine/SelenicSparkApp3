using Microsoft.AspNetCore.Mvc;
using PostsService.Data;
using PostsService.Models;
using PostsService.Models.DTO;

namespace PostsService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<PostController> _logger;

        public PostController(ILogger<PostController> logger, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateNew([FromBody] PostRequest pr)
        {
            var post = Post.MapToPostFromRequest(pr);
            if (!Post.Validate(post))
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    await _appDbContext.Posts.AddAsync(post);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(post.ID); // id is returned after insertion (automatically)
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
        }

        // v1: all at once;
        // v2: pagination
        [HttpGet]
        public async Task<ActionResult<List<PostResponse>>> GetAll()
        {
            try
            {
                var data = await Task.FromResult(_appDbContext.Posts
                    .Where(p => p.Enabled)
                    .OrderBy(p => p.ID)
                    .ToList());
                var result = new List<PostResponse>();
                if (data.Count == 0)
                {
                    return Ok(result);
                }
                else
                {
                    foreach (var post in data)
                    {
                        result.Add(Post.MapToResponseFromPost(post));
                    }
                    return Ok(result);
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                return Ok(new List<PostResponse>());
            }
            catch (Exception)// ex)
            {
                //_logger.LogWarning($"{DateTimeOffset.Now} - WARN: {ex}");
                return Ok(new List<PostResponse>());
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PostResponse>> GetID(int id)
        {
            try
            {
                var post = await Task.FromResult(_appDbContext.Posts.FirstOrDefault(p => p.ID == id && p.Enabled));
                if (post == null)
                {
                    return NotFound();
                }
                else
                {
                    var pr = Post.MapToResponseFromPost(post);
                    return Ok(pr);
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

        [HttpPut]
        public async Task<ActionResult<int>> Update([FromBody] Post updatedPost)
        {
            if (!Post.Validate(updatedPost) || !updatedPost.Enabled || updatedPost.ID <= 0)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    // safe update: automatically block all attempts to change all fields except "Text"
                    var oldPost = _appDbContext.Posts.First(p => p.ID == updatedPost.ID);
                    oldPost.Text = updatedPost.Text;

                    _appDbContext.Posts.Update(oldPost);
                    await _appDbContext.SaveChangesAsync();
                    
                    return Ok(oldPost.ID);
                }
                catch (Npgsql.PostgresException ex)
                {
                    _logger.LogError($"{DateTimeOffset.Now} - ERROR: {ex}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                catch (Exception)// ex)
                {
                    //_logger.LogWarning($"{DateTimeOffset.Now} - ERROR: {ex.Message} | SRC: {ex.StackTrace}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var post = _appDbContext.Posts.FirstOrDefault(p => p.ID == id && p.Enabled);

            if (post == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    post.Enabled = false;
                    _appDbContext.Update(post);
                    await _appDbContext.SaveChangesAsync();
                    return Ok();
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
        }
    }
}
