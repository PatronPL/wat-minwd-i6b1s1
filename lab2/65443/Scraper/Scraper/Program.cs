using HtmlAgilityPack;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Scraper
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

            GetHtmlAsync();
            Console.ReadKey();
            
        }

        private static async void GetHtmlAsync() {

            var url = "https://www.otomoto.pl/osobowe/dacia/?search%5Border%5D=created_at%3Adesc&search%5Bbrand_program_id%5D%5B0%5D=&search%5Bcountry%5D=";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
                      
           
            //Getting list of offers
            var ProductHtml = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("offers list")).ToList();

            // Getting names of products
            var ProductNames = ProductHtml[0].Descendants("a")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("offer-title__link")).ToList();

            // Getting Age of products
            var ProductYear = ProductHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("data-code", "")
                .Equals("year")).ToList();

            // Getting Mileages of products
            var ProductMileage = ProductHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("data-code", "")
                .Equals("mileage")).ToList();

            // Getting Capacity of products
            var ProductCapacity = ProductHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("data-code", "")
                .Equals("engine_capacity")).ToList();

            // Getting Fuel_Type of products
            var ProductFuel = ProductHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("data-code", "")
                .Equals("fuel_type")).ToList();

            // Getting Prices of products
            var ProductPrice = ProductHtml[0].Descendants("span")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("offer-price__number ds-price-number")).ToList();

            // Getting list of images
            var imgsrc = ProductHtml[0].SelectNodes("//img[@class='lazyload']");

            string[] imagesBase64 = new string[imgsrc.Count()];


            int j = 0;
            foreach (var elements in imgsrc)
            {
           
                imagesBase64[j] = imgsrc[j].Attributes["data-src"].Value;
           
                j++;
            }



            int i = 0;

            Console.WriteLine();

            List<Car> cars = new List<Car>();

            foreach (var element in ProductNames) {

                try
                {

                    cars.Add(new Car(ProductNames[i].InnerText.Trim(), ProductYear[i].InnerText.Trim(), ProductMileage[i].InnerText.Trim()
                        , ProductCapacity[i].InnerText.Trim(), ProductFuel[i].InnerText.Trim(), 
                        ProductPrice[i].InnerText.Trim().Replace(" ", string.Empty).Replace("PLN", string.Empty)));

                  //  Console.WriteLine("Nazwa: " + ProductNames[i].InnerText.Trim());
                  //  Console.WriteLine("Rok Produkcji: " + ProductYear[i].InnerText.Trim());
                  //  Console.WriteLine("Przebieg: " + ProductMileage[i].InnerText.Trim());
                  //  Console.WriteLine("Pojemność silnika: " + ProductCapacity[i].InnerText.Trim());
                  //  Console.WriteLine("Rodzaj Paliwa: " + ProductFuel[i].InnerText.Trim());
                  //  Console.WriteLine("Cena: " + ProductPrice[i].InnerText.Trim().Replace(" ", string.Empty).Replace("PLN", string.Empty));
                  //  Console.WriteLine("Base64String: " + getImageFromUrl(imagesBase64[i]));
                  //  Console.WriteLine();
                    ++i;

                }
                catch (Exception e)
                {
                    Console.WriteLine("Błąd przy wczytywaniu HTML!");
                }
                
               
                            
            }

            var json = new JavaScriptSerializer().Serialize(cars);
            Console.WriteLine(json);

            Console.WriteLine();
        }
    }
}
