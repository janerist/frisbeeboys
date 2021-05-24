using System;

namespace Frisbeeboys.Web.Controllers.Scorecards.Models
{
    public class ScorecardModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = null!;
        public string LayoutName { get; set; } = null!;
        public DateTime Date { get; set; }
        public int[] HolePars { get; set; } = null!;
        public ScorecardPlayerModel[] Players { get; set; } = null!;
    }
}