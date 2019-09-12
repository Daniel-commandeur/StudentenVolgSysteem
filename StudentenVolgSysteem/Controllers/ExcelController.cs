using StudentenVolgSysteem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentenVolgSysteem.Controllers
{
    public class ExcelController : Controller
    {
        private const char LF = '\n';

        // GET: Excel
        public ActionResult Index()
        {
            return View();
        }

        private void GetDropdownSheetData(MyDbContext db, string filePath)
        {
            //string pathToSheetTwo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\InfraWorkshopsSheet2.csv";
            StreamReader SheetTwoReader = new StreamReader(filePath);

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
        }

        private void GetTopicSheetData(MyDbContext db, string filePath)
        {
            //string pathToSheetOne = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\InfraWorkshopsSheet1.csv";
            //Console.WriteLine(pathToSheetOne);
            StreamReader SheetOneReader = new StreamReader(filePath);
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
                    splitLine[splitLine.Count - 1] = splitLine[splitLine.Count - 1] + moreSplits[0];
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

                TopicModel topicModel = new TopicModel();
                int counter = 0;
                foreach (var splitTopicEntry in splitLine)
                {
                    if (splitTopicEntry != string.Empty &&
                        splitTopicEntry != LF.ToString())
                    {
                        string type = Headers[counter];
                        counter++;
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
                                topicModel.Niveau = FindNiveau(splitTopicEntry, db);
                                break;
                            case "Topic":
                                // dit is de naam
                                topicModel.Name = splitTopicEntry;
                                break;
                            case "Duur":
                                topicModel.Duur = FindDuration(splitTopicEntry, db);
                                break;
                            case "Werkvorm(en)":
                                topicModel.Werkvorm = FindWerkvorm(splitTopicEntry, db);
                                break;
                            case "Leerdoel(en)":
                                // Dit zijn de leerdoelen
                                topicModel.Leerdoel = splitTopicEntry;
                                break;
                            case "Certificering":
                                topicModel.Certificeringen = FindCerts(splitTopicEntry, db);
                                break;
                            case "Benodigde voorkennis":
                                topicModel.Voorkennis = FindVoorkennis(splitTopicEntry, db);
                                break;
                            case "Inhoud":
                                // Dit is een beschrijving van de inhoud
                                topicModel.Inhoud = splitTopicEntry;
                                break;
                            case "Benodigdheden":
                                topicModel.Benodigdheden = FindBenodigdheden(splitTopicEntry, db);
                                break;
                            case "Percipio links":
                                topicModel.PercipioLinks = FindLinks(splitTopicEntry, db);
                                break;
                            case "Tags 1":
                                topicModel.Tags.Add(FindTag(splitTopicEntry, db));
                                break;
                            case "Tags 2":
                                topicModel.Tags.Add(FindTag(splitTopicEntry, db));
                                break;
                            case "Tags 3":
                                topicModel.Tags.Add(FindTag(splitTopicEntry, db));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private HashSet<TopicModel> FindVoorkennis(string splitTopicEntry, MyDbContext db)
        {
            //split the cell on newline and find all links that match, 
            //make new entries for anythign that doesn't match and add it.
            HashSet<TopicModel> voorkennis = new HashSet<TopicModel>();
            var entries = splitTopicEntry.Split('\n').ToList();
            foreach (var item in entries)
            {
                TopicModel tpm;
                if ((tpm = db.Topics.Where(a => a.Code == item).First()) == null)
                {
                    //TODO: log or display error in topic Hierarchy
                }
                voorkennis.Add(tpm);
            }
            return voorkennis;
        }

        private HashSet<PercipiolinkModel> FindLinks(string splitTopicEntry, MyDbContext db)
        {
            //split the cell on newline and find all links with that match, 
            //make new entries for anything that doesn't match and add them
            HashSet<PercipiolinkModel> percipiolinks = new HashSet<PercipiolinkModel>();
            var entries = splitTopicEntry.Split('\n').ToList();
            foreach (var item in entries)
            {
                PercipiolinkModel plm;
                if((plm = db.PercipioLinks.Where(a => a.Link == item).First()) == null)
                {
                    plm = MakePercipioLink(item, db);
                }
                percipiolinks.Add(plm);
            }
            return percipiolinks;
        }

        private PercipiolinkModel MakePercipioLink(string item, MyDbContext db)
        {
            if (db.PercipioLinks.Where(a => a.Link == item).First() == null)
            {
                try
                {
                    db.PercipioLinks.Add(new PercipiolinkModel { Link = item });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.PercipioLinks.Where(a => a.Link == item).First();
        }

        private HashSet<BenodigdheidModel> FindBenodigdheden(string splitTopicEntry, MyDbContext db)
        {
            //split the cell on newline and find all links that match, 
            //make new entries for anythign that doesn't match and add it.
            HashSet<BenodigdheidModel> benodigdheden = new HashSet<BenodigdheidModel>();
            var entries = splitTopicEntry.Split('\n').ToList();
            foreach (var item in entries)
            {
                BenodigdheidModel bhm;
                if ((bhm = db.Benodigdheden.Where(a => a.Content == item).First()) == null)
                {
                    bhm = MakeBenodigdheid(item, db);
                }
                benodigdheden.Add(bhm);
            }
            return benodigdheden;
        }

        private BenodigdheidModel MakeBenodigdheid(string item, MyDbContext db)
        {
            if (db.Benodigdheden.Where(a => a.Content == item).First() == null)
            {
                try
                {
                    db.Benodigdheden.Add(new BenodigdheidModel { Content = item });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.Benodigdheden.Where(a => a.Content == item).First();
        }

        private HashSet<CertificeringenInfraModel> FindCerts(string splitTopicEntry, MyDbContext db)
        {
            //split the cell on newline and find all links that match, 
            //make new entries for anythign that doesn't match and add it.
            HashSet<CertificeringenInfraModel> certs = new HashSet<CertificeringenInfraModel>();
            var entries = splitTopicEntry.Split('\n').ToList();
            foreach (var item in entries)
            {
                CertificeringenInfraModel cim;
                if((cim = db.CertificeringenInfras.Where(a => a.Certificering == item).First()) == null)
                {
                    cim = MakeCert(item, db);
                }
                certs.Add(cim);
            }
            return certs;
        }

        private CertificeringenInfraModel MakeCert(string item, MyDbContext db)
        {
            if (db.CertificeringenInfras.Where(a => a.Certificering == item).First() == null)
            {
                try
                {
                    db.CertificeringenInfras.Add(new CertificeringenInfraModel { Certificering = item });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.CertificeringenInfras.Where(a => a.Certificering == item).First();
        }

        private TagModel FindTag(string splitTopicEntry, MyDbContext db)
        {
            TagModel tmd;
            if ((tmd = db.Tags.Where(a => a.Naam == splitTopicEntry).First()) == null)
            {
                tmd = MakeTag(splitTopicEntry, db);
            }
            return tmd;
        }

        private TagModel MakeTag(string item, MyDbContext db)
        {
            if (db.Tags.Where(a => a.Naam == item).First() == null)
            {
                try
                {
                    db.Tags.Add(new TagModel { Naam = item });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.Tags.Where(a => a.Naam == item).First();
        }

        private WerkvormModel FindWerkvorm(string splitTopicEntry, MyDbContext db)
        {
            WerkvormModel wvm;
            if ((wvm = db.Werkvormen.Where(a => a.Werkvorm == splitTopicEntry).First()) == null)
            {
                wvm = MakeWerkvorm(splitTopicEntry, db);
            }
            return wvm;
        }

        private WerkvormModel MakeWerkvorm(string item, MyDbContext db)
        {
            if (db.Werkvormen.Where(a => a.Werkvorm == item).First() == null)
            {
                try
                {
                    db.Werkvormen.Add(new WerkvormModel { Werkvorm = item });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.Werkvormen.Where(a => a.Werkvorm == item).First();
        }

        private TijdsDuurModel FindDuration(string splitTopicEntry, MyDbContext db)
        {
            TijdsDuurModel tdm;
            if((tdm = db.TijdsDuren.Where(a => a.Eenheid == splitTopicEntry).First()) == null)
            {
                tdm = MakeDuur(splitTopicEntry, db);
            }
            return tdm;
        }

        private TijdsDuurModel MakeDuur(string item, MyDbContext db)
        {
            if (db.TijdsDuren.Where(a => a.Eenheid == item).First() == null)
            {
                try
                {
                    db.TijdsDuren.Add(new TijdsDuurModel { Eenheid = item });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.TijdsDuren.Where(a => a.Eenheid == item).First();
        }

        private NiveauModel FindNiveau(string splitTopicEntry, MyDbContext db)
        {
            NiveauModel nvm;
            if((nvm = db.Niveaus.Where(a => a.Niveau == splitTopicEntry).First()) == null)
            {
                nvm = MakeNiveau(splitTopicEntry, db);
            }
            return nvm;
        }

        private NiveauModel MakeNiveau(string item, MyDbContext db)
        {
            if (db.Niveaus.Where(a => a.Niveau == item).First() == null)
            {
                try
                {
                    db.Niveaus.Add(new NiveauModel { Niveau = item });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.Niveaus.Where(a => a.Niveau == item).First();
        }

    }
}