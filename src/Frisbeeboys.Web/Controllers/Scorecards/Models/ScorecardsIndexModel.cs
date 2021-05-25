using System.Collections.Generic;
using System.Linq;
using Frisbeeboys.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frisbeeboys.Web.Controllers.Scorecards.Models
{
    public record ScorecardsIndexModel
    {
        public PageModel<ScorecardModel> Scorecards { get; }
        public IEnumerable<SelectListItem> Courses { get; init; }
        public IEnumerable<SelectListItem> Layouts { get; init; }
        public string? Course { get; init; }
        public string? Layout { get; init; }

        public ScorecardsIndexModel(PageModel<ScorecardModel> scorecards, ILookup<string, string> courses,
            string? course, string? layout)
        {
            Scorecards = scorecards;
            Courses = new[] {new SelectListItem("All courses", "", course == null)}
                .Concat(courses
                    .Select(c => new SelectListItem(c.Key, c.Key, c.Key == course))
                    .OrderBy(c => c.Value));
            Layouts = new[] {new SelectListItem("All layouts", "", layout == null)}
                .Concat(courses.Where(c => c.Key == course)
                    .SelectMany(c => c.Select(l => new SelectListItem(l, l, l == layout)))
                    .OrderBy(l => l.Value));
            Course = course;
            Layout = layout;
        }
    }
}