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
            try
            {
                var year = Convert.ToInt32(args.GetValue(0));
                var idMapFileName = args.GetValue(1).ToString();
                var idConversionPath = args.GetValue(2).ToString();
                var positionsPath = args.GetValue(3).ToString();
                var fgBatter = args.GetValue(4).ToString();
                Console.WriteLine("name: {0}", idMapFileName);
                Console.WriteLine("path: {0}", idConversionPath);
                Console.WriteLine("positionsPath: {0}", positionsPath);
                Console.WriteLine("fgBatter: {0}", fgBatter);

                List<MasterConversionRecord> records = LoadFile<MasterConversionRecord>(idMapFileName);

                Mapper.Initialize(cfg =>
                    cfg.CreateMap<MasterConversionRecord, ConversionRecord>()
                        .ForMember(dest => dest.mlbamID, opt => opt.MapFrom(src => src.mlb_id))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.mlb_name))
                        .ForMember(dest => dest.BDB, opt => opt.MapFrom(src => src.bref_id))
                        .ForMember(dest => dest.statsID, opt => opt.MapFrom(src => src.nfbc_id))
                        .ForMember(dest => dest.cbsID, opt => opt.MapFrom(src => src.cbs_id))
                        .ForMember(dest => dest.espnID, opt => opt.MapFrom(src => src.espn_id))
                        .ForMember(dest => dest.retroID, opt => opt.MapFrom(src => src.retro_id))
                        .ForMember(dest => dest.Pos_Class, opt => opt.MapFrom(src => src.yahoo_pos.Replace("/", "|")))
                        .ForMember(dest => dest.bisID, opt => opt.MapFrom(src => src.fg_id))
                        .ForMember(dest => dest.team, opt => opt.MapFrom(src => src.mlb_team))
                );

                List<ConversionRecord> conversionRecords = Mapper.Map<List<MasterConversionRecord>, List<ConversionRecord>>(records);
                Console.WriteLine("converted records.count {0}", conversionRecords.Count);

                SaveFile<ConversionRecord>(conversionRecords, idConversionPath);

                List<LahmanFielding> lahmanRecords = LoadFile<LahmanFielding>(positionsPath);
                List<FangraphHitterRecord> fgrecords = LoadFile<FangraphHitterRecord>(fgBatter);

                fgrecords.ForEach(x =>
                {
                    var match = records.FirstOrDefault(y => x.playerid == y.fg_id);
                    if (match != null)
                    {
                        x.defaultPos = match.yahoo_pos.Replace("/", "|");
                        x.team = match.mlb_team;
                        x.mlbamID = match.mlb_id;
                        var lahmanMatch = lahmanRecords.FirstOrDefault(z => match.lahman_id == z.playerID && year == z.yearID);
                        if (lahmanMatch != null)
                        {
                            x.G = lahmanMatch.G_all;
                            x.G_1B = lahmanMatch.G_1b;
                            x.G_2B = lahmanMatch.G_2b;
                            x.G_3B = lahmanMatch.G_3b;
                            x.G_SS = lahmanMatch.G_ss;
                            x.G_C = lahmanMatch.G_c;
                            x.G_CF = lahmanMatch.G_cf;
                            x.G_LF = lahmanMatch.G_lf;
                            x.G_RF = lahmanMatch.G_rf;
                            x.G_OF = lahmanMatch.G_of;
                            x.league = lahmanMatch.lgID;
                        }
                    }
                });

                SaveFile<FangraphHitterRecord>(fgrecords, "/Users/eric.neunaber/Downloads/fred.csv");
            }
            catch (Exception ex){
                Console.WriteLine("Except caught:" + ex.Message);
                Console.WriteLine(ex.Data["CsvHelper"]);
            }
            Console.WriteLine("End....");
        }
        public static List<T> LoadFile<T>(string fileLocation) {
            List<T> records = new List<T>();
            Console.WriteLine("Begin reading for file: {0}", fileLocation);
            using (TextReader reader = File.OpenText(fileLocation)) {
                var csv = new CsvReader( reader );
                csv.Configuration.RegisterClassMap<FangraphHitterRecordMap>();
                records = csv.GetRecords<T>().ToList();
            }
            Console.WriteLine("{0}.count {1}", typeof(T).FullName,records.Count);
            return records;
        }
        private static void SaveFile<T>(List<T> records, string fileLocation)
        {
            Console.WriteLine("fgRecord.count: {0}", records.Count);
            using (TextWriter writer = File.CreateText(fileLocation))
            {
                var csv = new CsvWriter(writer);
                csv.WriteRecords(records);
                Console.WriteLine("....Done writing.....");
            }
        }        

    }
}
