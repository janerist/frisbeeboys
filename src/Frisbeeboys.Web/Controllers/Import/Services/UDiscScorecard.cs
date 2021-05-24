using System;
using System.Collections.Generic;

namespace Frisbeeboys.Web.Controllers.Import.Services
{
    public record UDiscScorecard(
        string CourseName, 
        string LayoutName, 
        DateTime Date, 
        int[] HolePars,
        IList<UDiscScorecardPlayer> Players
    );
}