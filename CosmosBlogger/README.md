# Cosmos Blogger

This demo was part of the main .NET Conf presentation for what's new with EF Core 6.

📽️ [Deck: What's New in EF Core 6](../Presentations/dotnetconf2021-efcore6-whatsnew.pptx)

🎞️ [Video: What's New in EF Core 6](https://www.youtube.com/watch?v=_1fJeW4F3ts)

## Loading the blog

To get started, you should have an instance of an Azure Cosmos DB account. You may also use the emulator. Create a new .NET 6 console app and paste the following code into the `Program.cs`. Change the blog feeds to any RSS feeds you would like, and update the endpoint and key. Run the program to populate the database.

```csharp
using CosmosBlogLoader;
using Microsoft.EntityFrameworkCore;
using System.Xml;

Console.WriteLine("Blog loader");

var urls = new[] { "https://devblogs.microsoft.com/dotnet/feed/", "https://blog.jeremylikness.com/blog/index.xml" };

var blogs = new List<Blog>();

foreach (var url in urls)
{
    var client = new HttpClient();
    var result = await client.GetAsync(url);
    result.EnsureSuccessStatusCode();
    var rawXml = await result.Content.ReadAsStringAsync();
    var rss = new XmlDocument();
    rss.LoadXml(rawXml);
    foreach (XmlElement channel in rss.DocumentElement.SelectNodes("channel"))
    {
        var blog = new Blog
        {
            Title = channel.SelectSingleNode("title").InnerText,
            Description = channel.SelectSingleNode("description").InnerText,
            Link = new Uri(channel.SelectSingleNode("link").InnerText),
        };

        var image = channel.SelectSingleNode("image");
        if (image != null)
        {
            blog.Image = new Uri(image.SelectSingleNode("url").InnerText);
        }

        foreach(XmlElement item in channel.SelectNodes("item"))
        {
            var post = new Post
            {
                Blog = blog,
                Title = item.SelectSingleNode("title").InnerText,
                Description = item.SelectSingleNode("description").InnerText,
                PermaLink = new Uri(item.SelectSingleNode("link").InnerText),
            };

            blog.Posts.Add(post);
        }

        blogs.Add(blog);
    }
}

using var context = new BlogContext();
await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();
context.Blogs.AddRange(blogs);
await context.SaveChangesAsync();

namespace CosmosBlogLoader
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Uri? PermaLink { get; set; }

        public Blog? Blog { get; set; }
    }

    public class Blog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Uri? Link { get; set; }
        public Uri? Image { get; set; }

        public IList<Post> Posts { get; set; } = new List<Post>();
    }

    public class BlogContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                "<your endpoint>",
                "<your key>",
                nameof(Blogs));
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Blog> Blogs { get; set; } = null!;
    }

}
```

## Performing the demo

Start by running the sample application and see the mocked data in action. Then in the server project, add a `DbContext`:

```csharp
public class BlogContext : DbContext
{
    public BlogContext() 
    {
    }

    public BlogContext(DbContextOptions<BlogContext> opts) : base(opts)
    {
    }

    public DbSet<Blog> Blogs { get; set; } = null!;
}
```

Implement the `IBlogAccess` interface with a class named `CosmosBlogAccess`:

```csharp
public class CosmosBlogAccess : IBlogAccess
{
    private readonly IDbContextFactory<BlogContext> context;
    public CosmosBlogAccess(IDbContextFactory<BlogContext> context) => this.context = context;

    public async Task<Blog> GetBlogAsync(Guid blogId)
    {
        using var ctx = context.CreateDbContext();
        return await ctx.Blogs
		.Where(b => b.Id == blogId)            
		.Select(b => new Blog
            {
                Id = blogId,
                Description = b.Description,
                Image = b.Image,
                Link = b.Link,
                Title = b.Title,
            })            
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Blog>> GetBlogsAsync()
    {
        using var ctx = context.CreateDbContext();
        return await ctx.Blogs
            .OrderBy(b => b.Title)
            .Select(b => new Blog
            {
                Id = b.Id,
                Description = b.Description,
                Image = b.Image,
                Link = b.Link,
                Title = b.Title,
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetFilteredPostsForBlogAsync(Guid blogId, string filter)
    {
        using var ctx = context.CreateDbContext();
        return (
            await ctx.Blogs
                .FromSqlRaw(@"
    SELECT c.Id, c.id, c.Discriminator, ARRAY(
    SELECT value p from p in c.Posts 
    WHERE c.Id = {0} AND EXISTS (
        SELECT 1 from p where contains(p.Title, {1}) or contains(p.Description, {1})) ) as Posts
    FROM c", blogId, filter)
        .ToListAsync())
        .SelectMany(b => b.Posts)
        .Select(p => PostClone(p))
        .OrderBy(p => p.Title)
        .ToList();
    }

    private static Post PostClone(Post p) => new()
    {
        Id = p.Id,
        Blog = new Blog { Id = p.Blog.Id },
        Description = p.Description,
        PermaLink = p.PermaLink,
        Title = p.Title
    };
}
```

Next, open `Program.cs` and locate this line:

```csharp
builder.Services.AddRazorPages();
```

Add this code after:

```csharp
var (endpoint, key) = SomeCompletelySecureLocation.Keys;
builder.Services.AddDbContextFactory<BlogContext>(
    opt => opt.UseCosmos(endpoint, key, nameof(BlogContext.Blogs)));
```

> __Note__: The `SomeCompletelySecureLocation` class contains the endpoint and keys for the Cosmos DB emulator. If you are using a "live" version, change the code to securely retrieve the keys from configuration or environment.

Finally, swap the service configuration for `MockBlogAccess` to `CosmosBlogAccess.`
