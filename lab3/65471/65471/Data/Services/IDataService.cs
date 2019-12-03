using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _65471.Data.Models;

namespace _65471.Data.Services
{
    public interface IDataService
    {
        Task<IEnumerable<DataDto>> GetAllAsync();
        Task<IEnumerable<DataDto>> GetByLine(int line);
        IEnumerable<int> GetLineNumbers();
        Task<DataDto> GetAsync(int line, string brigade);
    }
}