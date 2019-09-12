using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentenVolgSysteem.Models;

namespace ExcelConverterSVS
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("hoi");
        }

        public static void GetExcelData(MyDbContext db)
        {

            #region Sheet 2
            string pathToSheetTwo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\InfraWorkshopsSheet2.csv";
            StreamReader SheetTwoReader = new StreamReader(pathToSheetTwo);

            var SheetTwoHeaders = SheetTwoReader.ReadLine().Split('$');
            foreach (var item in SheetTwoHeaders)
            {
                Console.WriteLine(item);
            }

            string newLine;
            while ((newLine = SheetTwoReader.ReadLine()) != null)
            {
                var splitSheetTwoLine = newLine.Split('$').ToList();
                foreach (var item in splitSheetTwoLine)
                {
                    if (item != string.Empty)
                    {
                        string typeTwo = SheetTwoHeaders[splitSheetTwoLine.IndexOf(item)];

                        switch (typeTwo)
                        {
                            case "Werkvormen":
                                Console.WriteLine("Item: {0} | Type: {1} |Success: {2}", item, typeTwo, MakeWerkvorm(item, db));
                                break;
                            case "Niveau":
                                Console.WriteLine("Item: {0} | Type: {1} |Success: {2}", item, typeTwo, MakeNiveau(item, db));
                                break;
                            case "Duur":
                                Console.WriteLine("Item: {0} | Type: {1} |Success: {2}", item, typeTwo, MakeDuur(item, db));
                                break;
                            case "Tags":
                                Console.WriteLine("Item: {0} | Type: {1} |Success: {2}", item, typeTwo, MakeTag(item, db));
                                break;
                            case "Certificeringen Infra":
                                Console.WriteLine("Item: {0} | Type: {1} |Success: {2}", item, typeTwo, MakeCert(item, db));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

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

            while ((unsplitTopicEntry = SheetOneReader.ReadLine()) != null)
            {
                var splitLine = unsplitTopicEntry.Split('$').ToList();
                TopicModel topicModel = new TopicModel();
                foreach (var splitTopicEntry in splitLine)
                {
                    if (splitTopicEntry != string.Empty)
                    {
                        string type = Headers[splitLine.IndexOf(splitTopicEntry)];
                        switch (type)
                        {
                            case "#":
                                // De Id wordt door de database gegenereerd
                                break;
                            case "Code":
                                // dit is de code
                                topicModel.Code = splitTopicEntry;
                                break;
                            case "Niveau":
                                // TODO: kies of maak Niveau
                                topicModel.Niveau = FindNiveau(splitTopicEntry, db);
                                break;
                            case "Topic":
                                // dit is de naam
                                topicModel.Name = splitTopicEntry;
                                break;
                            case "Duur":
                                // TODO: kies of maak duur
                                break;
                            case "Werkvorm(en)":
                                // TODO: kies of maak werkvorm
                                break;
                            case "Leerdoel(en)":
                                // Dit zijn de leerdoelen
                                topicModel.Leerdoel = splitTopicEntry;
                                break;
                            case "Certificering":
                                // TODO: kies of maak certificeringen
                                break;
                            case "Benodigde voorkennis":
                                // TODO: kies of maak voorkennis
                                break;
                            case "Inhoud":
                                // Dit is een beschrijving van de inhoud
                                topicModel.Inhoud = splitTopicEntry;
                                break;
                            case "Benodigdheden":
                                // TODO: kies of maak benodigdheden
                                break;
                            case "Percipio links":
                                // TODO: kies of maak Percipio links
                                break;
                            case "Tags 1":
                                // TODO: kies of maak tag1
                                break;
                            case "Tags 2":
                                // TODO: kies of maak tag2
                                break;
                            case "Tags 3":
                                // TODO: kies of maak tag3
                                break;
                            default:
                                break;
                        }
                        //Console.WriteLine(splitTopicEntry);
                    }
                }
            }
            #endregion
            Console.ReadLine();
        }

        private static bool MakeCert(string item, MyDbContext db)
        {
            bool success = false;
            if(db.CertificeringenInfras.Find(item) == null) {
                try
                {
                    db.CertificeringenInfras.Add(new CertificeringenInfraModel { Certificering = item });
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        private static bool MakeTag(string item, MyDbContext db)
        {
            bool success = false;
            if (db.Tags.Find(item) == null)
            {
                try
                {
                    db.Tags.Add(new TagModel { Naam = item });
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        private static bool MakeDuur(string item, MyDbContext db)
        {
            bool success = false;
            if (db.TijdsDuren.Find(item) == null)
            {
                try
                {
                    db.TijdsDuren.Add(new TijdsDuurModel { Eenheid = item });
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        private static bool MakeWerkvorm(string item, MyDbContext db)
        {
            bool success = false;
            if (db.Werkvormen.Find(item) == null)
            {
                try
                {
                    db.Werkvormen.Add(new WerkvormModel { Werkvorm = item });
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        private static bool MakeNiveau(string item, MyDbContext db)
        {
            bool success = false;
            if (db.Niveaus.Find(item) == null)
            {
                try
                {
                    db.Niveaus.Add(new NiveauModel { Niveau = item });
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        private static NiveauModel FindNiveau(string splitTopicEntry, MyDbContext db)
        {
            return db.Niveaus.Find(splitTopicEntry);
        }
    }
}
