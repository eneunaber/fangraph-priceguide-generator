using System;
using System.IO;
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
                var previousYear = Convert.ToInt32(args.GetValue(0));
                var masterCSVFileLocation = args.GetValue(1).ToString();
                var lahmanAppearancesFileLocation = args.GetValue(2).ToString();
                var saveLocation = args.GetValue(3).ToString();

                Console.WriteLine("==== INPUTS ====");
                Console.WriteLine("previousYear: {0}", previousYear);
                Console.WriteLine("name: {0}", masterCSVFileLocation);
                Console.WriteLine("lahmanAppearancesFileLocation: {0}", lahmanAppearancesFileLocation);
                Console.WriteLine("path: {0}", saveLocation);

                AutoMapperMappings.Setup();

                List<MasterConversionRecord> masterRecords = FileHelper.LoadFile<MasterConversionRecord>(masterCSVFileLocation);
                List<LahmanAppearancesRecord> lahmanRecords = FileHelper.LoadFile<LahmanAppearancesRecord>(lahmanAppearancesFileLocation);
                List<ConversionRecord> conversionRecords = Mapper.Map<List<MasterConversionRecord>, List<ConversionRecord>>(masterRecords);
                Console.WriteLine("converted records.count {0}", conversionRecords.Count);
                FileHelper.SaveFile<ConversionRecord>(conversionRecords, Path.Combine(saveLocation,"IdConversion-FULL-New.csv"));                

                var extraDetails = new ExtraDetails { Year = previousYear, MasterRecords = masterRecords, LahmanRecords = lahmanRecords };
                for(int x = 4; x <= args.Length - 1; x++) {
                    try {
                        var strategy = new FangraphHitterStrategy();
                        var result = strategy.LoadCSV(args[x]);
                        strategy.WriteCSV(extraDetails,result,Path.Combine(saveLocation, Path.GetFileNameWithoutExtension(args[x]) + "-New.csv"));
                    } catch(CsvHelper.CsvMissingFieldException ex) {
                        var strategy = new FangraphPitcherStrategy();
                        var result = strategy.LoadCSV(args[x]);
                        strategy.WriteCSV(extraDetails,result,Path.Combine(saveLocation, Path.GetFileNameWithoutExtension(args[x]) + "-New.csv"));
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
