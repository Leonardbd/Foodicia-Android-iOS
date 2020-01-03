using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CaptoApplication
{
    public static class BarCodeManager

    {
        private static List<string> bannedWords = new List<string> {"se", "eko", "ekologisk", 
            "arla", "krav", "strimlad", "skiva", "tärnad", ".", "kryddad", "med", "uht", "/",
            "msc", "uts", "utz", "riven", "styckad", "färsk", "scan", "orginal", "original", "&"};

        public static string getBarNameDabas(string ean)
        {
            string url = "https://api.dabas.com/DABASService/V2/article/gtin/0" + ean + "/XML?apikey=9ea3b61d-c200-4316-9b29-5764e7b206e7";
            try
            {
                XNamespace ns = "http://schemas.datacontract.org/2004/07/DABAS.ViewModels.Api";

                var xml = XDocument.Load(url);
                var node = xml.Descendants(ns + "Artikelbenamning").ToList();
                var name = node[0].Value;

                if (!name.Equals(""))
                {
                    return name;
                }
                else
                {
                    return getBarName(ean);
                }
                
            }
            catch (Exception)
            {
                return getBarName(ean);
                
            }
        }

        public static string getBarName(string ean)
         {
            string url = "http://api.ean-search.org/api?token=Medieteknikor2020&op=barcode-lookup&ean="+ean;
            try
            {
                
                XmlDocument document = new XmlDocument();
                document.Load(url);
                XmlNode node = document.DocumentElement.SelectSingleNode("/BarcodeLookupResponse/product/name");
                string text = node.InnerText;
                return text;

            }
            catch (Exception)
            {
                return getBarName2(ean);
                

            }
        }

        public static string getBarName2(string ean)
        {

            string url = "https://eandata.com/feed/?v=3&keycode=869919BB5E0DF166&mode=xml&find=" + ean;
            try
            {

                XmlDocument document = new XmlDocument();
                document.Load(url);
                XmlNode node = document.DocumentElement.SelectSingleNode("/feed/product/attributes/product");
                string text = node.InnerText;
                return text;

            }
            catch (Exception)
            {
                return "Sugondese";

            }

        }

        public static string getCorrectName(string productname)
        {
            string finalname = productname.ToLower();

            var list = finalname.Split().ToList();

            string thisIsFinal = "";

            for (int i = 0; i < list.Count; i++)
            {
                foreach (string word in bannedWords)
                {
                    if (list[i].Contains(word))
                    {
                        if (word.Equals("se") && list[i].Length > 2)
                        {
                            continue;
                        }
                        list[i] = "";
                    }
                }

                if (list[i].Any(char.IsDigit))
                {
                    break;
                }
                else
                {
                    if (!thisIsFinal.Equals(""))
                    {
                        thisIsFinal += " " + list[i];
                    }
                    else
                    {
                        thisIsFinal = list[i];
                    }
                }
            }

            thisIsFinal = thisIsFinal.Replace("  ", " ").Replace("  ", " ").Replace(",", "").Replace("-", "").Replace("®️", "").Replace("©", "");

            if (thisIsFinal.Contains("mjölk") && !thisIsFinal.Contains("kokosmjölk") && !thisIsFinal.Contains("chokladmjölk"))
            {
                thisIsFinal = "Mjölk";
            }

            thisIsFinal = char.ToUpper(thisIsFinal[0]) + thisIsFinal.Substring(1);

            return thisIsFinal;
        }
    }
}
