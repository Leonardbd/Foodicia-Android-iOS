using System;
using System.Collections.Generic;
using System.Text;

namespace CaptoApplication
{
    public class Recipe
    {

        public string Url { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public int NumIngredients { get; set; }

        public int NumInRecipe { get; set; }

        public decimal Percentage { get; set; }

        public string Image { get; set; }


        public Recipe(string title, string description, List<Ingredient> ingredients, string url, int num, string image)
        {
            Title = title;
            Description = description;
            Ingredients = ingredients;
            Url = url;
            NumInRecipe = num;
            Image = image;

        }
    }
}
