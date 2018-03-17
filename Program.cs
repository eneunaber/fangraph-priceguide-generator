using System;
using System.IO;
using System.Collections.Generic;
using AutoMapper;
using fangraph_priceguide_generator.Strategy;
using McMaster.Extensions.CommandLineUtils;

namespace fangraph_priceguide_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoMapperMappings.Setup();

            var app = new CommandLineApplication();
            app.Name = "priceguide-generator";
            app.HelpOption("-?|-h|--help");

            app.OnExecute(() =>
                {
                    Console.WriteLine("Begin....");
                    return 0;
                });

            app.Command("conversion-record", (command) =>
                {
                    command.Description = "Create an IDConversion record to be used by GreaseMonkey Script.";
                    command.HelpOption("-?|-h|--help");

                    var optionMasterCSV = command.Option("-m|--masterCSV <FileLocation>", 
                                                    "Required. The location of the masterCSV file.", 
                                                    CommandOptionType.SingleValue)
                                                    .IsRequired();                  

                    var optionSaveLocation = command.Option("-s|--save-location <DestinationLocation>", 
                                                    "Required. The location of where IdConversion-FULL-New.csv will be saved.", 
                                                    CommandOptionType.SingleValue)
                                                    .IsRequired();                      

                    command.OnExecute(() =>
                        {
                            try {
                                string masterCSVFileLocation = optionMasterCSV.Value() ?? string.Empty;
                                string saveLocation = optionSaveLocation.Value() ?? string.Empty;
                                List<MasterConversionRecord> masterRecords = FileHelper.LoadFile<MasterConversionRecord>(masterCSVFileLocation);
                                List<ConversionRecord> conversionRecords = Mapper.Map<List<MasterConversionRecord>, List<ConversionRecord>>(masterRecords);
                                FileHelper.SaveFile<ConversionRecord>(conversionRecords, Path.Combine(saveLocation,"IdConversion-FULL-New.csv"));  
                            } catch(Exception ex) {
                                Console.WriteLine("Except caught:" + ex.Message);
                                return 1;
                            }
                            // Console.WriteLine("conversion-record......");
                            // Console.WriteLine("optionMasterCSV:" + optionMasterCSV.Value());
                            // Console.WriteLine("optionSaveLocation:" + optionSaveLocation.Value());
                            return 0;
                        });

                });

            app.Command("convert-fangraph", (command) =>
                {
                    command.Description = "Create an IDConversion record to be used by GreaseMonkey Script.";
                    command.HelpOption("-?|-h|--help");

                    var optionPreviousYear = command.Option("-py|--Previous-Year <PreviousYear>", 
                                                    "Required. The previous MLB season (e.g. 2017).", 
                                                    CommandOptionType.SingleValue)
                                                    .IsRequired();

                    var optionMasterCSV = command.Option("-m|--masterCSV <FileLocation>", 
                                                    "Required. The location of the masterCSV file.", 
                                                    CommandOptionType.SingleValue)
                                                    .IsRequired();                                                                    

                    var optionLahmanAppearancesLocation = command.Option("-l|--lahmanAppearances <FileLocation>", 
                                                    "Required. The location of the Lahman Appearances file.", 
                                                    CommandOptionType.SingleValue)
                                                    .IsRequired();                                                                    

                    var optionSaveLocation = command.Option("-s|--save-location <DestinationLocation>", 
                                                    "Required. The location of where IdConversion-FULL-New.csv will be saved.", 
                                                    CommandOptionType.SingleValue)
                                                    .IsRequired();                  

                    var optionFangraphFiles = command.Option("-f|--fangraph-files <FangraphFileLocation>", 
                                                    "Required. The location of the Fangraph files.", 
                                                    CommandOptionType.MultipleValue)
                                                    .IsRequired();                  

                    command.OnExecute(() =>
                        {
                            try {
                                int previousYear = int.Parse(optionPreviousYear.Value());
                                string masterCSVFileLocation = optionMasterCSV.Value() ?? string.Empty;
                                string lahmanAppearancesLocation = optionLahmanAppearancesLocation.Value() ?? string.Empty;
                                string saveLocation = optionSaveLocation.Value() ?? string.Empty;
                                List<string> fangraphFiles = optionFangraphFiles.Values;


                                // Console.WriteLine("convert-fangraph......");
                                // Console.WriteLine("previousYear:" + optionPreviousYear.Value());
                                // Console.WriteLine("masterCSVFileLocation:" + optionMasterCSV.Value());
                                // Console.WriteLine("lahmanAppearancesLocation:" + optionLahmanAppearancesLocation.Value());
                                // Console.WriteLine("saveLocation:" + optionSaveLocation.Value());
                                // Console.WriteLine("fangraphFiles:" + string.Join(", ", optionFangraphFiles.Values));

                                List<MasterConversionRecord> masterRecords = FileHelper.LoadFile<MasterConversionRecord>(masterCSVFileLocation);
                                List<LahmanAppearancesRecord> lahmanRecords = FileHelper.LoadFile<LahmanAppearancesRecord>(lahmanAppearancesLocation);

                                var extraDetails = new ExtraDetails { Year = previousYear, MasterRecords = masterRecords, LahmanRecords = lahmanRecords };
                                fangraphFiles.ForEach(f => {
                                    try {
                                        var strategy = new FangraphHitterStrategy();
                                        var result = strategy.LoadCSV(f);
                                        strategy.WriteCSV(extraDetails,result,Path.Combine(saveLocation, Path.GetFileNameWithoutExtension(f) + "-New.csv"));
                                    } catch(CsvHelper.CsvMissingFieldException) {
                                        Console.WriteLine("Failed to convert {0}, trying pitcher strategy", f);
                                        var strategy = new FangraphPitcherStrategy();
                                        var result = strategy.LoadCSV(f);
                                        strategy.WriteCSV(extraDetails,result,Path.Combine(saveLocation, Path.GetFileNameWithoutExtension(f) + "-New.csv"));
                                    }                                    
                                });
                            } catch(Exception ex) {
                                Console.WriteLine("Exception caught:" + ex.Message);
                                return 1;
                            }
                            return 0;
                        });

                });
            
            app.Execute(args);


            // try
            // {
            //     var previousYear = Convert.ToInt32(args.GetValue(0));
            //     var masterCSVFileLocation = args.GetValue(1).ToString();
            //     var lahmanAppearancesFileLocation = args.GetValue(2).ToString();
            //     var saveLocation = args.GetValue(3).ToString();

            //     Console.WriteLine("==== INPUTS ====");
            //     Console.WriteLine("previousYear: {0}", previousYear);
            //     Console.WriteLine("name: {0}", masterCSVFileLocation);
            //     Console.WriteLine("lahmanAppearancesFileLocation: {0}", lahmanAppearancesFileLocation);
            //     Console.WriteLine("path: {0}", saveLocation);

            //     AutoMapperMappings.Setup();

            //     List<MasterConversionRecord> masterRecords = FileHelper.LoadFile<MasterConversionRecord>(masterCSVFileLocation);
            //     List<LahmanAppearancesRecord> lahmanRecords = FileHelper.LoadFile<LahmanAppearancesRecord>(lahmanAppearancesFileLocation);

            //     var extraDetails = new ExtraDetails { Year = previousYear, MasterRecords = masterRecords, LahmanRecords = lahmanRecords };
            //     for(int x = 4; x <= args.Length - 1; x++) {
            //         try {
            //             var strategy = new FangraphHitterStrategy();
            //             var result = strategy.LoadCSV(args[x]);
            //             strategy.WriteCSV(extraDetails,result,Path.Combine(saveLocation, Path.GetFileNameWithoutExtension(args[x]) + "-New.csv"));
            //         } catch(CsvHelper.CsvMissingFieldException ex) {
            //             var strategy = new FangraphPitcherStrategy();
            //             var result = strategy.LoadCSV(args[x]);
            //             strategy.WriteCSV(extraDetails,result,Path.Combine(saveLocation, Path.GetFileNameWithoutExtension(args[x]) + "-New.csv"));
            //         }
            //     }
            // }
            // catch (Exception ex){
            //     Console.WriteLine("Except caught:" + ex.Message);
            //     Console.WriteLine(ex.Data["CsvHelper"]);
            // }
            // Console.WriteLine("End....");
        }
    }
}
