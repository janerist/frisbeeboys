using System.Threading.Tasks;
using Frisbeeboys.Web.Controllers.Scorecards.Models;
using Frisbeeboys.Web.Controllers.Scorecards.Services;
using Frisbeeboys.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Frisbeeboys.Web.Controllers.Scorecards
{
    public class ScorecardsController : Controller
    {
        private readonly ScorecardService _scorecardService;

        public ScorecardsController(ScorecardService scorecardService)
        {
            _scorecardService = scorecardService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            int page = 1, 
            int pageSize = 20,
            string? course = null,
            string? layout = null)
        {
            var (scorecards, total) = await _scorecardService.GetScorecards(page, pageSize, course, layout);
            return View(new PageModel<ScorecardModel>(scorecards, page, pageSize, total));
        }
    }
}