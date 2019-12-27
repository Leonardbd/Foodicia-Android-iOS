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
    }
}
