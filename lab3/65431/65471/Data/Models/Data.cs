using System;
using Newtonsoft.Json;

namespace _65471.Data.Models
{
    public class Data
    {
        [JsonProperty("Lat")]
        public double Latitude { get; set; }
        [JsonProperty("Lon")]
        public double Longitude { get; set; }
        [JsonProperty("Time")]
        public DateTime Time { get; set; }
        [JsonProperty("Lines")]
        public int Line { get; set; }
        [JsonProperty("Brigade")]
        public string Brigade { get; set; }
    }
}