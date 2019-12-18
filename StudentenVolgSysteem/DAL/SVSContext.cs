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

        public SVSContext() : base("name=DefaultConnection")
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<MyDbContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SVSContext>());
        }

        /// <summary>
        /// Returns a connection string based on current Visual Studio environment.
        /// </summary>
        /// <returns></returns>
        private static string GetBase()
        {
            string ret = "name=LiveConnection";
            #if (DEBUG)
                // Debug connection string
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

        /// <summary>
        /// Generic function to retrieve specific data from context, returns default if IsDeleted or null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetFromDatabase<T>(int? id) where T : class, IDeletable
        {
            DbSet<T> dbSet = this.Set<T>();
            if (id == null) return default;
            var result = dbSet.Find(id);
            if (result == null || result.IsDeleted) return default;

            return result;
        }

        /// <summary>
        /// Marks any "Removed" Entities as "Modified" and then sets the Db [IsDeleted] Flag to true
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            // If an entity is marked for deletion add to markedAsDeleted
            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                // if entity implements IDeletable interface
                if (item.Entity is IDeletable entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets send to Db as an update.. null values
                    item.State = EntityState.Unchanged;
                    // Only update IsDeleted flag.
                    entity.IsDeleted = true;
                }
            }
            return base.SaveChanges();
        }
    }

    /// <summary>
    /// Standard Interface for softDeletes
    /// </summary>
    public interface IDeletable
    {
        bool IsDeleted { get; set; }
    }
}