using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Collections.Generic;
using AutoMapper;

namespace fangraph_priceguide_generator.Commands
{
    public class ConversionRecordCommand : ICommand
    {
        public static void Configure(CommandLineApplication command) {
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
                    (new ConversionRecordCommand(optionMasterCSV, optionSaveLocation)).Run();                    
                });                                    
        }

        private readonly CommandOption _optionMasterCSV;
        private readonly CommandOption _optionSaveLocation;


        public ConversionRecordCommand(CommandOption optionMasterCSV, CommandOption optionSaveLocation)
        {
            _optionMasterCSV = optionMasterCSV;
            _optionMasterCSV = optionSaveLocation;
        }

        public void Run()
        {
            string masterCSVFileLocation = _optionMasterCSV.Value() ?? string.Empty;
            string saveLocation = _optionSaveLocation.Value() ?? string.Empty;
            List<MasterConversionRecord> masterRecords = FileHelper.LoadFile<MasterConversionRecord>(masterCSVFileLocation);
            List<ConversionRecord> conversionRecords = Mapper.Map<List<MasterConversionRecord>, List<ConversionRecord>>(masterRecords);
            FileHelper.SaveFile<ConversionRecord>(conversionRecords, Path.Combine(saveLocation,"IdConversion-FULL-New.csv"));  
            // Console.WriteLine("conversion-record......");
            // Console.WriteLine("optionMasterCSV:" + optionMasterCSV.Value());
            // Console.WriteLine("optionSaveLocation:" + optionSaveLocation.Value());
        }
    }
}