using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;

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
            } catch(Exception ex){
                Console.WriteLine("Except caught:" + ex.Message);
                Console.WriteLine(ex.Data["CsvHelper"]);
            }
            Console.WriteLine("End....");
        }
    }
}
