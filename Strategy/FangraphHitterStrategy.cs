using System.Collections.Generic;

namespace fangraph_priceguide_generator.Strategy
{
    public abstract class FangraphHitterStrategy : FangraphStrategy
    {
        public override List<FangraphHitterRecord> LoadCSV<FangraphHitterRecord>(string location) {
            return null;
        }
        public override void WriteCSV<FangraphHitterRecord>(ExtraDetails extraDetails, List<FangraphHitterRecord> contents, string location) {

        }
    }
}