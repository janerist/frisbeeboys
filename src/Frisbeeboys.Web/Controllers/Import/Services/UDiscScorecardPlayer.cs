namespace Frisbeeboys.Web.Controllers.Import.Services
{
    public record UDiscScorecardPlayer(
        string Name, 
        int Total, 
        int Par, 
        int[] HoleScores
    );
}