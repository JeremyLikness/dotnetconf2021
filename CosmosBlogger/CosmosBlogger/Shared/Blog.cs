namespace CosmosBlogger.Shared
{
    public class Blog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Uri? Link { get; set; }
        public Uri? Image { get; set; }

        public IList<Post> Posts { get; set; } = new List<Post>();
    }
}
