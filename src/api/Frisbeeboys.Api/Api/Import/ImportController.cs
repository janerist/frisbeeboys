using System.Threading.Tasks;
using Frisbeeboys.Api.Api.Import.Models;
using Frisbeeboys.Api.Api.Import.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Frisbeeboys.Api.Api.Import
{
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly ImportService _importService;

        public ImportController(ImportService importService)
        {
            _importService = importService;
        }

        [HttpPost("/import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            try
            {
                var count = await _importService.ImportAsync(file);
                return Ok(new ImportResponse(count));
            }
            catch (UDiscCsvParserException)
            {
                return BadRequest(new {message = "Not valid UDisc CSV export format"});
            }
        }
    }
}