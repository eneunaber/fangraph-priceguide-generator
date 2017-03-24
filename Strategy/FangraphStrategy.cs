using System.Collections.Generic;

namespace fangraph_priceguide_generator.Strategy
{
    public abstract class FangraphStrategy<T>
    {
        public abstract List<T> LoadCSV(string fileLocation);
        public abstract void WriteCSV(ExtraDetails extraDetails, List<T> contents, string fileLocation);
    }
}