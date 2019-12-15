using System;
using System.Collections.Generic;
using System.Text;

namespace CaptoApplication
{
    public class Recipe
    {

        public string Url { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public Recipe(string name, string description, List<Ingredient> ingredients, string url )
        {
            Name = name;
            Description = description;
            Ingredients = ingredients;
            Url = url;

        }
    }
}
