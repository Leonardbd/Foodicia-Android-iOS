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
            IngredientList = new ObservableCollection<Ingredient>();

        }
        public IngredientsViewModel(List<Ingredient> list)
        {
            IngredientList = new ObservableCollection<Ingredient>(list);

        }


    }
}
