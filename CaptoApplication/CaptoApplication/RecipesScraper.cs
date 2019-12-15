using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace CaptoApplication
{

    
    public class RecipesScraper
    {
        public string Url { get; set; }
        HttpClient httpclient { get; set; }

        public RecipesScraper(string searchword)
        {
            Url = "https://www.coop.se/globalt-sok/?query=" + searchword;

            

        }

        public async void GetHtmlAsync()
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

            var finalList2 = new List<HtmlNode>();
            finalList2 = finalList.Where(node => node.GetAttributeValue("class", "")
                                        .Equals("RecipeTeaser-content")).ToList();


            var finalList3 = new List<string>();

            finalList3.Add(finalList2[1].SelectSingleNode("//a").Attributes["href"].Value.ToString());

            // ReceptLista = htmldoc.DocumentNode.SelectNodes("//article").ToList();
        }

    }
}
