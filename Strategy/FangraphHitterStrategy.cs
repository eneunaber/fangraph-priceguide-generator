using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using fangraph_priceguide_generator.Models;


namespace fangraph_priceguide_generator.Strategy
{
    public class FangraphHitterStrategy : FangraphStrategy<FangraphHitterRecord>
    {
        public override List<FangraphHitterRecord> LoadCSV(string fileLocation)
        {
            return FileHelper.LoadFile<FangraphHitterRecord>(fileLocation);
        }

        public override void WriteCSV(ExtraDetails extraDetails, List<FangraphHitterRecord> fgRecords, string fileLocation)
        {
            CreateHitterRecord(extraDetails, fgRecords);
            FileHelper.SaveFile(fgRecords, fileLocation);
            AlterHitterHeaders(fileLocation);
        }

        private static void CreateHitterRecord(ExtraDetails extraDetails, List<FangraphHitterRecord> fgRecords)
        {
            fgRecords.ForEach(fangraphRecord =>
            {
                var match = extraDetails.MasterRecords.FirstOrDefault(masterRecord => fangraphRecord.playerid == masterRecord.fg_id);
                if (match != null)
                {
                    fangraphRecord.defaultPos = match.yahoo_pos.Replace("/", "|");
                    fangraphRecord.team = match.mlb_team;
                    fangraphRecord.mlbamID = match.mlb_id;
                    var lahmanMatch = extraDetails.LahmanRecords.FirstOrDefault(z => match.lahman_id == z.playerID && extraDetails.Year == z.yearID);
                    if (lahmanMatch != null)
                    {
                        fangraphRecord.G = lahmanMatch.G_all;
                        fangraphRecord.G_1B = lahmanMatch.G_1b;
                        fangraphRecord.G_2B = lahmanMatch.G_2b;
                        fangraphRecord.G_3B = lahmanMatch.G_3b;
                        fangraphRecord.G_SS = lahmanMatch.G_ss;
                        fangraphRecord.G_C = lahmanMatch.G_c;
                        fangraphRecord.G_CF = lahmanMatch.G_cf;
                        fangraphRecord.G_LF = lahmanMatch.G_lf;
                        fangraphRecord.G_RF = lahmanMatch.G_rf;
                        fangraphRecord.G_OF = lahmanMatch.G_of;
                        fangraphRecord.league = lahmanMatch.lgID;
                    }
                }
            });
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