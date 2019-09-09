namespace StudentenVolgSysteem.Migrations.MyDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CertificeringenInfraModels",
                c => new
                    {
                        CertificeringenInfraId = c.Int(nullable: false, identity: true),
                        Certificering = c.String(),
                    })
                .PrimaryKey(t => t.CertificeringenInfraId);
            
            CreateTable(
                "dbo.NiveauModels",
                c => new
                    {
                        NiveauId = c.Int(nullable: false, identity: true),
                        niveau = c.String(),
                    })
                .PrimaryKey(t => t.NiveauId);
            
            CreateTable(
                "dbo.TagModels",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        naam = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.TijdsDuurModels",
                c => new
                    {
                        TijdsDuurId = c.Int(nullable: false, identity: true),
                        Eenheid = c.String(),
                    })
                .PrimaryKey(t => t.TijdsDuurId);
            
            CreateTable(
                "dbo.WerkvormModels",
                c => new
                    {
                        WerkvormId = c.Int(nullable: false, identity: true),
                        Werkvorm = c.String(),
                    })
                .PrimaryKey(t => t.WerkvormId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WerkvormModels");
            DropTable("dbo.TijdsDuurModels");
            DropTable("dbo.TagModels");
            DropTable("dbo.NiveauModels");
            DropTable("dbo.CertificeringenInfraModels");
        }
    }
}
