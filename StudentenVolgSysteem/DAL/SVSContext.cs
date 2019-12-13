using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using StudentenVolgSysteem.Models;

namespace StudentenVolgSysteem.DAL
{
    public class SVSContext : DbContext
    {
        public SVSContext() : base(GetBase())
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<MyDbContext>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MyDbContext>());
        }

        /// <summary>
        /// Returns a connection string based on current Visual Studio environment.
        /// </summary>
        /// <returns></returns>
        private static string GetBase()
        {
            string ret = "name=LiveConnection";
#if (DEBUG)
            ret = "name=DefaultConnection";
#endif
            return (ret);
        }

        public DbSet<Werkvorm> Werkvormen { get; set; }
        public DbSet<Niveau> Niveaus { get; set; }
        public DbSet<Tijdsduur> Tijdsduren { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Certificering> Certificeringen { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Benodigdheid> Benodigdheden { get; set; }
        public DbSet<PercipioLink> PercipioLinks { get; set; }
        public DbSet<Student> Studenten { get; set; }
        public DbSet<Curriculum> Curricula { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            modelBuilder.Entity<Topic>().HasMany(m => m.Voorkennis).WithMany();
        }
    }
}