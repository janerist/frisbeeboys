using System;

namespace Frisbeeboys.Api.Api.Scorecards.Models
{
    public class ScorecardPlayerResponse
    {
        public int ScorecardId { get; set; }
        public string Name { get; set; } = null!;
        public int Total { get; set; }
        public int Par { get; set; }
        public int[] HoleScores { get; set; } = null!;
    }

    public class ScorecardResponse
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = null!;
        public string LayoutName { get; set; } = null!;
        public DateTime Date { get; set; }
        public int[] HolePars { get; set; } = null!;
        public ScorecardPlayerResponse[] Players { get; set; } = null!;
    }
}