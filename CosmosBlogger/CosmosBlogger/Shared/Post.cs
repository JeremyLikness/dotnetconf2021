namespace CosmosBlogger.Shared
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Uri? PermaLink { get; set; }

        public Blog? Blog { get; set; }
    }
}
