using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using StudentenVolgSysteem.Models;
//using StudentenVolgSysteem.DAL;

namespace ExcelConverterSVS
{
    class Program
    {
        const char LF = '\n';

        static void Main(string[] args)
        {
            //MyDbContext db = new MyDbContext();
            //GetExcelData(db);
            FindFiles();

            Console.ReadLine();
        }

        public static void FindFiles()
        {
            string dirPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data";
            IEnumerable<string> filePaths = Directory.EnumerateFiles(dirPath);
            foreach (var item in filePaths)
            {
                Console.WriteLine(item);
            }
        }

        public static void GetExcelData(SVSContext db)
        {

            #region Sheet 2
            //string pathToSheetTwo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\InfraWorkshopsSheet2.csv";
            //StreamReader SheetTwoReader = new StreamReader(pathToSheetTwo);

            //var SheetTwoHeaders = SheetTwoReader.ReadLine().Split('$');
            //foreach (var item in SheetTwoHeaders)
            //{
            //    Console.WriteLine(item);
            //}

            //string newLine;
            //while ((newLine = SheetTwoReader.ReadLine()) != null)
            //{
            //    var splitSheetTwoLine = newLine.Split('$').ToList();
            //    foreach (var item in splitSheetTwoLine)
            //    {
            //        if (item != string.Empty)
            //        {
            //            string typeTwo = SheetTwoHeaders[splitSheetTwoLine.IndexOf(item)];

            //            switch (typeTwo)
            //            {
            //                case "Werkvormen":
            //                    Console.WriteLine("Item: {0} | Type: {1} |Success: {2}", item, typeTwo, MakeWerkvorm(item, db));
            //                    break;
            //                case "Niveau":
            //                    Console.WriteLine("Item: {0} | Type: {1} |Success: {2}", item, typeTwo, MakeNiveau(item, db));
            //                    break;
            //                case "Duur":
            //                    Console.WriteLine("Item: {0} | Type: {1} |Success: {2}", item, typeTwo, MakeDuur(item, db));
            //                    break;
            //                case "Tags":
            //                    Console.WriteLine("Item: {0} | Type: {1} |Success: {2}", item, typeTwo, MakeTag(item, db));
            //                    break;
            //                case "Certificeringen Infra":
            //                    Console.WriteLine("Item: {0} | Type: {1} |Success: {2}", item, typeTwo, MakeCert(item, db));
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //    }
            //}

            #endregion

            #region Sheet 1
            string pathToSheetOne = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\InfraWorkshopsSheet1.csv";
            Console.WriteLine(pathToSheetOne);
            StreamReader SheetOneReader = new StreamReader(pathToSheetOne);
            string unsplitTopicEntry;

            var Headers = SheetOneReader.ReadLine().Split('$');
            foreach (var item in Headers)
            {
                Console.WriteLine(item);
            }

            while ((unsplitTopicEntry = SheetOneReader.ReadLine() + LF) != LF.ToString())
            {
                var splitLine = unsplitTopicEntry.Split('$').ToList();
                //Trim all parts
                var tempSplit = splitLine;
                for (int i = 0; i < splitLine.Count; i++)
                {
                    tempSplit[i] = splitLine[i].Trim(' ', '*', '"', '-', '•');
                }
                splitLine = tempSplit;

                while (splitLine.Count < Headers.Count())
                {
                    var moreLine = SheetOneReader.ReadLine() + LF;
                    var moreSplits = moreLine.Split('$').ToList();
                    //Trim all inner parts
                    var tempSplitsTrimmed = moreSplits;
                    for (int i = 0; i < moreSplits.Count; i++)
                    {
                        tempSplitsTrimmed[i] = moreSplits[i].Trim(' ', '*', '"', '-', '•');
                    }
                    moreSplits = tempSplitsTrimmed;

                    //Join the last and first entry, cut off first entry
                    splitLine[splitLine.Count-1] = splitLine[splitLine.Count-1] + moreSplits[0];
                    moreSplits.Remove(moreSplits[0]);

                    //and add the newline to the current line
                    foreach (var item in moreSplits)
                    {
                        //Console.WriteLine("Pre-trim| {0}", item);
                        var titem = item.Trim(' ', '*', '"', '-', '•');
                        //Console.WriteLine("Post-trim| {0}", titem);
                        splitLine.Add(titem);
                    }
                }

                Topic topicModel = new Topic();
                int counter = 0;
                foreach (var splitTopicEntry in splitLine)
                {
                    if (splitTopicEntry != string.Empty &&
                        splitTopicEntry != LF.ToString())
                    {
                        string type = Headers[counter];
                        //string[] lines;
                        switch (type)
                        {
                            case "#":
                                // De Id wordt door de database gegenereerd
                                break;
                            case "Code":
                                // dit is de code
                                //topicModel.Code = splitTopicEntry;
                                break;
                            case "Niveau":
                                //topicModel.Niveau = FindNiveau(splitTopicEntry, db);
                                break;
                            case "Topic":
                                // dit is de naam
                                //topicModel.Name = splitTopicEntry;
                                break;
                            case "Duur":
                                break;
                            case "Werkvorm(en)":
                                break;
                            case "Leerdoel(en)":
                                // Dit zijn de leerdoelen
                                //topicModel.Leerdoel = splitTopicEntry;
                                break;
                            case "Certificering":
                                //lines = splitTopicEntry.Split('\n');
                                //Console.WriteLine("---LINES CERT---");
                                //foreach (var item in lines)
                                //{
                                //    Console.WriteLine(item);
                                //}
                                break;
                            case "Benodigde voorkennis":
                                //lines = splitTopicEntry.Split('\n');
                                //Console.WriteLine("---LINES VOORKENNIS---");
                                //foreach (var item in lines)
                                //{
                                //    Console.WriteLine(item);
                                //}
                                break;
                            case "Inhoud":
                                // Dit is een beschrijving van de inhoud
                                //topicModel.Inhoud = splitTopicEntry;
                                //lines = splitTopicEntry.Split('\n');
                                //Console.WriteLine("---LINES INHOUD---");
                                //foreach (var item in lines)
                                //{
                                //    Console.WriteLine(item);
                                //}
                                break;
                            case "Benodigdheden":
                                //lines = splitTopicEntry.Split('\n');
                                //Console.WriteLine("---LINES BENOD---");
                                //foreach (var item in lines)
                                //{
                                //    Console.WriteLine(item);
                                //}
                                break;
                            case "Percipio links":
                                //lines = splitTopicEntry.Split('\n');
                                //Console.WriteLine("---LINES LINKS---");
                                //foreach (var item in lines)
                                //{
                                //    Console.WriteLine(item);
                                //}
                                break;
                            case "Tags 1":
                                break;
                            case "Tags 2":
                                break;
                            case "Tags 3":
                                //int a = (int)splitTopicEntry[0];
                                //Console.WriteLine(a);
                                break;
                            default:
                                break;
                        }
                        Console.WriteLine("-----{0}-----", type);
                        Console.WriteLine(splitTopicEntry);
                    }
                    counter++;
                }
            }
            #endregion
            Console.ReadLine();
        }

        private static bool MakeCert(string item, SVSContext db)
        {
            bool success = false;
            if(db.Certificeringen.Find(item) == null) {
                try
                {
                    db.Certificeringen.Add(new Certificering { Naam = item });
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        private static bool MakeTag(string item, SVSContext db)
        {
            bool success = false;
            if (db.Tags.Find(item) == null)
            {
                try
                {
                    db.Tags.Add(new Tag { Naam = item });
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        private static bool MakeDuur(string item, SVSContext db)
        {
            bool success = false;
            if (db.Tijdsduren.Find(item) == null)
            {
                try
                {
                    db.Tijdsduren.Add(new Tijdsduur { Eenheid = item });
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        private static bool MakeWerkvorm(string item, SVSContext db)
        {
            bool success = false;
            if (db.Werkvormen.Find(item) == null)
            {
                try
                {
                    db.Werkvormen.Add(new Werkvorm { Naam = item });
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        private static bool MakeNiveau(string item, SVSContext db)
        {
            bool success = false;
            if (db.Niveaus.Find(item) == null)
            {
                try
                {
                    db.Niveaus.Add(new Niveau { Naam = item });
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        private static Niveau FindNiveau(string splitTopicEntry, SVSContext db)
        {
            return db.Niveaus.Find(splitTopicEntry);
        }
    }
}
