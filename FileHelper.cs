using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;
using fangraph_priceguide_generator.Models;
using fangraph_priceguide_generator.Mappings;

namespace fangraph_priceguide_generator
{
    public class FileHelper
    {
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

        public static void SaveFile<T>(List<T> records, string fileLocation)
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