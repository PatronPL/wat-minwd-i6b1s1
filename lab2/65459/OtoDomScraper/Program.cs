using System;
using System.Net.Http;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;

namespace OtoDomScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("C# Scraper OTODOM by Damian Tomasik");
            GetHtmlAsync();
            Console.ReadLine();
        }
        static async void GetHtmlAsync()
        {
            var url = "https://www.otodom.pl/sprzedaz/mieszkanie/warszawa/?search%5Bregion_id%5D=7&search%5Bsubregion_id%5D=197&search%5Bcity_id%5D=26&nrAdsPerPage=72";
            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var ProductsHtml = htmlDocument.DocumentNode.Descendants("div").
                Where(node => node.GetAttributeValue("class", "").Equals("listing")).ToList();

            var ProductListItems = new List<HtmlNode>();
            try
            {
                ProductListItems = ProductsHtml[0].Descendants("article")
                .Where(node => node.GetAttributeValue("data-item-id", "") != null).ToList();
            }

            catch (Exception e)
            {
                Console.WriteLine("No ni działa");
            }
            Console.WriteLine();
            Console.WriteLine("Liczba ofert: " + ProductListItems.Count());
            Console.WriteLine("Wyniki:");
            Console.WriteLine();

            foreach (var ProductListItem in ProductListItems)
            {
                Console.WriteLine();
                //ID
                Console.WriteLine("ID: " + ProductListItem.GetAttributeValue("data-item-id", ""));
                //Nazwa
                Console.WriteLine(ProductListItem.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("offer-item-title")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));
                //Mieszkanie/Dom + Kupno/Sprzedaż + Dzielnica
                Console.WriteLine(ProductListItem.Descendants("p")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("text-nowrap")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    );
                // Metraż
                Console.WriteLine(ProductListItem.Descendants("li")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("hidden-xs offer-item-area")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));
                //Koszt    
                Console.WriteLine(ProductListItem.Descendants("li")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("hidden-xs offer-item-price-per-m")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    );
                Console.WriteLine(ProductListItem.Descendants("li")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("offer-item-price")).FirstOrDefault().InnerText.Trim(' ', '\r', '\n', '\t')
                    );
                //URL
                Console.WriteLine("URL OFERTY:   " +
                    ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "")
                    );
                //URL zdjęcia
                Console.WriteLine("URL zdjęcia:   " + ProductListItem.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("img-cover lazy")).FirstOrDefault().GetAttributeValue("data-src", "")
                    );

                Console.WriteLine();
            }
        }
    }
}
