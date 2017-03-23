using System.Collections.Generic;

namespace fangraph_priceguide_generator
{
    public class ExtraDetails
    {
        public int Year {get;set;} 
        public List<MasterConversionRecord> masterRecords {get;set;}
        public List<LahmanAppearancesRecord> lahmanRecords {get;set;}
    }
}