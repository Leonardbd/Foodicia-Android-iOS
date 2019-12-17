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
                new Ingredient("Korv med bröd","2","20-12-2019")
            };

        }

        public IngredientsViewModel(Ingredient ingredient)
        {

        }
    }
}
