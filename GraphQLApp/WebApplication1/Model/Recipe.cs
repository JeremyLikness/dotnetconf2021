using HotChocolate.Data;

namespace WebApplication1.Model
{
    public class Recipe : BaseModel
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<Step> Steps { get; set; } = new List<Step>();

        [UseSorting]
        public ICollection<Ingredient> Ingredients {  get; set; } = 
            new List<Ingredient>();

        public override string ToString() => $"{Name}: {Description}";
    }
}
