using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

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

        public List<Recipe> ListOfRecipes { get; set; }

        public RecipesScraper(string searchword)
        {
            Url = "https://www.coop.se/globalt-sok/?query=" + searchword;
            ListRecipeURL = new List<string>();
            ListOfRecipes = new List<Recipe>();
            Searchword = searchword;

        }

        
        public async void GetFirstPageRecipesURLsAsync()
        {
            
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


                                //getTitle

                                string title = innerhtmlH1.Substring(innerhtmlH1.IndexOf(">") + 1, IndexOfSecond(innerhtmlH1, "<") - (innerhtmlH1.IndexOf(">") + 1));
                                Debug.WriteLine("Titel: " + title);


                                //getDescription

                                string description = innerhtmlH1.Substring(innerhtmlH1.IndexOf("<p") + 47, innerhtmlH1.IndexOf("</p>") - (innerhtmlH1.IndexOf("<p") + 47));
                                Debug.WriteLine("Beskrivning: " + description);


                                //GetIngredienserToList

                                var ingredienthtml = htmldoc2.DocumentNode.Descendants("ul")
                                                            .Where(node => node.GetAttributeValue("class", "")
                                                            .Equals("List List--section")).ToList();


                                var ingredientList = ingredienthtml[0].Descendants("li")
                                                                    .Where(node => node.GetAttributeValue("class", "")
                                                                    .Equals("u-paddingHxsm u-textNormal u-colorBase")).ToList();

                                var ingrediensLista = new List<Ingredient>();

                                foreach (var ingredient in ingredientList)
                                {
                                    string ingredientInnerHtml = ingredient.InnerHtml;
                                    string ingredientName = ingredientInnerHtml.Substring(ingredientInnerHtml.IndexOf("<span class=") + 32, IndexOfSecond(ingredientInnerHtml, "</span>") - (ingredientInnerHtml.IndexOf("<span class=") + 32));
                                    string ingredientMeasure = ingredientInnerHtml.Substring(ingredientInnerHtml.IndexOf("<span>") + 6, ingredientInnerHtml.IndexOf("</span>") - (ingredientInnerHtml.IndexOf("<span>") + 6));

                                    ingrediensLista.Add(new Ingredient(ingredientName, ingredientMeasure));

                                    Debug.WriteLine("Ingrediens: " + ingredientName);


                                }

                                ListOfRecipes.Add(new Recipe(title, description, ingrediensLista, url));


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
