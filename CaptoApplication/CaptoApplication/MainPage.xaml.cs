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
using Plugin.LocalNotifications;

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

        public string selectedCategory { get; set; }

        ZXingScannerPage scanPage;
        public MainPage()
        {
           
            InitializeComponent();

            PersonalIngredientList = new List<Ingredient>();
            RecipeList = new List<Recipe>();

            db = new DataBase();        
            db.createDataBase();
            PersonalIngredientList = db.GetIngredientsItems();
            PersonalIngredientList.Sort((a, b) => a.Date.CompareTo(b.Date));
            Model = new IngredientsViewModel(GreenOrRed(PersonalIngredientList));

            RModel = new RecipeViewModel();

            BindingContext = Model;
            categoryPicker.SelectedIndex = 0;

        }

        private List<Ingredient> GreenOrRed(List<Ingredient> ingredients)
        {
            foreach (var item in ingredients)
            {
                if ((item.Date - DateTime.Today).TotalDays < 3)
                {
                    item.Color = "#a40021";
                }
                else
                {
                    item.Color = "DarkCyan";
                }
            }

            return ingredients;
        }

        async void IngredientSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            if (IngredientSearchBar.Placeholder == "Sök recept" || IngredientSearchBar.Placeholder == "Hittade inga recept")
            {
                var keyword = IngredientSearchBar.Text;

                IngredientSearchBar.Text = null;

                IngredientSearchBar.Placeholder = RandomFunctionality.WhatMeal("Alla recept");

                progbar.IsVisible = true;
                progbar.Progress = 0;

                progbar.ProgressTo(0.65, 7000, Easing.Linear);

                RModel.RecipeList.Clear();

                RecipeListView.BindingContext = RModel;
                
                List<string> recipes = new List<string> { };
               
                var scraper = new RecipesScraper();

                await Task.WhenAll(
                scraper.GetRecipesTasteline(keyword, "1"),
                scraper.GetRecipesTasteline(keyword, "2"),
                scraper.GetRecipesTasteline(keyword, "3"),
                scraper.GetRecipesKoket(keyword),
                scraper.GetRecipesMittkok(keyword),
                scraper.GetRecipesCoop(keyword)
                );

                var recipeList = scraper.ListSorter(scraper.ListOfRecipes);

                if (recipeList.Count == 0)
                {
                    IngredientSearchBar.Placeholder = "Hittade inga recept";

                }
                else
                {
                    foreach (Recipe recipe in recipeList)
                    {
                        recipes.Add(recipe.Title);
                        RModel.RecipeList.Add(recipe);
                    }
                    IngredientSearchBar.Placeholder = "Sök recept";
                }
                
                await progbar.ProgressTo(1, 600, Easing.Linear);
                progbar.IsVisible = false;
            }

        }

        private async void btnadd_Clicked(object sender, EventArgs e)
        {

            await btnadd.ScaleTo(1.1, 80);
            await btnadd.ScaleTo(1, 80, Easing.BounceOut);


            var pop = new PopUp();
            await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            pop.OnDialogClosed += (s, arg) =>
            {
                string productname = arg.ProductName;


                if (!string.IsNullOrWhiteSpace(productname) && !productname.Equals(""))
                {

                    productname = char.ToUpper(productname[0]) + productname.Substring(1);
                    DateTime date = arg.ExpirationDate;
                    var ingredient = new Ingredient(productname, date);
                    db.InsertIntoTable(ingredient);
                    PersonalIngredientList.Add(ingredient);
                    PersonalIngredientList.Sort((a, b) => a.Date.CompareTo(b.Date));
                    Model = new IngredientsViewModel(PersonalIngredientList);
                    BindingContext = Model;

                    CrossLocalNotifications.Current.Show("Utgående vara", "Din vara '" + ingredient.Name + "' går ut snart! Använd vår sökfunktion för att hitta passande recept:)", ingredient.ID, date.AddDays(-2).AddHours(16));

                }
               

            };
            
        }

        private async void btnscan_Clicked(object sender, EventArgs e)
        {
            await btnscan.ScaleTo(1.1, 80);
            await btnscan.ScaleTo(1, 80, Easing.BounceOut);

            scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;

                var pop = new PopUp(BarCodeManager.getCorrectName(BarCodeManager.getBarNameDabas(result.Text)));

                App.Current.MainPage.Navigation.PushPopupAsync(pop, true);

                pop.OnDialogClosed += (s, arg) =>
                {
                    string productname = arg.ProductName;
                    if (!string.IsNullOrWhiteSpace(productname) && !productname.Equals(""))
                    {
                        productname = char.ToUpper(productname[0]) + productname.Substring(1);
                        DateTime date = arg.ExpirationDate;
                        var ingredient = new Ingredient(productname, date);
                        db.InsertIntoTable(ingredient);
                        PersonalIngredientList.Add(ingredient);
                        Model.IngredientList.Add(ingredient);
                        PersonalIngredientList.Sort((a, b) => a.Date.CompareTo(b.Date));
                        Model = new IngredientsViewModel(PersonalIngredientList);
                        BindingContext = Model;

                        CrossLocalNotifications.Current.Show("Utgående vara", "Din vara '" + ingredient.Name + "' går ut snart! Använd vår sökfunktion för att hitta passande recept:)", ingredient.ID, date.AddDays(-2).AddHours(16));
                    }
                    else
                    {
                        pop.changePlaceholder("Kunde inte hitta varan");

                    }

                };

                //Gör något med "result"
                Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PopModalAsync();
                        //DisplayAlert("Scanned Barcode", result.Text, "OK");

                        //string textresult = BarCodeManager.getBarName(result.Text);
                    });

        
            };
            
            await Navigation.PushModalAsync(scanPage);

        }
       
        private void removeitembtn_Clicked(object sender, EventArgs e)
        {
            
            var button = sender as ImageButton;

            var ingredient = button?.BindingContext as Ingredient;

            var vm = BindingContext as IngredientsViewModel;

            CrossLocalNotifications.Current.Cancel(ingredient.ID);

            PersonalIngredientList.Remove(ingredient);
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

        }

        private async void checkBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            
            foreach (Ingredient ingredientLoop in PersonalIngredientList)
            {
                if (ingredientLoop.selectedItem)
                {
                    categoryPicker.IsEnabled = true;
                    btnsearch.IsEnabled = true;
                    await Task.WhenAll(categoryPicker.FadeTo(1, 400, Easing.Linear), btnsearch.FadeTo(1, 400, Easing.Linear));
                    
                    return;
                }
                
            }
            categoryPicker.IsEnabled = false;
            btnsearch.IsEnabled = false;
            await Task.WhenAll(categoryPicker.FadeTo(0, 400, Easing.Linear), btnsearch.FadeTo(0, 400, Easing.Linear));
            

        }

        private async void btnsearch_Clicked(object sender, EventArgs e)
        {
            await btnsearch.ScaleTo(1.1, 80);
            await btnsearch.ScaleTo(1, 80, Easing.BounceOut);

            if (IngredientSearchBar.Placeholder == "Sök recept" || IngredientSearchBar.Placeholder == "Hittade inga recept")
            {
                IngredientSearchBar.Placeholder = RandomFunctionality.WhatMeal(selectedCategory);

                tp.CurrentPage = tp.Children[1];

                progbar.IsVisible = true;
                progbar.Progress = 0;

                progbar.ProgressTo(0.65, 7000, Easing.Linear);

                List<string> listan = new List<string>();

                foreach (var item in PersonalIngredientList)
                {
                    if (item.selectedItem)
                    {
                        listan.Add(item.Name);
                    }
                }

                string searchword = selectedCategory;

                if (selectedCategory.Equals("Alla recept"))
                {
                    searchword = "";
                }

                foreach (var ord in listan)
                {
                    searchword += " " + ord;
                }

                RModel.RecipeList.Clear();

                RecipeListView.BindingContext = RModel;
                
                List<string> recipes = new List<string> { };

                var scraper = new RecipesScraper();

                await Task.WhenAll(
                scraper.GetRecipesTasteline(searchword, "1"),
                scraper.GetRecipesTasteline(searchword, "2"),
                scraper.GetRecipesTasteline(searchword, "3"),
                scraper.GetRecipesKoket(searchword),
                scraper.GetRecipesMittkok(searchword),
                scraper.GetRecipesCoop(searchword)
                );

                var recipeList = scraper.ListSorter(scraper.ListOfRecipes);

                if (recipeList.Count == 0)
                {
                    IngredientSearchBar.Placeholder = "Hittade inga recept";

                }
                else
                {
                    foreach (Recipe recipe in recipeList)
                    {
                        recipes.Add(recipe.Title);
                        RModel.RecipeList.Add(recipe);
                    }
                    IngredientSearchBar.Placeholder = "Sök recept";
                }

                await progbar.ProgressTo(1, 600, Easing.Linear);
                progbar.IsVisible = false;
                
            }

        }

        private void categoryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var category = picker.SelectedItem;
            selectedCategory = category.ToString();
        }

    }
    
}
