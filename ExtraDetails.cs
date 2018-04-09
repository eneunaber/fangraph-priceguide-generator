using System.Collections.Generic;
using fangraph_priceguide_generator.Models;

namespace fangraph_priceguide_generator
{
    public class ExtraDetails
    {
        public int Year {get;set;} 
        public List<MasterConversionRecord> MasterRecords {get;set;}
        public List<LahmanAppearancesRecord> LahmanRecords {get;set;}
    }
}