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
            fgrecords.ForEach(fangraphRecord =>
            {
                var match = extraDetails.MasterRecords.FirstOrDefault(masterRecord => fangraphRecord.playerid == masterRecord.fg_id);
                if (match != null)
                {
                    fangraphRecord.defaultPos = match.yahoo_pos.Replace("/", "|");
                    fangraphRecord.team = match.mlb_team;
                    fangraphRecord.mlbamID = match.mlb_id;
                    var lahmanMatch = extraDetails.LahmanRecords.FirstOrDefault(z => match.lahman_id == z.playerID && extraDetails.Year == z.yearID);
                    if (lahmanMatch != null)
                    {
                        fangraphRecord.G = lahmanMatch.G_all;
                        if(lahmanMatch.GS.HasValue) {
                            fangraphRecord.G_SP = lahmanMatch.GS.Value;
                            fangraphRecord.G_RP = lahmanMatch.G_p - lahmanMatch.GS.Value;
                        }
                        fangraphRecord.league = lahmanMatch.lgID;
                    }
                }
            });
        }        
    }
}