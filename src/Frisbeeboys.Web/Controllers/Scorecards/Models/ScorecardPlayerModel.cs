using System.Text.Json.Serialization;

namespace Frisbeeboys.Web.Controllers.Scorecards.Models
{
    public class ScorecardPlayerModel
    {
        [JsonIgnore]
        public int ScorecardId { get; set; }
        public string Name { get; set; } = null!;
        public int Total { get; set; }
        public int Par { get; set; }
        public int[] HoleScores { get; set; } = null!;
    }
}