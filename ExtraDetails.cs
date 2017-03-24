using System.Collections.Generic;

namespace fangraph_priceguide_generator
{
    public class ExtraDetails
    {
        public int Year {get;set;} 
        public List<MasterConversionRecord> MasterRecords {get;set;}
        public List<LahmanAppearancesRecord> LahmanRecords {get;set;}
    }
}