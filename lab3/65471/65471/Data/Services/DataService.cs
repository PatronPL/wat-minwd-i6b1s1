using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using _65471.Data.Extensions;
using _65471.Data.Mappers;
using _65471.Data.Models;
using _65471.Data.Options;
using _65471.Data.Repositories;
using Microsoft.Extensions.Options;

namespace _65471.Data.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IDataRepository _dataRepository;
        private readonly IOptions<ApiOptions> _apiOptions;

        public DataService(HttpClient httpClient, IDataRepository dataRepository, IOptions<ApiOptions> apiOptions)
        {
            _httpClient = httpClient;
            _dataRepository = dataRepository;
            _apiOptions = apiOptions;

        }

        public async Task<IEnumerable<DataDto>> GetAllAsync()
        {
            _dataRepository.Clear();
            _dataRepository.ClearSet();
            
            var response = await _httpClient.GetAsync(
                $"https://api.um.warszawa.pl/api/action/busestrams_get/?resource_id=f2e5503e-927d-4ad3-9500-4ab9e55deb59&apikey={_apiOptions.Value.Key}&type={_apiOptions.Value.Type}");
            
            var myData = ConvertExtension.JsonConverterExtension(await ConvertExtension.GetDataFromUrlAsString(response));
            var returnValues = myData.Select(Mapper.MapDataToDto).ToList();
            _dataRepository.Add(returnValues);
            
            return returnValues;
        }

        public async Task<IEnumerable<DataDto>> GetByLine(int line)
        {
            var response = await _httpClient.GetAsync(
                $"https://api.um.warszawa.pl/api/action/busestrams_get/?resource_id=f2e5503e-927d-4ad3-9500-4ab9e55deb59&apikey={_apiOptions.Value.Key}&type={_apiOptions.Value.Type}&line={line}");
            
            var myData = ConvertExtension.JsonConverterExtension(await ConvertExtension.GetDataFromUrlAsString(response));
            var returnValues = myData.Select(Mapper.MapDataToDto).ToList();
            
            return returnValues;
        }

        public IEnumerable<int> GetLineNumbers()
        {
            var data = _dataRepository.Get();
            _dataRepository.Add(data.Select(x => x.Line).ToList());

            return _dataRepository.GetSet();
        }

        public async Task<DataDto> GetAsync(int line, string brigade)
        {
            var data = _dataRepository.Get(line, brigade);
            
            var response = await _httpClient.GetAsync(
                $"https://api.um.warszawa.pl/api/action/busestrams_get/?resource_id=f2e5503e-927d-4ad3-9500-4ab9e55deb59&apikey={_apiOptions.Value.Key}&type={_apiOptions.Value.Type}&line={data.Line}&brigade={data.Brigade}");
            
            var myData = ConvertExtension.JsonConverterExtension(await ConvertExtension.GetDataFromUrlAsString(response));
            var returnValue = myData.Select(Mapper.MapDataToDto).FirstOrDefault();

            return returnValue;
        }
    }
}