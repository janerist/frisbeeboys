using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace Frisbeeboys.Web.Controllers.Import.Services
{
    public class UDiscCsvParser
    {
        /**
         * PlayerName,CourseName,LayoutName,Date,Total,+/-,Hole1,Hole2,Hole3,Hole4,Hole5,Hole6,Hole7,Hole8,Hole9,Hole10,Hole11,Hole12,Hole13,Hole14,Hole15,Hole16,Hole17,Hole18
Par,Othilienborg,Standard,2020-07-16 12:40,34,,3,3,3,3,3,3,3,3,4,3,3,,,,,,,
Jan-Erik Strøm,Othilienborg,Standard,2020-07-16 12:40,39,5,5,3,3,2,4,3,3,3,6,4,3,,,,,,,

         * 
         */
        public async Task<UDiscScorecard[]> ParseUDiscCsvAsync(TextReader csvReader)
        {
            // Skip header
            await csvReader.ReadLineAsync();

            var csv = await csvReader.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(csv)) 
            {
                throw new UDiscCsvParserException("No content");
            }

            try
            {
                return csv
                    .Split('\n')
                    .Aggregate(new List<UDiscScorecard>(), (scorecards, line) =>
                    {
                        if (line.StartsWith("Par"))
                        {
                            var elems = line.Split(',');
                            var scorecard = new UDiscScorecard(
                                CourseName: elems[1],
                                LayoutName: elems[2],
                                Date: DateTime.Parse(elems[3], CultureInfo.InvariantCulture),
                                HolePars: elems.Skip(6)
                                    .TakeWhile(s => int.TryParse(s, out _))
                                    .Select(int.Parse)
                                    .ToArray(),
                                Players: new List<UDiscScorecardPlayer>()
                            );

                            scorecards.Add(scorecard);
                        }
                        else if (!string.IsNullOrWhiteSpace(line))
                        {
                            var elems = line.Split(',');
                            var player = new UDiscScorecardPlayer(
                                Name: elems[0],
                                Total: int.Parse(elems[4]),
                                Par: int.Parse(elems[5]),
                                HoleScores: elems.Skip(6)
                                    .TakeWhile(s => int.TryParse(s, out _))
                                    .Select(int.Parse)
                                    .ToArray()
                            );
                            scorecards.Last().Players.Add(player);
                        }

                        return scorecards;
                    })
                    .ToArray();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CSV parsing failed");
                throw new UDiscCsvParserException("Could not parse", ex);
            }
        }
    }
}