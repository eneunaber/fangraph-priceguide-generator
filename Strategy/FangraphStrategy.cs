using System.Collections.Generic;

namespace fangraph_priceguide_generator.Strategy
{
    public interface IFangraphStrategy<T>
    {
        List<T> LoadCSV(string fileLocation);
        void WriteCSV(ExtraDetails extraDetails, List<T> contents, string fileLocation);
    }
}