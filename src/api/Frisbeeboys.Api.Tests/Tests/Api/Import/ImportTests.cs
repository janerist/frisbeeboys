using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Frisbeeboys.Api.Data;
using Frisbeeboys.Api.Tests.Helpers;
using Xunit;

namespace Frisbeeboys.Api.Tests.Tests.Api.Import
{
    [Collection("Api Tests")]
    public class ImportTests : ApiTestBase
    {
        public ImportTests(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task ValidatesCsv()
        {
            var scorecardCsv = "Nonsense that should no parse correctly";

            var response = await ImportAsync(scorecardCsv);
            await response.AssertBadRequest("Not valid UDisc CSV export format");
        }

        [Fact]
        public async Task NewScorecardsAreStoredCorrectly()
        {
            var scorecardCsv =
                "PlayerName,CourseName,LayoutName,Date,Total,+/-,Hole1,Hole2,Hole3,Hole4,Hole5,Hole6,Hole7,Hole8,Hole9,Hole10,Hole11,Hole12,Hole13,Hole14,Hole15,Hole16,Hole17,Hole18\n" +
                "Par,Rotvoll Diskgolfpark,Good tees,2021-05-14 18:27,34,,3,3,3,3,3,3,4,3,3,3,3,,,,,,,\n" +
                "Jan-Erik Strøm,Rotvoll Diskgolfpark,Good tees,2021-05-14 18:27,42,8,4,3,4,3,3,3,4,4,3,6,5,,,,,,,\n" +
                "Espen,Rotvoll Diskgolfpark,Good tees,2021-05-14 18:27,38,4,4,3,3,4,3,2,6,3,4,4,2,,,,,,,";

            var response = await ImportAsync(scorecardCsv);
            
            await response.AssertOk();

            var scorecard = await Database.QuerySingleAsync<Scorecard>("select * from scorecards");
            var scorecardPlayers = await Database.QueryAsync<ScorecardPlayer>("select * from scorecard_players");
            
            Assert.Equal("Rotvoll Diskgolfpark", scorecard.CourseName);
            Assert.Equal("Good tees", scorecard.LayoutName);
            Assert.Equal("2021-05-14 18:27", scorecard.Date.ToString("yyyy-MM-dd HH:mm"));
            
            Assert.Equal(2, scorecardPlayers.Count);
            
            Assert.Equal("Jan-Erik Strøm", scorecardPlayers[0].Name);
            Assert.Equal(42, scorecardPlayers[0].Total);
            Assert.Equal(8, scorecardPlayers[0].Par);
            
            Assert.Equal("Espen", scorecardPlayers[1].Name);
            Assert.Equal(38, scorecardPlayers[1].Total);
            Assert.Equal(4, scorecardPlayers[1].Par);
        }

        [Fact]
        public async Task DuplicateScorecardsAreIgnored()
        {
            var scorecardCsv =
                "PlayerName,CourseName,LayoutName,Date,Total,+/-,Hole1,Hole2,Hole3,Hole4,Hole5,Hole6,Hole7,Hole8,Hole9,Hole10,Hole11,Hole12,Hole13,Hole14,Hole15,Hole16,Hole17,Hole18\n" +
                "Par,Dragvoll Diskgolfarena,Dragvoll 2021,2021-03-09 16:36,56,,3,4,3,3,3,3,3,3,4,3,3,3,3,3,3,3,3,3\n" +
                "Jan-Erik Strøm,Dragvoll Diskgolfarena,Dragvoll 2021,2021-03-09 16:36,72,16,3,7,3,3,5,3,4,5,3,3,6,3,3,5,4,5,4,3\n" +
                "Espen,Dragvoll Diskgolfarena,Dragvoll 2021,2021-03-09 16:36,64,8,3,5,3,2,4,3,4,6,3,3,5,3,3,3,4,3,4,3";

            var response = await ImportAsync(scorecardCsv);
            await response.AssertOk();
            
            response = await ImportAsync(scorecardCsv);
            await response.AssertOk();
            
            var scorecards = await Database.QueryAsync<Scorecard>("select * from scorecards");
            var scorecardPlayers = await Database.QueryAsync<ScorecardPlayer>("select * from scorecard_players");
            
            Assert.Equal(1, scorecards.Count);
            Assert.Equal(1, scorecards[0].Id);
            Assert.Equal(2, scorecardPlayers.Count);
        }

        private Task<HttpResponseMessage> ImportAsync(string scorecardCsv)
        {
            return Client.PostAsync("/import", new MultipartFormDataContent
            {
                {new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(scorecardCsv))), "file", "scorecards.csv"}
            });
        }
    }
}