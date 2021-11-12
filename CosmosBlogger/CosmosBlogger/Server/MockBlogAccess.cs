using CosmosBlogger.Shared;

namespace CosmosBlogger.Server
{
    public class MockBlogAccess : IBlogAccess
    {
        private static readonly IList<Blog> fakeBlogs = new List<Blog>();

        static MockBlogAccess()
        {
            var blog1 = new Blog
            {
                Image = new Uri("data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAsJCQcJCQcJCQkJCwkJCQkJCQsJCwsMCwsLDA0QDBEODQ4MEhkSJRodJR0ZHxwpKRYlNzU2GioyPi0pMBk7IRP/2wBDAQcICAsJCxULCxUsHRkdLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCwsLCz/wAARCACgAUEDASIAAhEBAxEB/8QAGwABAAIDAQEAAAAAAAAAAAAAAAUGAQMEBwL/xABOEAABAwMBBQMFCQsKBwEAAAABAAIDBAUREgYTITFBFFFhFSIycYEWIzU2c3SRk6EkUlRVZHWSsrPC0TM0U2KxwcPS4eJCY4KDorTw8f/EABsBAQACAwEBAAAAAAAAAAAAAAAFBgEEBwMI/8QALBEBAAICAAQEBgEFAAAAAAAAAAECAwQFETFBBiFhkRIUFRZRUiITccHR4f/aAAwDAQACEQMRAD8A9bREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREGUREGEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERBlERBhERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEWUQYREQYKg59p7NTzz08nad5BK+J+mHI1MOk4OVO9y8su3wpd/n1T+0Kj97Ytr0i1O6b4Po4t3JauXnyiOy5e62xflf1P+5PdbYvyv6n/cqAiifqeb0WT7e1PX3/AOPS2322uomXBu/7O6odTD3vz943OfNzy4FafdNZ/wAp+qH8VXYPitT/AJ2m/wARRy1d3jOxgvWtOXnET0/Kh78Rr7N8VOkTMLn7prP+U/VD/MtsF/tdTPBTxb/eTP0M1R4GcE8TnwVH9i7rR8KWz5f9x68MHHdnJlrSYjlMxHRpRltMvQFoq6qGipp6qbVuoGF79A1O05xwC3hRW0PwJdvmx/WCuzaZtt9tl1lmhpTMJImCRzZo9GWF2nLeJ5dfWpReZ217rTU7O3TJ7PWiohqO5obM6CQezzHewq9XqvFttlZVBwEgZu4OXGaTzWn2cz6lkc42mszq1lA107p31PZGlsWYzJktyHZ5eK2y362w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"),
                Title = "Codes",
                Description = "Code all the things.",
                Link = new Uri("http://code.localhost/")
            };

            var post1 = new Post
            {
                Blog = blog1,
                Description = "Something to talk about.",
                Title = "A Thing",
                PermaLink = new Uri("http://code.localhost/a-thing")
            };

            var post2 = new Post
            {
                Blog = blog1,
                Description = "Something else to talk about.",
                Title = "Another Thing",
                PermaLink = new Uri("http://code.localhost/another-thing")
            };

            blog1.Posts.Add(post1);
            blog1.Posts.Add(post2);

            var blog2 = new Blog
            {
                Image = new Uri("data:image/jpeg;base64,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"),
                Title = "Gasterology",
                Description = "Bradyisms",
                Link = new Uri("http://brady.localhost/")
            };

            var post3 = new Post
            {
                Blog = blog2,
                Description = "Brady loves .NET",
                Title = "Brady's Love of .NET",
                PermaLink = new Uri("http://brady.localhost/post1")
            };

            blog2.Posts.Add(post3);

            fakeBlogs.Add(blog1);
            fakeBlogs.Add(blog2);
        }
        public Task<Blog> GetBlogAsync(Guid blogId) =>
            Task.FromResult(
                fakeBlogs
                    .Where(b => b.Id == blogId)
                    .Select(b => new Blog
                    {
                        Id = b.Id,
                        Description = b.Description,
                        Title = b.Title,
                        Image = b.Image,
                        Link = b.Link
                    })
                    .FirstOrDefault());

        public Task<IEnumerable<Blog>> GetBlogsAsync() =>
            Task.FromResult(
                fakeBlogs
                    .Select(b => new Blog
                    {
                        Id = b.Id,
                        Description = b.Description,
                        Title = b.Title,
                        Image = b.Image,
                        Link = b.Link
                    })
                    .OrderBy(b => b.Title)
                    .AsEnumerable());

        public Task<IEnumerable<Post>> GetFilteredPostsForBlogAsync(Guid blogId, string filter) =>
            Task.FromResult(
                fakeBlogs
                    .Where(b => b.Id == blogId)
                    .SelectMany(b => b.Posts
                                .Where(p => $"{p.Title}{p.Description}"
                                    .ToLowerInvariant()
                                    .IndexOf(filter.ToLowerInvariant()) > 0)
                    .Select(p => new Post()
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        PermaLink = p.PermaLink,
                        Blog = new Blog() { Id = blogId }
                    })
                    .OrderBy(p => p.Title)
                    .AsEnumerable()));
    }
}
