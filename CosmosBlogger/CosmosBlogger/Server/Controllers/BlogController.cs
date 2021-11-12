using CosmosBlogger.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CosmosBlogger.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogAccess access;

        public BlogController(IBlogAccess access) => this.access = access;

        // GET: api/Blog
        [HttpGet]
        public async Task<IEnumerable<Blog>> GetAsync() => await access.GetBlogsAsync();

        // GET api/Blog/{guid}
        [HttpGet("{id:Guid}")]
        public async Task<Blog> GetBlogAsync(Guid id) => await access.GetBlogAsync(id);

        // POST api/{id}
        [HttpPost("{id:Guid}")]
        public async Task<IEnumerable<Post>> Post(Guid id, [FromBody] PostQuery query)
        {
            if (query == null)
            {
                return null;
            }

            if (query.BlogId != id)
            {
                throw new InvalidOperationException($"Blog id in query ({query.BlogId} does not match url ({id}).");
            }

            return await access.GetFilteredPostsForBlogAsync(id, query.Filter);
        }
    }
}
