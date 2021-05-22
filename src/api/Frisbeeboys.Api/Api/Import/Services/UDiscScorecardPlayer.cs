namespace Frisbeeboys.Api.Api.Import.Services
{
    public record UDiscScorecardPlayer(
        string Name, 
        int Total, 
        int Par, 
        int[] HoleScores
    );
}