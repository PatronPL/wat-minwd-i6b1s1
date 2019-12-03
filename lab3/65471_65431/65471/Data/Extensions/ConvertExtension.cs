using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace _65471.Data.Extensions
{
    public static class ConvertExtension
    {
        public static async Task<string> GetDataFromUrlAsString(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static IEnumerable<Models.Data> JsonConverterExtension(string resp)
        {
            var tmpJsonObj = JObject.Parse(resp);
            var tmpJsonArray = (JArray)tmpJsonObj["result"];
            return tmpJsonArray.ToObject<IList<Models.Data>>();
        }
    }
}