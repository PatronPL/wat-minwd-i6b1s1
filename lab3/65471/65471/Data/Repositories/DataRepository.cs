using System;
using System.Collections.Generic;
using System.Linq;
using _65471.Data.MemoriesDb;
using _65471.Data.Models;

namespace _65471.Data.Repositories
{
    public class DataRepository : IDataRepository
    {
        private readonly MemoryDb _memoryDb;

        public DataRepository(MemoryDb memoryDb)
        {
            _memoryDb = memoryDb;
        }

        //public IEnumerable<DataDto> Get(int line)
           // => _memoryDb.DataDtoList.Where(x => x.Line == line).ToList();

       public IEnumerable<DataDto> Get()
            => _memoryDb.DataDtoList.ToList();

        public IEnumerable<int> GetSet()
            => _memoryDb.LineNumbers.ToList();

        public DataDto Get(int line, string brigade)
            => _memoryDb.DataDtoList.FirstOrDefault(x => x.Line == line && x.Brigade == brigade);

        public void Add(IEnumerable<DataDto> dataDtoList)
            => _memoryDb.DataDtoList.AddRange(dataDtoList);

        public void Add(IEnumerable<int> lineNumbers)
        {
            foreach (var lineNumber in lineNumbers)
            {
                _memoryDb.LineNumbers.Add(lineNumber);
            }
        }

        public void Clear()
            => _memoryDb.DataDtoList.Clear();

        public void ClearSet()
            => _memoryDb.LineNumbers.Clear();
    }
}