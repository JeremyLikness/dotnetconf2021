namespace CosmosBlogger.Shared
{
    public interface IBlogAccess
    {
        public Task<IEnumerable<Blog>> GetBlogsAsync();
        public Task<Blog> GetBlogAsync(Guid blogId);
        public Task<IEnumerable<Post>> GetFilteredPostsForBlogAsync(Guid blogId, string filter);
    }
}
