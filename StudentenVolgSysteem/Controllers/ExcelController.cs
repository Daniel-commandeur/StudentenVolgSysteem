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

        /// <summary>
        /// Reads and parses data from a $-separated file and writes the parsed dropdown models into the database
        /// </summary>
        /// <param name="db">The database context to write the dropdown models to</param>
        /// <param name="filePath">The path to the $-separated file</param>
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

        /// <summary>
        /// Reads and parses data from a $-separated file and writes the parsed Topics into the database
        /// </summary>
        /// <param name="db">The database context to write the TopicModels to</param>
        /// <param name="filePath">The path to the $-separated file</param>
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

        /// <summary>
        /// Vindt de topics die als voorkennis nodig zijn.
        /// </summary>
        /// <param name="topicsString">Een string met daarin de codes van TopicModels die met LineFeeds gescheiden zijn</param>
        /// <param name="db">De database context om de TopicModels in te vinden</param>
        /// <returns>De HashSet van TopicModels die gevonden zijn</returns>
        private HashSet<TopicModel> FindVoorkennis(string topicsString, MyDbContext db)
        {
            //split the cell on newline and find all links that match, 
            //make new entries for anythign that doesn't match and add it.
            HashSet<TopicModel> voorkennis = new HashSet<TopicModel>();
            var entries = topicsString.Split('\n').ToList();
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

        /// <summary>
        /// Vindt de PercipiolinkModels die in links voorkomen.
        /// Maakt de PercipiolinkModels die niet gevonden kunnen worden.
        /// </summary>
        /// <param name="links">Een string met links die door LineFeeds gescheiden zijn</param>
        /// <param name="db">De database context om de PercipiolinkModels in aan te maken</param>
        /// <returns>De HashSet van PercipiolinkModels die gevonden of gemaakt zijn</returns>
        private HashSet<PercipiolinkModel> FindLinks(string links, MyDbContext db)
        {
            //split the cell on newline and find all links with that match, 
            //make new entries for anything that doesn't match and add them
            HashSet<PercipiolinkModel> percipiolinks = new HashSet<PercipiolinkModel>();
            var entries = links.Split('\n').ToList();
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

        /// <summary>
        /// Maakt een PercipiolinkModel van link
        /// </summary>
        /// <param name="link">De link die in het model moet komen</param>
        /// <param name="db">De database context om het PercipiolinkModel in aan te maken</param>
        /// <returns>Het gemaakte PercipiolinkModel</returns>
        private PercipiolinkModel MakePercipioLink(string link, MyDbContext db)
        {
            if (db.PercipioLinks.Where(a => a.Link == link).First() == null)
            {
                try
                {
                    db.PercipioLinks.Add(new PercipiolinkModel { Link = link });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.PercipioLinks.Where(a => a.Link == link).First();
        }

        /// <summary>
        /// Vindt de BenodigdheidModels die in benodigdhedenString voorkomen.
        /// Maakt de BenodigdheidModels die niet gevonden kunnen worden.
        /// </summary>
        /// <param name="benodigdhedenString">Een string met benodigdheden die door LineFeeds gescheiden zijn</param>
        /// <param name="db">De database context om de benodigdheden in te vinden</param>
        /// <returns>De HashSet van BenodigdheidModel met daarin de gevonden of gemaakte modellen</returns>
        private HashSet<BenodigdheidModel> FindBenodigdheden(string benodigdhedenString, MyDbContext db)
        {
            //split the cell on newline and find all links that match, 
            //make new entries for anythign that doesn't match and add it.
            HashSet<BenodigdheidModel> benodigdheden = new HashSet<BenodigdheidModel>();
            var entries = benodigdhedenString.Split('\n').ToList();
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

        /// <summary>
        /// Maakt een BenodigdheidModel aan met content
        /// </summary>
        /// <param name="content">De content van het BenodigdheidModel</param>
        /// <param name="db">De database om het BenodigdheidModel in aan te maken</param>
        /// <returns>Het aangemaakte BenodigdheidModel</returns>
        private BenodigdheidModel MakeBenodigdheid(string content, MyDbContext db)
        {
            if (db.Benodigdheden.Where(a => a.Content == content).First() == null)
            {
                try
                {
                    db.Benodigdheden.Add(new BenodigdheidModel { Content = content });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.Benodigdheden.Where(a => a.Content == content).First();
        }

        /// <summary>
        /// Vindt de certificeringen die in certifications voorkomen.
        /// Maakt de certificeringen aan die niet gevonden kunnen worden.
        /// </summary>
        /// <param name="certifications">Een string met certificaties die door LineFeeds gescheiden zijn</param>
        /// <param name="db">De database context om de certificeringen in te vinden</param>
        /// <returns>De HashSet van CertificeringenInfraModel met daarin de gevonden of gemaakte modellen</returns>
        private HashSet<CertificeringenInfraModel> FindCerts(string certifications, MyDbContext db)
        {
            //split the cell on newline and find all links that match, 
            //make new entries for anythign that doesn't match and add it.
            HashSet<CertificeringenInfraModel> certs = new HashSet<CertificeringenInfraModel>();
            var entries = certifications.Split('\n').ToList();
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

        /// <summary>
        /// Maakt een CertificeringenInfraModel aan met certificering
        /// </summary>
        /// <param name="certificering">De certificering om aan te maken</param>
        /// <param name="db">De database context om de certificering in aan te maken</param>
        /// <returns>Het aangemaakte CertificeringenInfraModel</returns>
        private CertificeringenInfraModel MakeCert(string certificering, MyDbContext db)
        {
            if (db.CertificeringenInfras.Where(a => a.Certificering == certificering).First() == null)
            {
                try
                {
                    db.CertificeringenInfras.Add(new CertificeringenInfraModel { Certificering = certificering });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.CertificeringenInfras.Where(a => a.Certificering == certificering).First();
        }

        /// <summary>
        /// Vindt het TagModel dat bij tag hoort.
        /// Maakt een nieuw TagModel aan in de database als deze niet gevonden kan worden.
        /// </summary>
        /// <param name="tag">De tag om te vinden</param>
        /// <param name="db">De database om de tag in te vinden</param>
        /// <returns>Het gevonden of gemaakte TagModel</returns>
        private TagModel FindTag(string tag, MyDbContext db)
        {
            TagModel tmd;
            if ((tmd = db.Tags.Where(a => a.Naam == tag).First()) == null)
            {
                tmd = MakeTag(tag, db);
            }
            return tmd;
        }

        /// <summary>
        /// Maakt een Tag aan met tag
        /// </summary>
        /// <param name="tag">De tag om een TagModel van te maken</param>
        /// <param name="db">De database context om het TagModel in aan te maken</param>
        /// <returns>Het aangemaakte TagModel</returns>
        private TagModel MakeTag(string tag, MyDbContext db)
        {
            if (db.Tags.Where(a => a.Naam == tag).First() == null)
            {
                try
                {
                    db.Tags.Add(new TagModel { Naam = tag });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.Tags.Where(a => a.Naam == tag).First();
        }

        /// <summary>
        /// Vindt het WerkvormModel dat bij de werkvorm hoort.
        /// Maakt een nieuw WerkvormModel in de database aan als deze niet gevonden kan worden.
        /// </summary>
        /// <param name="werkvorm">De werkvorm om te vinden</param>
        /// <param name="db">De database context om de werkvorm in te vinden</param>
        /// <returns>Het gevonden of gemaakte WerkvormModel</returns>
        private WerkvormModel FindWerkvorm(string werkvorm, MyDbContext db)
        {
            WerkvormModel wvm;
            if ((wvm = db.Werkvormen.Where(a => a.Werkvorm == werkvorm).First()) == null)
            {
                wvm = MakeWerkvorm(werkvorm, db);
            }
            return wvm;
        }

        /// <summary>
        /// Maakt een Werkvorm aan met werkvorm
        /// </summary>
        /// <param name="werkvorm">De werkvorm om een WerkvormModel van te maken</param>
        /// <param name="db">De database context om het WerkvormModel in aan te maken</param>
        /// <returns>Het aangemaakte WerkvormModel</returns>
        private WerkvormModel MakeWerkvorm(string werkvorm, MyDbContext db)
        {
            if (db.Werkvormen.Where(a => a.Werkvorm == werkvorm).First() == null)
            {
                try
                {
                    db.Werkvormen.Add(new WerkvormModel { Werkvorm = werkvorm });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.Werkvormen.Where(a => a.Werkvorm == werkvorm).First();
        }

        /// <summary>
        /// Vindt het TijdsDuurModel dat bij tijdsduur hoort.
        /// Maakt een nieuw TijdsDuurModel in de database aan als deze niet gevonden kan worden.
        /// </summary>
        /// <param name="tijdsduur">De tijdsduur om te vinden</param>
        /// <param name="db">De database context om de tijdsduur in te vinden</param>
        /// <returns>Het gevonden of gemaakte TijdsDuurModel</returns>
        private TijdsDuurModel FindDuration(string tijdsduur, MyDbContext db)
        {
            TijdsDuurModel tdm;
            if((tdm = db.TijdsDuren.Where(a => a.Eenheid == tijdsduur).First()) == null)
            {
                tdm = MakeDuur(tijdsduur, db);
            }
            return tdm;
        }

        /// <summary>
        /// Maakt een TijdsDuurModel aan met tijdsduur
        /// </summary>
        /// <param name="tijdsduur">De tijdsduur om een TijdsDuurModel van te maken</param>
        /// <param name="db">De database context om het TijdsDuurModel in aan te maken</param>
        /// <returns>Het aangemaakte TijdsDuurModel</returns>
        private TijdsDuurModel MakeDuur(string tijdsduur, MyDbContext db)
        {
            if (db.TijdsDuren.Where(a => a.Eenheid == tijdsduur).First() == null)
            {
                try
                {
                    db.TijdsDuren.Add(new TijdsDuurModel { Eenheid = tijdsduur });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.TijdsDuren.Where(a => a.Eenheid == tijdsduur).First();
        }

        /// <summary>
        /// Vindt het NiveauModel dat bij de niveau parameter hoort.
        /// Maakt een nieuw NiveauModel in de database aan als deze niet gevonden kan worden.
        /// </summary>
        /// <param name="niveau">Het niveau om te vinden</param>
        /// <param name="db">De database context om het niveau in te vinden</param>
        /// <returns>Het gevonden of gemaakte NiveauModel</returns>
        private NiveauModel FindNiveau(string niveau, MyDbContext db)
        {
            NiveauModel nvm;
            if((nvm = db.Niveaus.Where(a => a.Niveau == niveau).First()) == null)
            {
                nvm = MakeNiveau(niveau, db);
            }
            return nvm;
        }

        /// <summary>
        /// Maakt een nieuw NiveauModel met niveau
        /// </summary>
        /// <param name="niveau">Het Niveau</param>
        /// <param name="db">De database context om het NiveauModel in aan te maken</param>
        /// <returns>Het aangemaakte NiveauModel</returns>
        private NiveauModel MakeNiveau(string niveau, MyDbContext db)
        {
            if (db.Niveaus.Where(a => a.Niveau == niveau).First() == null)
            {
                try
                {
                    db.Niveaus.Add(new NiveauModel { Niveau = niveau });
                    db.SaveChanges();
                }
                catch { }
            }
            return db.Niveaus.Where(a => a.Niveau == niveau).First();
        }

    }
}