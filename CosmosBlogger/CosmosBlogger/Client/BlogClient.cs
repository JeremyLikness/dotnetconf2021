using CosmosBlogger.Shared;
using System.Net.Http.Json;

namespace CosmosBlogger.Client
{
    public class BlogClient : IBlogAccess
    {
        private readonly HttpClient httpClient;

        public BlogClient(HttpClient client) => httpClient = client;

        public async Task<Blog> GetBlogAsync(Guid blogId) =>
            await httpClient.GetFromJsonAsync<Blog>($"api/blog/{blogId}");

        public async Task<IEnumerable<Blog>> GetBlogsAsync() =>
            await httpClient.GetFromJsonAsync<Blog[]>("api/blog");

        public async Task<IEnumerable<Post>> GetFilteredPostsForBlogAsync(Guid blogId, string filter)
        {
            var query = new PostQuery { BlogId = blogId, Filter = filter };
            var result = await httpClient.PostAsJsonAsync($"api/blog/{blogId}", query);
            return await result.Content.ReadFromJsonAsync<Post[]>();
        }                   
    }
}
