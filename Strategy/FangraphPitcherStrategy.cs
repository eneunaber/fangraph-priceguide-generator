using System.Linq;
using System.Collections.Generic;
using fangraph_priceguide_generator.Models;

namespace fangraph_priceguide_generator.Strategy
{
    public class FangraphPitcherStrategy : FangraphStrategy<FangraphPitcherRecord>
    {
        public override List<FangraphPitcherRecord> LoadCSV(string fileLocation) {
            return FileHelper.LoadFile<FangraphPitcherRecord>(fileLocation);
        }
        public override void WriteCSV(ExtraDetails extraDetails, List<FangraphPitcherRecord> fgRecords, string fileLocation) {
            CreatePitchingRecord(extraDetails, fgRecords);
            FileHelper.SaveFile<FangraphPitcherRecord>(fgRecords, fileLocation);
        }
        private static void CreatePitchingRecord(ExtraDetails extraDetails, List<FangraphPitcherRecord> fgrecords)
        {
            fgrecords.ForEach(x =>
            {
                var match = extraDetails.MasterRecords.FirstOrDefault(y => x.playerid == y.fg_id);
                if (match != null)
                {
                    x.defaultPos = match.yahoo_pos.Replace("/", "|");
                    x.team = match.mlb_team;
                    x.mlbamID = match.mlb_id;
                    var lahmanMatch = extraDetails.LahmanRecords.FirstOrDefault(z => match.lahman_id == z.playerID && extraDetails.Year == z.yearID);
                    if (lahmanMatch != null)
                    {
                        x.G = lahmanMatch.G_all;
                        if(lahmanMatch.GS.HasValue) {
                            x.G_SP = lahmanMatch.GS.Value;
                            x.G_RP = lahmanMatch.G_p - lahmanMatch.GS.Value;
                        }
                        x.league = lahmanMatch.lgID;
                    }
                }
            });
        }        
    }
}