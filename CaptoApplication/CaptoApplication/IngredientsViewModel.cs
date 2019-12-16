using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CaptoApplication
{
    public class IngredientsViewModel
    {

        public ObservableCollection<Ingredient> IngredientList { get; set; }

        public IngredientsViewModel()
        {
            IngredientList = new ObservableCollection<Ingredient>()
            {
                new Ingredient(){Name ="Köttfärs", Measure ="500G" },
                new Ingredient(){Name = "Korv", Measure = "4"}
            };

        }
    }
}
