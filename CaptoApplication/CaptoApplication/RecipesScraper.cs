using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CaptoApplication
{

    
    public class RecipesScraper
    {

        int IndexOfSecond(string theString, string toFind)
        {
            int first = theString.IndexOf(toFind);

            if (first == -1) return -1;

            // Find the "next" occurrence by starting just past the first
            return theString.IndexOf(toFind, first + 1);
        }
        public string Url { get; set; }
        public HttpClient httpclient { get; set; }

        public string Searchword { get; set; }
        public int? NumPages { get; set; }
        public List<string> ListRecipeURL { get; set; }

        //public List<Recipe> ListOfRecipes { get; set; }

        public RecipesScraper(string searchword)
        {
            Url = "https://www.coop.se/globalt-sok/?query=" + searchword;
            ListRecipeURL = new List<string>();
            //ListOfRecipes = new List<Recipe>();
            Searchword = searchword;

        }

        
        public async Task<List<Recipe>> GetFirstPageRecipesURLsAsync()
        {
                List<Recipe> ListOfRecipes = new List<Recipe>();

                httpclient = new HttpClient();
                var html = await httpclient.GetStringAsync(Url);

                var htmldoc = new HtmlDocument();
                htmldoc.LoadHtml(html);

                var ReceptLista = new List<HtmlNode>();
                ReceptLista = htmldoc.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("Grid Grid--recipe Grid--gutterAxsm js-recipesSearchResultList u-lg-marginTz")).ToList();

                var finalList = new List<HtmlNode>();
                finalList = ReceptLista[0].Descendants("article").ToList();

                Match m;
                string HRefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";

                try
                {
                    foreach (var item in finalList)
                    {
                        m = Regex.Match(item.InnerHtml, HRefPattern,
                                        RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                        TimeSpan.FromSeconds(1));
                        while (m.Success)
                        {

                            if (m.Groups[1].Index == 75)
                            {

                                Debug.WriteLine("Found href " + m.Groups[1].Value);

                                

                                    string url = "https://www.coop.se" + m.Groups[1].Value;

                                ListRecipeURL.Add(url);



                            using (HttpClient client = new HttpClient())
                            {

                                var html2 = await client.GetStringAsync(url);
                                HtmlDocument htmldoc2 = new HtmlDocument();
                                htmldoc2.LoadHtml(html2);


                                var list = new List<HtmlNode>();
                                list = htmldoc2.DocumentNode.Descendants("div")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Equals("u-paddingVxlg u-paddingTlg u-sm-paddingTxlg u-paddingHlg u-lg-paddingHxlg u-lg-paddingBz u-bgWhite")).ToList();
                                string innerhtmlH1 = list[0].InnerHtml;

                                //getImage
                                var htmlList= new List<HtmlNode>();
                                htmlList = htmldoc2.DocumentNode.Descendants("img")
                                    .Where(node => node.GetAttributeValue("class", "")
                                    .Equals("u-hiddenVisually")).ToList();

                                string image = "https:"+ htmlList[0].Attributes[2].Value;
                                Debug.WriteLine("Image: " + image);

                                //getTitle

                                string title = innerhtmlH1.Substring(innerhtmlH1.IndexOf(">") + 1, IndexOfSecond(innerhtmlH1, "<") - (innerhtmlH1.IndexOf(">") + 1));
                                title = convertUTF(title);
                                Debug.WriteLine("Titel: " + title);


                                //getDescription

                                string description = innerhtmlH1.Substring(innerhtmlH1.IndexOf("<p") + 47, innerhtmlH1.IndexOf("</p>") - (innerhtmlH1.IndexOf("<p") + 47));
                                description = convertUTF(description);
                                Debug.WriteLine("Beskrivning: " + description);


                                //GetIngredienserToList

                                var ingredienthtml = htmldoc2.DocumentNode.Descendants("ul")
                                                            .Where(node => node.GetAttributeValue("class", "")
                                                            .Equals("List List--section")).ToList();


                                var ingredientList = ingredienthtml[0].Descendants("li")
                                                                    .Where(node => node.GetAttributeValue("class", "")
                                                                    .Equals("u-paddingHxsm u-textNormal u-colorBase")).ToList();

                                var ingredientList2 = new List<HtmlNode>();

                                foreach (HtmlNode nod in ingredientList)
                                {
                                    ingredientList2.Add(nod.SelectSingleNode("span[@class='u-textWeightBold ']"));
                                }

                                var ingrediensLista = new List<Ingredient>();
                                int counter = 0;
                                  foreach (var ingredient in ingredientList2)
                                    {

                                    string ingredientString = convertUTF(ingredient.InnerHtml);
                                    ingrediensLista.Add(new Ingredient(ingredientString));
                                    
                                    Debug.WriteLine("Ingrediens: " + ingredientString);
                                    counter++;
                                    if(counter == ingredientList.Count)
                                    {
                                        var recipe = new Recipe(title, description, ingrediensLista, url, ingredientList.Count, image);

                                        ListOfRecipes.Add(recipe);
                                        SetRecipeMatches(recipe);
                                    }

                                    }
                                

                            }


                        }
                            m = m.NextMatch();
                        }
                    }
                    

                }
                catch (RegexMatchTimeoutException)
                {
                    Console.WriteLine("The matching operation timed out.");
                }
            return ListSorter(ListOfRecipes);

        }

        private string convertUTF(string text)
        {

            if (text.Contains("&#246;"))
            {
                text = text.Replace("&#246;", "ö");
            }
            if (text.Contains("&#228;"))
            {
                text = text.Replace("&#228;", "ä");
            }
            if (text.Contains("&#229;"))
            {
                text = text.Replace("&#229;", "å");
            }
            if (text.Contains("&#214;"))
            {
                text = text.Replace("&#214;", "Ö");
            }
            if (text.Contains("&#196;"))
            {
                text = text.Replace("&#196;", "Ä");
            }
            if (text.Contains("&#197;"))
            {
                text = text.Replace("&#197;", "Å");
            }
            if (text.Contains("&#233;"))
            {
                text = text.Replace("&#233;", "é");
            }
            if (text.Contains("&#176;"))
            {
                text = text.Replace("&#176;", "°");
            }
            if (text.Contains("&#224;"))
            {
                text = text.Replace("&#224;", "à");
            }

            return text;
        }

        public void SetRecipeMatches(Recipe recipe)
        {
            int numMatches = 0;
            var db = new DataBase();
            var list = new List<Ingredient>();
            list = db.GetIngredientsItems();

            
                foreach (var ingredient in recipe.Ingredients)
                {
                    foreach (var item in list)
                    {
                        if(ingredient.Name.ToLower().Contains(item.Name.ToLower()))
                        {
                            numMatches++;
                            recipe.NumIngredients = numMatches;
                            Debug.WriteLine(numMatches + " AV " + recipe.NumInRecipe);
                        }
                    }
                }
            }
           

        public List<Recipe> ListSorter(List<Recipe> recipes)
        {
            List<Recipe> list = new List<Recipe>();
            foreach (var item in recipes)
            {
                item.Percentage = (decimal)item.NumIngredients / (decimal) item.NumInRecipe;
                list.Add(item);

            }

            return list.OrderByDescending(x => x.Percentage).ThenBy(i => i.NumInRecipe).ToList();

        }
        public async void GetNumberOfPages()
        {
            httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(Url);

            var htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(html);


            var listOfPages = new List<HtmlNode>();
            listOfPages = htmldoc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("Pagination Pagination--personalization js-pagination u-paddingTxlg")).ToList();



            Match m;
            string HRefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";
            var listOfNumPages = new List<string>();

            try
            {
                foreach (var item in listOfPages)
                {
                    m = Regex.Match(item.InnerHtml, HRefPattern,
                                    RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                    TimeSpan.FromSeconds(1));
                    while (m.Success)
                    {

                        

                            //Debug.WriteLine("Found href " + m.Groups[1].Value + " Index: " + m.Groups[1].Index);
                        if (m.Groups[1].Value.StartsWith("https"))
                            {
                            listOfNumPages.Add(m.Groups[1].Value);
                            }
                        

                        m = m.NextMatch();
                    }
                }

                

            }
            catch (RegexMatchTimeoutException)
            {
                Console.WriteLine("The matching operation timed out.");
            }



            var twoNumList = new List<int>();
            var oneNumList = new List<int>();


            foreach (var item in listOfNumPages)
            {
                int parsednum;

                bool twoNums = int.TryParse(item.Substring(item.Length - 2, 2), out parsednum);
                if (twoNums == true)
                {
                    twoNumList.Add(parsednum);
                }

                bool oneNum = int.TryParse(item.Substring(item.Length - 1, 1), out parsednum);
                if (oneNum == true)
                {
                    oneNumList.Add(parsednum);
                }
            }

                if(twoNumList.Any())
                {

                     NumPages = twoNumList.Max();

                }
                else if(!twoNumList.Any() && oneNumList.Any())
                {
                     NumPages = oneNumList.Max();
                }
                else
                 {
                     NumPages = null;
                }

            
           
        }

        
    }
}
