namespace StudentenVolgSysteem.Migrations.MyDbContext
{
    using StudentenVolgSysteem.Controllers;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentenVolgSysteem.Models.MyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\MyDbContext";
        }

        protected override void Seed(StudentenVolgSysteem.Models.MyDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            CSVController csv = new CSVController();
            csv.GetTopicSheetData("~/csv_files/TopicDataInfra.csv");
        }
    }
}
