using System.Collections.Generic;

namespace fangraph_priceguide_generator.Strategy
{
    public abstract class FangraphStrategy
    {
        public abstract List<T> LoadCSV<T>(string location);
        public abstract void WriteCSV<T>(ExtraDetails extraDetails, List<T> contents, string location);
    }
}