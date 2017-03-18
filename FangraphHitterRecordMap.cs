using CsvHelper.Configuration;

namespace fangraph_priceguide_generator
{
    public class FangraphHitterRecordMap : CsvClassMap<FangraphHitterRecord>
    {
        public FangraphHitterRecordMap(){
            Map(m => m.playerName).Name("Name");
            Map(m => m.G).Name("G");
            Map(m => m.AB).Name("AB");
            Map(m => m.H).Name("H");
            Map(m => m.Doubles).Name("2B");
            Map(m => m.Triples).Name("3B");
            Map(m => m.HR).Name("HR");
            Map(m => m.R).Name("R");
            Map(m => m.RBI).Name("RBI");
            Map(m => m.BB).Name("BB");
            Map(m => m.SO).Name("SO");
            Map(m => m.HBP).Name("HBP");
            Map(m => m.SB).Name("SB");
            Map(m => m.CS).Name("CS");
            Map(m => m.playerid).Name("playerid");
        }
    }
}