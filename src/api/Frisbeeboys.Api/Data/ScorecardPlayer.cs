namespace Frisbeeboys.Api.Data
{
    [TableName("scorecard_players")]
    public class ScorecardPlayer
    {
        public int ScorecardId { get; set; }
        public string Name { get; set; } = default!;
        public int Total { get; set; }
        public int Par { get; set; }
        public int[] HoleScores { get; set; } = default!;
    }
}