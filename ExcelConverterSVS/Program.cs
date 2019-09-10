using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelConverterSVS
{
    class Program
    {
        static void Main(string[] args)
        {
            string fullPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\InfraWorkshopsSheet1.csv";
            Console.WriteLine(fullPath);
            StreamReader SheetReader = new StreamReader(fullPath);
            string unsplitTopicEntry;

            var Headers = SheetReader.ReadLine().Split('$');

            while ((unsplitTopicEntry = SheetReader.ReadLine()) != null)
            {
                var splitLine = unsplitTopicEntry.Split('$');
                foreach (var splitTopicEntry in splitLine)
                {
                    //AddTopicToSeed(splitTopicEntry);
                    Console.WriteLine(splitTopicEntry);
                }
            }
            Console.ReadLine();
        }

        private static void AddTopicToSeed(string item)
        {
            throw new NotImplementedException();
        }
    }
}
