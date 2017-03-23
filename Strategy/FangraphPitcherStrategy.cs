using System.Collections.Generic;

namespace fangraph_priceguide_generator.Strategy
{
    public abstract class FangraphPitcherrStrategy : FangraphStrategy
    {
        public override List<FangraphPitcherRecord> LoadCSV<FangraphPitcherRecord>(string location) {
            return null;
        }
        public override void WriteCSV<FangraphPitcherRecord>(ExtraDetails extraDetails, List<FangraphPitcherRecord> contents, string location) {

        }
    }
}