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
                var fgPitcher = args.GetValue(5).ToString();

                Console.WriteLine("name: {0}", idMapFileName);
                Console.WriteLine("path: {0}", idConversionPath);
                Console.WriteLine("positionsPath: {0}", positionsPath);
                Console.WriteLine("fgBatter: {0}", fgBatter);
                Console.WriteLine("fgPitcher: {0}", fgPitcher);

                SetUpMappings();

                List<MasterConversionRecord> masterRecords = LoadFile<MasterConversionRecord>(idMapFileName);
                List<LahmanAppearancesRecord> lahmanRecords = LoadFile<LahmanAppearancesRecord>(positionsPath);
                List<FangraphHitterRecord> fgHitterRecords = LoadFile<FangraphHitterRecord>(fgBatter);
                List<FangraphPitchingRecord> fgPitchingRecords = LoadFile<FangraphPitchingRecord>(fgPitcher);

                List<ConversionRecord> conversionRecords = Mapper.Map<List<MasterConversionRecord>, List<ConversionRecord>>(masterRecords);
                Console.WriteLine("converted records.count {0}", conversionRecords.Count);

                SaveFile<ConversionRecord>(conversionRecords, idConversionPath);

                CreateHitterRecord(year, masterRecords, lahmanRecords, fgHitterRecords);
                CreatePitchingRecord(year, masterRecords, lahmanRecords, fgPitchingRecords);

                SaveFile<FangraphHitterRecord>(fgHitterRecords, "/Users/eric.neunaber/Downloads/fred.csv");
                AlterHitterHeaders("/Users/eric.neunaber/Downloads/fred.csv");
                SaveFile<FangraphPitchingRecord>(fgPitchingRecords, "/Users/eric.neunaber/Downloads/ted.csv");
            }
            catch (Exception ex){
                Console.WriteLine("Except caught:" + ex.Message);
                Console.WriteLine(ex.Data["CsvHelper"]);
            }
            Console.WriteLine("End....");
        }

        private static void CreateHitterRecord(int year, List<MasterConversionRecord> records, List<LahmanAppearancesRecord> lahmanRecords, List<FangraphHitterRecord> fgrecords)
        {
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
        }

        private static void CreatePitchingRecord(int year, List<MasterConversionRecord> records, List<LahmanAppearancesRecord> lahmanRecords, List<FangraphPitchingRecord> fgrecords)
        {
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
                        if(lahmanMatch.GS.HasValue) {
                            x.G_SP = lahmanMatch.GS.Value;
                            x.G_RP = lahmanMatch.G_p - lahmanMatch.GS.Value;
                        }
                        x.league = lahmanMatch.lgID;
                    }
                }
            });
        }

        private static void SetUpMappings()
        {
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
        }

        public static List<T> LoadFile<T>(string fileLocation) {
            List<T> records = new List<T>();
            Console.WriteLine("Begin reading for file: {0}", fileLocation);
            using (TextReader reader = File.OpenText(fileLocation)) {
                var csv = new CsvReader( reader );
                csv.Configuration.RegisterClassMap<FangraphHitterRecordMap>();
                csv.Configuration.RegisterClassMap<FangraphPitcherRecordMap>();
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
        private static void AlterHitterHeaders(string fileLocation)
        {
            Console.WriteLine("update replace hitter headers");
            string tempfile = Path.GetTempFileName();
            FileStream fileStream = new FileStream(fileLocation, FileMode.Open);
            FileStream tempStream = new FileStream(tempfile, FileMode.OpenOrCreate);
            
            using (StreamWriter writer = new StreamWriter(tempStream)) {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    var firstLine = reader.ReadLine().Replace("Doubles", "2B").Replace("Triples","3B");
                    writer.WriteLine(firstLine);
                    while (!reader.EndOfStream)
                        writer.WriteLine(reader.ReadLine());
                }
            }
            File.Copy(tempfile, fileLocation, true);
        }        

    }
}
