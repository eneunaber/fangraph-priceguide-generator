using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Collections.Generic;
using AutoMapper;
using fangraph_priceguide_generator.Strategy;

namespace fangraph_priceguide_generator.Commands
{
    public class ConfigureFangraphCommand : ICommand
    {
        public static void Configure(CommandLineApplication command) {
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
                    (new ConfigureFangraphCommand(optionPreviousYear, optionMasterCSV, optionLahmanAppearancesLocation, optionSaveLocation, optionFangraphFiles)).Run();
                });                                    
        }

        private readonly CommandOption _optionPreviousYear;
        private readonly CommandOption _optionMasterCSV;
        private readonly CommandOption _optionLahmanAppearancesLocation;
        private readonly CommandOption _optionSaveLocation;
        private readonly CommandOption _optionFangraphFiles;

        public ConfigureFangraphCommand(CommandOption optionPreviousYear, CommandOption optionMasterCSV, CommandOption optionLahmanAppearancesLocation, CommandOption optionSaveLocation, CommandOption optionFangraphFiles)
        {
            _optionPreviousYear = optionPreviousYear;
            _optionMasterCSV = optionMasterCSV;
            _optionLahmanAppearancesLocation = optionLahmanAppearancesLocation;
            _optionSaveLocation = optionSaveLocation;
            _optionFangraphFiles = optionFangraphFiles;
        }

        public void Run()
        {
            int previousYear = int.Parse(_optionPreviousYear.Value());
            string masterCSVFileLocation = _optionMasterCSV.Value() ?? string.Empty;
            string lahmanAppearancesLocation = _optionLahmanAppearancesLocation.Value() ?? string.Empty;
            string saveLocation = _optionSaveLocation.Value() ?? string.Empty;
            List<string> fangraphFiles = _optionFangraphFiles.Values;


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
        }
    }
}