namespace StudentenVolgSysteem.Migrations.MyDbContext
{
    using StudentenVolgSysteem.Controllers;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentenVolgSysteem.DAL.SVSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations\MyDbContext";
        }

        protected override void Seed(StudentenVolgSysteem.DAL.SVSContext context)
        {
            context.AanwezigheidOptions.Add(new Models.AanwezigheidOptionModel
            {
                Naam = "Aanwezig",
                IsDeleted = false
            });
            context.AanwezigheidOptions.Add(new Models.AanwezigheidOptionModel
            {
                Naam = "Afwezig",
                IsDeleted = false
            });
            context.AanwezigheidOptions.Add(new Models.AanwezigheidOptionModel
            {
                Naam = "Ziek",
                IsDeleted = false
            });
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //CSVController csv = new CSVController();
            //csv.GetTopicSheetData("~/csv_files/TopicDataInfra.csv");
        }
    }
}
