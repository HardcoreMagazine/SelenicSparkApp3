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

        public PostController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // v1: all at once;
        // v2: pagination
        [HttpGet(Name = "post")]
        public IEnumerable<Post> Get()
        {
            return _appDbContext.Posts?.ToList() ?? new List<Post>();
        }
    }
}
