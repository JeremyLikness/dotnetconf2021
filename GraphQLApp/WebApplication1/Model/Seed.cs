namespace WebApplication1.Model
{
    public static class Seed
    {
        public static IList<Recipe> GetRecipes()
        {
            var recipes = new List<Recipe>()
            {
                new Recipe()
                {
                    Name = "Garlic Three Pepper Savage Sauce",
                    Description = "So hot it'll make your neighbor sweat",
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient ()
                        {
                            WholeQty = 10,
                            Measurement = "Piece(s)",
                            Name = "Habanero"
                        },
                        new Ingredient ()
                        {
                            WholeQty = 10,
                            Measurement = "Piece(s)",
                            Name = "Serrano"
                        },
                        new Ingredient ()
                        {
                            WholeQty = 10,
                            Measurement = "Piece(s)",
                            Name = "Jalafuego"
                        },
                        new Ingredient ()
                        {
                            WholeQty = 2,
                            Measurement = "Piece(s)",
                            Name = "Garlic Clove"
                        },
                        new Ingredient ()
                        {
                            Numerator = 1,
                            Denominator = 2,
                            Measurement = "Piece(s)",
                            Name = "White onion"
                        },
                        new Ingredient ()
                        {
                            Numerator = 1,
                            Denominator = 2,
                            Measurement = "Piece(s)",
                            Name = "Lime"
                        },
                        new Ingredient ()
                        {
                            Numerator = 1,
                            Denominator = 2,
                            Measurement = "Cup(s)",
                            Name = "White vinegar"
                        },
                    },
                    Steps = new List<Step>
                    {
                        new Step ()
                        {
                            Order = 1,
                            Duration = TimeSpan.FromMinutes(2),
                            Instructions = "Smash cloves, dice onion, and cut peppers in half and arrange on a baking sheet."
                        },
                        new Step ()
                        {
                            Order = 2,
                            Duration = TimeSpan.FromMinutes(20),
                            Instructions = "Pre-heat oven to 425 degrees and place tray on middle rack. Bake until the peppers start turning black. Burnt and bubbling pepper skin is fine."
                        },
                        new Step ()
                        {
                            Order = 3,
                            Duration = TimeSpan.FromMinutes(10),
                            Instructions = "Remove tray from oven and allow to cool. The peppers with thicker skin will have wrinkled skins that you may wish to remove."
                        },
                        new Step ()
                        {
                            Order = 4,
                            Duration = TimeSpan.FromMinutes(5),
                            Instructions = "Combine contents of tray with vinegar and lime juice in a food processor or blender. Blend until smooth. Store in a sealable container. Can refrigerate for up to 3 weeks."
                        }
                    }
                }
            };

            return recipes;
        } 
    }
}
