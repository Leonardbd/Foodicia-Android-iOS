using System;
using System.Collections.Generic;
using System.Text;

namespace CaptoApplication
{
    public class IngredientsViewModel
    {

        public List<Ingredient> IngredientList { get; set; }

        public IngredientsViewModel()
        {
            IngredientList = new List<Ingredient>()
            {
                new Ingredient("Köttfärs","500 G")
            };

        }
    }
}
