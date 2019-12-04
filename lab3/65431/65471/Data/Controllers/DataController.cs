using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _65471.Data.Models;
using _65471.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace _65471.Data.Controllers
{
    [Route("[controller]")]
    public class DataController : Controller
    {
        private readonly IDataService _dataService;
        
        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataDto>>> Get()
            => Json(await _dataService.GetAllAsync());

        [HttpGet("{line}&{brigade}")]
        public async Task<ActionResult<DataDto>> Get(int line, string brigade)
            => Json(await _dataService.GetAsync(line, brigade));
        
        [HttpGet("line/{line}")]
        public async Task<ActionResult<IEnumerable<DataDto>>> Get(int line)
            => Json( await _dataService.GetByLine(line));
        
        [HttpGet("line")]
        public ActionResult<IEnumerable<DataDto>> GetLine()
            => Json( _dataService.GetLineNumbers());
    }
}