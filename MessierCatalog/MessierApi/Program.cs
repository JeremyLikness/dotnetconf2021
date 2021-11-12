using DataAccess;
using Microsoft.EntityFrameworkCore;

const string rootPath = @"<path to folder that contains messier.db>";

var builder = WebApplication.CreateBuilder(args);

var dbPath = Path.Combine(rootPath, "messier.db");

builder.Services.AddDbContextFactory<MessierContext>(opt => opt.UseSqlite(
    $"Data Source={dbPath}").LogTo(
    Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }));

using var htmlStream = typeof(Program).Assembly.GetManifestResourceStream("MessierApi.index.html");
using var htmlReader = new StreamReader(htmlStream);
var html = htmlReader.ReadToEnd();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", async (HttpResponse resp) =>
{
    resp.ContentType = "text/html";
    await resp.WriteAsync(html.Replace("{port}", resp.HttpContext.Request.Host.Port.ToString()));
});

app.MapGet("/messier", async (IDbContextFactory<MessierContext> factory) =>
{
    using var ctx = factory.CreateDbContext();
    var result = await ctx.Targets.OrderBy(t => t.Index).ToListAsync();
    return result;
});

app.MapGet("/messier/{id}", async (IDbContextFactory<MessierContext> factory, Guid id) =>
{
    using var ctx = factory.CreateDbContext();
    return await ctx.Targets.FirstOrDefaultAsync(tgt => tgt.Id == id);
});

app.Run();