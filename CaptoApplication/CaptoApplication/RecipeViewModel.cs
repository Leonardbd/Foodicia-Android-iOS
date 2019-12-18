using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace CaptoApplication
{
    public class RecipeViewModel
    {

        public ObservableCollection<Recipe> RecipeList { get; set; }

     
        public RecipeViewModel()
        {
            RecipeList = new ObservableCollection<Recipe>();

        }
        public RecipeViewModel(List<Recipe> list)
        {
            RecipeList = new ObservableCollection<Recipe>(list);

        }


    }
}
