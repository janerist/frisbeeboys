using System;
using System.Threading.Tasks;
using Frisbeeboys.Web.Controllers.Import.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frisbeeboys.Web.Controllers.Import
{
    public class ImportController : Controller
    {
        private readonly ImportService _importService;

        public ImportController(ImportService importService)
        {
            _importService = importService;
        }

        [HttpGet("/import")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/importfile")]
        public async Task<IActionResult> ImportFile(IFormFile file)
        {
            if (file == null)
            {
                TempData["error"] = "Please select a file.";
                return RedirectToAction("Index");
            }
            
            return await Import(() => _importService.ImportAsync(file));
        }

        [HttpPost("/importtext")]
        public async Task<IActionResult> ImportText(string csv)
        {
            if (csv == null)
            {
                TempData["error"] = "Please paste some text.";
                return RedirectToAction("Index");
            }
            return await Import(() => _importService.ImportAsync(csv));
        }

        private async Task<IActionResult> Import(Func<Task<int>> import)
        {
            try
            {
                var count = await import();
                if (count > 0)
                {
                    TempData["success"] = $"Successfully imported {count} new scorecard(s).";
                }
                else
                {
                    TempData["success"] = "Success, but these scorecards have already been imported.";
                }
            }
            catch (UDiscCsvParserException)
            {
                TempData["error"] = "Not valid UDisc CSV format.";
            }
            
            return RedirectToAction("Index");
        }
    }
}