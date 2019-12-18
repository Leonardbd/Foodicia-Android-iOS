using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using ZXing.Net.Mobile.Forms;
using System.Diagnostics;
using System.Threading;
using Rg.Plugins.Popup.Extensions;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
namespace CaptoApplication
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {

        public DataBase db { get; set; }
        
        public List<Ingredient> PersonalIngredientList {get; set;}

        public List<Recipe> RecipeList { get; set; }

        public IngredientsViewModel Model { get; set; }

        public RecipeViewModel RModel { get; set; }
        

        ZXingScannerPage scanPage;
        public MainPage()
        {
            InitializeComponent();
            
            PersonalIngredientList = new List<Ingredient>();
            RecipeList = new List<Recipe>();

            db = new DataBase();
            
            db.createDataBase();
            PersonalIngredientList = db.GetIngredientsItems();

            Model = new IngredientsViewModel(PersonalIngredientList);
            RModel = new RecipeViewModel();

            BindingContext = Model;

            
        }

        
        async void IngredientSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            progbar.IsVisible = true;
            progbar.Progress = 0;            
            
            RModel.RecipeList.Clear();
            
            
            RecipeListView.BindingContext = RModel;
            progbar.ProgressTo(0.65, 2300, Easing.Linear);
            List<string> recipes = new List<string> { };
            

            var keyword = IngredientSearchBar.Text;
            var scraper = new RecipesScraper(keyword);
            
            var recipeList = await scraper.GetFirstPageRecipesURLsAsync();

            if (recipeList.Count == 0)
            {
                IngredientSearchBar.Text = "Hittade inga recept";
            }
            else
            {
                foreach (Recipe recipe in recipeList)
                {
                    recipes.Add(recipe.Title);
                    RModel.RecipeList.Add(recipe);
                }
                

                
            }
            await progbar.ProgressTo(1, 600, Easing.Linear);
            progbar.IsVisible = false;


        }


        private void btnadd_Clicked(object sender, EventArgs e)
        {
            var pop = new PopUp();
            App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            pop.OnDialogClosed += (s, arg) =>
            {
                string productname = arg.ProductName;
                string date = arg.ExpirationDate;
                var ingredient = new Ingredient(productname, date);
                db.InsertIntoTable(ingredient);
                PersonalIngredientList.Add(ingredient);
                Model.IngredientList.Add(ingredient);               

            };

        }

        private async void btnscan_Clicked(object sender, EventArgs e)
        {
            scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) => {
                scanPage.IsScanning = false;

                var pop = new PopUp(BarCodeManager.getBarName(result.Text));
                
                App.Current.MainPage.Navigation.PushPopupAsync(pop, true);

                pop.OnDialogClosed += (s, arg) =>
                {
                    string productname = arg.ProductName;
                    string date = arg.ExpirationDate;
                    var ingredient = new Ingredient(productname, date);
                    db.InsertIntoTable(ingredient);
                    PersonalIngredientList.Add(ingredient);
                    Model.IngredientList.Add(ingredient);

                };

                //Gör något med "result"
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopModalAsync();
                    //DisplayAlert("Scanned Barcode", result.Text, "OK");

                    string textresult = BarCodeManager.getBarName(result.Text);
                    
                    Debug.WriteLine(textresult);
                    

                });
            };

            await Navigation.PushModalAsync(scanPage);
        }

        private void btnremove_Clicked(object sender, EventArgs e)
        {
           
        }

        private void removeitembtn_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;

            var ingredient = button?.BindingContext as Ingredient;
            
            var vm = BindingContext as IngredientsViewModel;

            db.DeleteIngredientItem(ingredient);
            vm?.RemoveCommand.Execute(ingredient);
        }

        private void RecipeListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (sender is ListView lv) 
            {
                Recipe recipe = (Recipe) lv.SelectedItem;
                lv.SelectedItem = null;
            
                Browser.OpenAsync(recipe.Url, BrowserLaunchMode.SystemPreferred);

            }

            Debug.WriteLine("HEJ");
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;

            var ingredient = button?.BindingContext as Ingredient;

            var vm = BindingContext as IngredientsViewModel;

            db.DeleteIngredientItem(ingredient);
            vm?.RemoveCommand.Execute(ingredient);
        }

        private void checkBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                btnsearch.IsVisible = true;
            }
            else btnsearch.IsVisible = false;
        }
    }
    
}
