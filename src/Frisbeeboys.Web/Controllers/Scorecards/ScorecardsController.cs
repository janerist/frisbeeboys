using System.Linq;
using System.Threading.Tasks;
using Frisbeeboys.Web.Controllers.Scorecards.Models;
using Frisbeeboys.Web.Controllers.Scorecards.Services;
using Frisbeeboys.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var courses = await _scorecardService.GetCourses();

            course = course != null && courses.Contains(course) ? course : null;
            layout = layout != null && course != null && courses[course].Contains(layout) ? layout : null;
            
            var (scorecards, total) = await _scorecardService.GetScorecards(page, pageSize, course, layout);

            var model = new ScorecardsIndexModel(new PageModel<ScorecardModel>(scorecards, page, pageSize, total),
                courses,
                course,
                layout);
            
            return View(model);
        }
    }

    
}