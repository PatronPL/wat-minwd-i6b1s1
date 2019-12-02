using HtmlAgilityPack;
using Nancy.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ScraperOLX
{
    class Program
    {

        public static string getImageFromUrl(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            byte[] b = null;

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();

            if (request.HaveResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    using (BinaryReader br = new BinaryReader(receiveStream))
                    {
                        b = br.ReadBytes(500000);
                        br.Close();
                    }
                }
            }

            string base64String = Convert.ToBase64String(b, 0, b.Length);

            return base64String;
        }

        static void Main(string[] args)
        {     

            IWebDriver driver = new ChromeDriver();

            // Navigate to a page      
            driver.Navigate().GoToUrl(@"https://www.olx.pl/oferty/");
                   
            var html = driver.PageSource;

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);


            //Getting list of offers
            var ProductHtml = htmlDocument.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("offers_table")).ToList();


            // Getting names of products
            var ProductNames = ProductHtml[0].Descendants("a")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("marginright5 link linkWithHash detailsLink")).ToList();

            // Getting prices of products
            var ProductPrices = ProductHtml[0].Descendants("p")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("price")).ToList();

            //Getting location and time
            var ProductPlace = ProductHtml[0].Descendants("td")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("bottom-cell")).ToList();

            //Getting category
            var ProductCategory = ProductHtml[0].Descendants("p")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("color-9 lheight16 margintop5")).ToList();



            //Getting links to images
            var imgsrc = ProductHtml[0].SelectNodes("//img[@class='fleft']");         

            string[] imagesBase64 = new string[imgsrc.Count()];

            int j = 0;
            foreach (var elements in imgsrc)
            {
               
                imagesBase64[j] = imgsrc[j].Attributes["src"].Value;                
         
                j++;
            }


            static string getPlace(string s)
            {
                string[] result = s.Split("\t");

                string miejsce = result[0];

                return miejsce;

            }

            static string getTime(string s)
            {
                string[] result = s.Split("\t");

                string czas = result[1];

                return czas;
            }


            int i = 0;

            List<ScraperItem> items = new List<ScraperItem>();

            foreach (var element in ProductNames)
            {
                try
                {

                    var name = ProductNames[i].InnerText.Trim();
                    var category = ProductCategory[i].InnerText.Trim();
                    var price = ProductPrices[i].InnerText.Trim();
                    var place = getPlace(
                                                   ProductPlace[i].InnerText.Replace("  ", string.Empty)
                                                  .Replace("\r\n\r\n\r\n\r\n\r\n\t\t\t\t\t\t\t\t\t\t", string.Empty)
                                                  .Replace("\r\n\r\n\r\n\r\n\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t", string.Empty)
                                                  .Replace("\t\t\t\t\t\t\t\t\t\r\n\r\n\r\n\r\n\r\n", string.Empty));

                    var time = getTime(
                                                   ProductPlace[i].InnerText.Replace("  ", string.Empty)
                                                  .Replace("\r\n\r\n\r\n\r\n\r\n\t\t\t\t\t\t\t\t\t\t", string.Empty)
                                                  .Replace("\r\n\r\n\r\n\r\n\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t", string.Empty)
                                                  .Replace("\t\t\t\t\t\t\t\t\t\r\n\r\n\r\n\r\n\r\n", string.Empty));

                    var imgString = getImageFromUrl(imagesBase64[i]);


                    items.Add(new ScraperItem(name, category, price, place, time, imgString));
                    
                    ++i;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Błąd przy pobieraniu HTML!");
                }

            }
               

            var json = new JavaScriptSerializer().Serialize(items);
            Console.WriteLine(json);

           // File.WriteAllText(@"C:\Users\kubry\Desktop\ScraperOLX", json);


            Console.ReadKey();

        }
    }
}
