using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Frisbeeboys.Web.Data;
using Microsoft.AspNetCore.Http;

namespace Frisbeeboys.Web.Controllers.Import.Services
{
    public class ImportService
    {
        private readonly UDiscCsvParser _parser;
        private readonly ScorecardDatabase _database;

        public ImportService(UDiscCsvParser parser, ScorecardDatabase database)
        {
            _parser = parser;
            _database = database;
        }

        public async Task<int> ImportAsync(IFormFile file)
        {
            var udiscScorecards = await ParseCsvAsync(file);
            return await SaveToDatabaseAsync(udiscScorecards);
        }

        private async Task<UDiscScorecard[]> ParseCsvAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            return await _parser.ParseUDiscCsvAsync(reader);
        }

        private async Task<int> SaveToDatabaseAsync(IEnumerable<UDiscScorecard> udiscScorecards)
        {
            var count = 0;

            using var transaction = TransactionFactory.CreateTransactionScope();
            
            foreach (var uDiscScorecard in udiscScorecards.Where(s => s.Players.Count > 1))
            {
                if (await UpsertScorecardAsync(uDiscScorecard) != null)
                {
                    count++;
                }
            }

            transaction.Complete();

            return count;
        }
        
        private async Task<int?> UpsertScorecardAsync(UDiscScorecard scorecard)
        {
            const string insertSql =
                @"INSERT INTO scorecards (course_name, layout_name, date, hole_pars) 
                  VALUES (:CourseName, :LayoutName, :Date, :HolePars)           
                  ON CONFLICT ON CONSTRAINT scorecards_course_name_layout_name_date_key DO NOTHING                             
                  RETURNING id";

            var id = await _database.QuerySingleOrDefaultAsync<int?>(insertSql, scorecard);

            if (id != null)
            {
                const string insertPlayersSql = 
                    @"INSERT INTO scorecard_players (scorecard_id, name, total, par, hole_scores) 
                      VALUES (:Id, :Name, :Total, :Par, :HoleScores)";
                
                await _database.ExecuteAsync(insertPlayersSql, scorecard.Players.Select(p => new
                {
                    Id = id,
                    p.Name,
                    p.Total,
                    p.Par,
                    p.HoleScores
                }));
            }

            return id;
        }
    }
}