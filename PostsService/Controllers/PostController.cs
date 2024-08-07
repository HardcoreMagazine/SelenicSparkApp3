using Generics.Models;
using Microsoft.AspNetCore.Mvc;
using PostsService.Models.Data;
using PostsService.Models.DTO;
using PostsService.Service;

namespace PostsService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly IRepository<Post> _postManager;

        public PostController(ILogger<PostController> logger, IRepository<Post> postManager)
        {
            _logger = logger;
            _postManager = postManager;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateNew([FromBody] PostRequest pr)
        {
            var post = PostMapper.MapToPostFromRequest(pr);
            if (!Post.Validate(post))
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var id = await _postManager.CreateAsync(post);
                    return Ok(id);
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
                var data = await _postManager.GetAllAsync();
                var result = new List<PostResponse>();
                if (data.Count == 0)
                {
                    return Ok(result);
                }
                else
                {
                    foreach (var post in data)
                    {
                        result.Add(PostMapper.MapToResponseFromPost(post));
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
                var post = await _postManager.GetAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                else
                {
                    var pr = PostMapper.MapToResponseFromPost(post);
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
        public async Task<ActionResult<int>> Update([FromBody] PostResponse pr)
        {
            // instead of exposing our entire Post model we use DTO we've sent to the user
            // (likely modified by the user)
            var post = PostMapper.MapToPostFromResponse(pr);
            if (!Post.Validate(post))
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var result = await _postManager.UpdateAsync(post);
                    if (result)
                        return Ok(post.ID);
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
                    //_logger.LogWarning($"{DateTimeOffset.Now} - ERROR: {ex.Message} | SRC: {ex.StackTrace}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _postManager.DeleteAsync(id);
                if (result)
                    return Ok();
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
    }
}
