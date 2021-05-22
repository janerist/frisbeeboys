using System.Threading.Tasks;
using Frisbeeboys.Api.Api.Scorecards.Models;
using Frisbeeboys.Api.Api.Scorecards.Services;
using Frisbeeboys.Api.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frisbeeboys.Api.Api.Scorecards
{
    [ApiController]
    public class ScorecardsController : ControllerBase
    {
        private readonly ScorecardService _scorecardService;

        public ScorecardsController(ScorecardService scorecardService)
        {
            _scorecardService = scorecardService;
        }

        [HttpGet("/scorecards")]
        public async Task<PagedResponse<ScorecardResponse>> GetScorecards(int page = 1, int pageSize = 20,
            string? course = null, string? layout = null)
        {
            var (scorecards, total) = await _scorecardService.GetScorecards(page, pageSize, course, layout);
            return new PagedResponse<ScorecardResponse>(scorecards, page, pageSize, total);
        }
    }
}