using System;

namespace Frisbeeboys.Api.Data
{
    [TableName("scorecards")]
    public class Scorecard
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = default!;
        public string LayoutName { get; set; } = default!;
        public DateTime Date { get; set; }
        public int[] HolePars { get; set; } = default!;
    }
}