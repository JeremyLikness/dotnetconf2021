# Messier Catalog GraphQL with EF Core 6 Demo

This demo is intended to show both how well HotChocolate integrates with EF Core, but also how easy it is to use GraphQL to stand up a rich API. The following steps can be used to walk through the demo.

📽️ [Presentation deck](../Presentations/dotnetconf2021-efcore6-graphql.pptx)

🎞️ [Video](https://youtu.be/GBvTRcV4PVA)

## Setup

The application requires a SQLite database. Edit the path in `Program.cs` in the `DatabaseLoader` project to point to the `messier` subdirectory that contains images and the `mcatalog.tsv` source data. Run the `DatabaseLoader` console application. Note the location of the generated `messier.db` and update the root in `Program.cs` in the `WebApi` project.

## Add GraphQL

The references have already been added to the `WebApi` project. In the root of the `WebApi` project, add the `Query` class:

```csharp
public class Query
{
    public IQueryable<MessierTarget> GetTargets() =>
        new List<MessierTarget>().AsQueryable();
}
```

In the `Program.cs` add the middleware configuration _after_ the `DbContextFactory` setup.

```csharp
builder.Services.AddGraphQLServer().AddQueryType<Query>();
```

After the `app.UseHttpsRedirection();` statement, configure the GraphQL endpoint:

```csharp
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapGraphQL("/graphql"));
```

Run the application. Navigate to `/graphql` and explore the schema.

## Connect GraphQL to EF Core

Configure an instance of the `MessierContext` by using the context factory. Place this code after the factory is configured:

```csharp
builder.Services.AddScoped<MessierContext>(sp => 
    sp.GetRequiredService<IDbContextFactory<MessierContext>>()
    .CreateDbContext());
```

Update the `Query` class to use the context and extend the schema to support projections, filtering, and sorting.

```csharp
public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MessierTarget> GetTargets([Service] MessierContext ctx) =>
        ctx.Targets;
}
```

Update the middleware to add the new services.

```csharp
builder.Services.AddGraphQLServer().AddQueryType<Query>().AddProjections().AddFiltering().AddSorting();
```

Copy the contents of `interimindex.txt` into the `<script>` tag in `index.html` to use GraphQL instead of REST.

Run and point out that the images are not in order and show uninteresting entries with question marks. Open the server output to show that the generated SQL matches the projections.

## Show filtering, sorting, and projections

Add the "loading" image by copying the `<script>` block in `finalindex.txt` and pasting it just after the `<body>` tag. Then, copy the remaining code and replace the contents of the JavaScript `<script>` tag. Re-run and show the updates.
