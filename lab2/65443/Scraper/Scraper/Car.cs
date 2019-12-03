using System;
using System.Collections.Generic;
using System.Text;

namespace Scraper
{
    class Car
    {

        public string Name { get; set; }
        public string Year { get; set; }
        public string Mileage { get; set; }
        public string Capacity { get; set; }
        public string Fuel { get; set; }
        public string Price { get; set; }
        public string ImageString { get; set; }

        public Car(string name, string year, string mileage, string capacity, string fuel, string price)
        {
            Name = name;
            Year = year;
            Mileage = mileage;
            Capacity = capacity;
            Fuel = fuel;
            Price = price;
            
        }
    }
}
