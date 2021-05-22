using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frisbeeboys.Api.Api.Scorecards.Models;
using Frisbeeboys.Api.Data;

namespace Frisbeeboys.Api.Api.Scorecards.Services
{
    public class ScorecardService
    {
        private readonly ScorecardDatabase _database;

        public ScorecardService(ScorecardDatabase database)
        {
            _database = database;
        }

        public async Task<(IList<ScorecardResponse>, int)> GetScorecards(int page, int pageSize, string? course, string? layout)
        {
            var (scorecards, totalCount) = await _database.Query<ScorecardResponse>()
                .Select("s.*")
                .From("scorecards s")
                .Filter("s.course_name = :Course", new {Course = course}, onlyIf: course != null)
                .Filter("s.layout_name = :Layout", new {Layout = layout}, onlyIf: layout != null)
                .OrderBy("s.date desc")
                .PageAndCountAsync(page, pageSize);

            var players = (await _database.Query<ScorecardPlayerResponse>()
                    .Select("sp.*")
                    .From("scorecard_players sp")
                    .Filter("sp.scorecard_id = ANY(:ScorecardIds)",
                        new {ScorecardIds = scorecards.Select(s => s.Id).ToArray()})
                    .AllAsync())
                .ToLookup(sp => sp.ScorecardId);

            foreach (var scorecard in scorecards)
            {
                scorecard.Players = players[scorecard.Id].ToArray();
            }

            return (scorecards, totalCount);
        }
    }
}