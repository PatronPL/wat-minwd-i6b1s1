using System;
using System.Collections.Generic;
using _65471.Data.Models;

namespace _65471.Data.Repositories
{
    public interface IDataRepository
    {
        DataDto Get(int line, string brigade);
        IEnumerable<DataDto> Get();
        IEnumerable<int> GetSet();
        //IEnumerable<DataDto> Get(int line);
        void Add(IEnumerable<DataDto> dataDtoList);
        void Add(IEnumerable<int> lineNumbers);
        void Clear();
        void ClearSet();
    }
}