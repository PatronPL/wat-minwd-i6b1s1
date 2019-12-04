using System;
using System.Collections.Generic;
using System.Text;

namespace ScraperOLX
{
    class ScraperItem
    {

        public string Name { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }
        public string Place { get; set; }
        public string Time { get; set; }
        public string ImageString { get; set; }

        public ScraperItem(string name, string category, string price, string place, string time, string imageString)
        {
            Name = name;
            Category = category;
            Price = price;
            Place = place;
            Time = time;
            ImageString = imageString;
        }
    }
}
