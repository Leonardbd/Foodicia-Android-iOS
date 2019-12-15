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

        public string Image { get; set; }



        public Recipe(string title, string description, List<Ingredient> ingredients, string url )
        {
            Title = title;
            Description = description;
            Ingredients = ingredients;
            Url = url;

        }
    }
}
