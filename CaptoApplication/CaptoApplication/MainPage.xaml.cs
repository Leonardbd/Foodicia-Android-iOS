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

namespace CaptoApplication
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {

        public DataBase db { get; set; }
        
        public List<Ingredient> PersonalIngredientList {get; set;}

        public IngredientsViewModel Model { get; set; }
        

        ZXingScannerPage scanPage;
        public MainPage()
        {
            InitializeComponent();
            
            PersonalIngredientList = new List<Ingredient>();         

            db = new DataBase();
            
            db.createDataBase();
            PersonalIngredientList = db.GetIngredientsItems();

            Model = new IngredientsViewModel(PersonalIngredientList);

            BindingContext = Model;

            
        }

       
      

        void IngredientSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {

            List<string> recipes = new List<string> { };
            
            var keyword = IngredientSearchBar.Text;
            var scraper = new RecipesScraper(keyword);
            scraper.GetFirstPageRecipesURLsAsync();

            foreach (Recipe recipe in scraper.ListOfRecipes)
            {
                recipes.Add(recipe.Title);
                Debug.WriteLine("RECIPE \n RECIPE \n RECIPE \n" + recipe.Title);

            }

            RecipeListView.ItemsSource = recipes;
        }


        private void btnadd_Clicked(object sender, EventArgs e)
        {
            var pop = new PopUp();
            App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            pop.OnDialogClosed += (s, arg) =>
            {
                string productname = arg.ProductName;
                string measure = arg.Measurement;
                string date = arg.ExpirationDate;
                var ingredient = new Ingredient(productname, measure, date);
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

                var pop = new PopUp(result.Text);
                
                App.Current.MainPage.Navigation.PushPopupAsync(pop, true);

                pop.OnDialogClosed += (s, arg) =>
                {
                    string productname = arg.ProductName;
                    string measure = arg.Measurement;
                    string date = arg.ExpirationDate;
                    var ingredient = new Ingredient(productname, measure, date);
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
    }
}
