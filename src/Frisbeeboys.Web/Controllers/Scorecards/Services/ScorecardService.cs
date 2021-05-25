using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frisbeeboys.Web.Controllers.Scorecards.Models;
using Frisbeeboys.Web.Data;

namespace Frisbeeboys.Web.Controllers.Scorecards.Services
{
    public class ScorecardService
    {
        private readonly ScorecardDatabase _database;

        public ScorecardService(ScorecardDatabase database)
        {
            _database = database;
        }

        public async Task<(IList<ScorecardModel>, int)> GetScorecards(int page, int pageSize, string? course, string? layout)
        {
            var (scorecards, totalCount) = await _database.Query<ScorecardModel>()
                .Select("s.*")
                .From("scorecards s")
                .Filter("s.course_name = :Course", new {Course = course}, onlyIf: course != null)
                .Filter("s.layout_name = :Layout", new {Layout = layout}, onlyIf: layout != null)
                .OrderBy("s.date desc")
                .PageAndCountAsync(page, pageSize);

            var players = (await _database.Query<ScorecardPlayerModel>()
                    .Select("sp.*")
                    .From("scorecard_players sp")
                    .Filter("sp.scorecard_id = ANY(:ScorecardIds)",
                        new {ScorecardIds = scorecards.Select(s => s.Id).ToArray()})
                    .AllAsync())
                .ToLookup(sp => sp.ScorecardId);

            foreach (var scorecard in scorecards)
            {
                scorecard.Players = players[scorecard.Id].OrderBy(p => p.Total).ToArray();
            }

            return (scorecards, totalCount);
        }

        public async Task<ILookup<string, string>> GetCourses()
        {
            var courses = await _database.Query<(string CourseName, string LayoutName)>()
                .SelectDistinct("s.course_name, s.layout_name")
                .From("scorecards s")
                .AllAsync();

            return courses.ToLookup(s => s.CourseName, s => s.LayoutName);
        }
    }
}