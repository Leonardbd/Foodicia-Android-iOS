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
        public string Url { get; set; }
        public HttpClient httpclient { get; set; }

        public string Searchword { get; set; }
        public int? NumPages { get; set; }
        public List<string> ListRecipeURL { get; set; }

        public RecipesScraper(string searchword)
        {
            Url = "https://www.coop.se/globalt-sok/?query=" + searchword;
            ListRecipeURL = new List<string>();
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

                                ListRecipeURL.Add("coop.se" + m.Groups[1].Value);
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
