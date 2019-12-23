using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace CaptoApplication
{
    public class IngredientsViewModel
    {
        
        public ObservableCollection<Ingredient> IngredientList { get; set; }
        public Command <Ingredient> RemoveCommand
        {
            get
            {
                return new Command<Ingredient>((ingredient) =>
                {
                    IngredientList.Remove(ingredient);
                    
                });
            }

        }

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
