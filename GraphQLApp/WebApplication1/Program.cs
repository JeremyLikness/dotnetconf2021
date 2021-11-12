using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddDbContextFactory<RecipesContext>(opt => opt.UseSqlite("Data Source=recipes.sqlite3"));
builder.Services.AddScoped<RecipesContext>(
    sp => sp.GetRequiredService<IDbContextFactory<RecipesContext>>()
    .CreateDbContext());
builder.Services.AddGraphQLServer().AddQueryType<Query>().AddProjections().AddFiltering().AddSorting();

var app = builder.Build();

await RecipesContext.CheckAndSeedDatabaseAsync(
    app.Services.GetRequiredService<IDbContextFactory<RecipesContext>>()
    .CreateDbContext());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapGraphQL("/graphql");
app.Run();
