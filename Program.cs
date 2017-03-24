using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using fangraph_priceguide_generator.Strategy;

namespace fangraph_priceguide_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin....");
            try
            {
                var year = Convert.ToInt32(args.GetValue(0));
                var idMapFileName = args.GetValue(1).ToString();
                var idConversionPath = args.GetValue(2).ToString();
                var positionsPath = args.GetValue(3).ToString();

                Console.WriteLine("name: {0}", idMapFileName);
                Console.WriteLine("path: {0}", idConversionPath);
                Console.WriteLine("positionsPath: {0}", positionsPath);

                AutoMapperMappings.Setup();

                List<MasterConversionRecord> masterRecords = FileHelper.LoadFile<MasterConversionRecord>(idMapFileName);
                List<LahmanAppearancesRecord> lahmanRecords = FileHelper.LoadFile<LahmanAppearancesRecord>(positionsPath);
                List<ConversionRecord> conversionRecords = Mapper.Map<List<MasterConversionRecord>, List<ConversionRecord>>(masterRecords);
                Console.WriteLine("converted records.count {0}", conversionRecords.Count);
                FileHelper.SaveFile<ConversionRecord>(conversionRecords, idConversionPath);                

                var extraDetails = new ExtraDetails { Year = year, MasterRecords = masterRecords, LahmanRecords = lahmanRecords };
                for(int x = 4; x <= args.Length - 1; x++) {
                    try {
                        var strategy1 = new FangraphHitterStrategy();
                        var result = strategy1.LoadCSV(args[x]);
                        strategy1.WriteCSV(extraDetails,result,"/Users/eric.neunaber/Downloads/fred.csv");
                    } catch(Exception ex) {
                        var strategy1 = new FangraphPitcherStrategy();
                        var result = strategy1.LoadCSV(args[x]);
                        strategy1.WriteCSV(extraDetails,result,"/Users/eric.neunaber/Downloads/ted.csv");
                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine("Except caught:" + ex.Message);
                Console.WriteLine(ex.Data["CsvHelper"]);
            }
            Console.WriteLine("End....");
        }
    }
}
