using CsvHelper.Configuration;

namespace fangraph_priceguide_generator
{
    public class FangraphPitcherRecordMap : CsvClassMap<FangraphPitcherRecord>
    {
        public FangraphPitcherRecordMap(){
            Map(m => m.playerName).Name("Name");
            Map(m => m.G).Name("G");
            Map(m => m.GS).Name("GS");
            Map(m => m.W).Name("W");
            Map(m => m.L).Name("L");
            Map(m => m.IP).Name("IP");
            Map(m => m.H).Name("H");
            Map(m => m.ER).Name("ER");
            Map(m => m.HR).Name("HR");
            Map(m => m.BB).Name("BB");
            Map(m => m.K).Name("SO");
            Map(m => m.playerid).Name("playerid");
            // // Map(m => m.R).Name("R");
            // // Map(m => m.S).Name("S");
            // // Map(m => m.BS).Name("BS");
            // // Map(m => m.HLD).Name("HLD");
            // // Map(m => m.CG).Name("CG");
            // // Map(m => m.SHO).Name("SHO");            
        }
    }
}

