using HotChocolate;
using HotChocolate.Data;
using WebApplication1.Model;

namespace WebApplication1.Data
{
    public class Query
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]        
        public IQueryable<Recipe> GetRecipes([Service] RecipesContext context) =>
            context.Recipes;
    }
}
