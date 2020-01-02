using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CaptoApplication
{
    public static class BarCodeManager
    {

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
                return null;

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
                return null;

            }

        }

        public static string getCorrectName(string productname)
        {
            string finalname = productname;

            if (productname.Contains("0") ||
               productname.Contains("1") ||
               productname.Contains("2") ||
               productname.Contains("3") ||
               productname.Contains("4") ||
               productname.Contains("5") ||
               productname.Contains("6") ||
               productname.Contains("7") ||
               productname.Contains("8") ||
               productname.Contains("9") ||
               productname.ToLower().Contains("se") ||
               productname.ToLower().Contains("eko") ||
               productname.ToLower().Contains("ekologisk") ||
                productname.ToLower().Contains("krav"))

            {
                finalname = "";
                var list = productname.ToLower().Split(" ").ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Contains("0") ||
                       list[i].Contains("1") ||
                       list[i].Contains("2") ||
                       list[i].Contains("3") ||
                       list[i].Contains("4") ||
                       list[i].Contains("5") ||
                       list[i].Contains("6") ||
                       list[i].Contains("7") ||
                       list[i].Contains("8") ||
                       list[i].Contains("9") ||
                       list[i].Contains("se") ||
                       list[i].Contains("eko") ||
                       list[i].Contains("ekologisk") ||
                       list[i].Contains("krav"))
                    {
                        bool result = list[i].Any(x => char.IsLetter(x));
                        bool result2 = list[i].Contains("%");
                        if (!result && !result2)
                        {
                            i++;
                        }
                    }
                    else
                    {
                        if (!finalname.Equals(""))
                        {
                            finalname += " " + list[i];
                        }
                        else
                        {
                            finalname = list[i];
                        }
                    }


                }

            }

            return finalname;

        }
    }
}
