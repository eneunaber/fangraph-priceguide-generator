using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;
using AutoMapper;

namespace fangraph_priceguide_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin....");
            try {
                var idMapFileName = args.GetValue(0).ToString();
                var idConversionPath = args.GetValue(1).ToString();
                Console.WriteLine("name: {0}", idMapFileName);
                Console.WriteLine("path: {0}", idConversionPath);

                List<MasterConversionRecord> records;

                using (TextReader reader = File.OpenText(idMapFileName)) {
                    var csv = new CsvReader( reader );
                    records = csv.GetRecords<MasterConversionRecord>().ToList();
                }
                Console.WriteLine("records.count {0}", records.Count);

                Mapper.Initialize(cfg => 
                    cfg.CreateMap<MasterConversionRecord, ConversionRecord>()
                        .ForMember(dest => dest.mlbamID, opt => opt.MapFrom(src => src.mlb_id))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.mlb_name))
                        .ForMember(dest => dest.BDB, opt => opt.MapFrom(src => src.bref_id))
                        .ForMember(dest => dest.statsID, opt => opt.MapFrom(src => src.nfbc_id))
                        .ForMember(dest => dest.cbsID, opt => opt.MapFrom(src => src.cbs_id))
                        .ForMember(dest => dest.espnID, opt => opt.MapFrom(src => src.espn_id))
                        .ForMember(dest => dest.retroID, opt => opt.MapFrom(src => src.retro_id))
                        .ForMember(dest => dest.Pos_Class, opt => opt.MapFrom(src => src.yahoo_pos))
                        .ForMember(dest => dest.bisID, opt => opt.MapFrom(src => src.fg_id))
                        .ForMember(dest => dest.team, opt => opt.MapFrom(src => src.mlb_team))
                );

                List<ConversionRecord> conversionRecords = Mapper.Map<List<MasterConversionRecord>, List<ConversionRecord>>(records);
                Console.WriteLine("converted records.count {0}", conversionRecords.Count);

                using (TextWriter writer = File.CreateText(idConversionPath)) {
                    var csv = new CsvWriter( writer );
                    csv.WriteRecords(conversionRecords);
                    Console.WriteLine("....Done writing.....");
                }

            } catch(Exception ex){
                Console.WriteLine("Except caught:" + ex.Message);
                Console.WriteLine(ex.Data["CsvHelper"]);
            }
            Console.WriteLine("End....");
        }
    }
}
