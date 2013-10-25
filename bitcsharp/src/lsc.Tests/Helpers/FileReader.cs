using System.Collections.Generic;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace lsc.Tests
{
    public partial class Helpers
    {
        public static FileInfo[] ReadCsvAsFileInfos(string path, bool hasHeaders)
        {
            System.Console.WriteLine(path);
            using (CsvReader csv = new CsvReader(new StreamReader(path), hasHeaders))
            {
                List<FileInfo> data = new List<FileInfo>(25);
                while (csv.ReadNextRecord())
                    data.Add(new FileInfo { Path = csv[0], Description = csv[1] });
                return data.ToArray();
            }
        }
    }
}
