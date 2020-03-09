using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using StudentenVolgSysteem.Models;

namespace StudentenVolgSysteem.DAL
{
    public class SVSContext : DbContext
    {
        public SVSContext() : base(GetBase())
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

        public DbSet<Topic> Topics { get; set; }
        public DbSet<Niveau> Niveaus { get; set; }
        public DbSet<Werkvorm> Werkvormen { get; set; }  
        public DbSet<Tijdsduur> Tijdsduren { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Certificering> Certificeringen { get; set; }  
        public DbSet<Benodigdheid> Benodigdheden { get; set; }
        public DbSet<PercipioLink> PercipioLinks { get; set; }
        public DbSet<Curriculum> Curricula { get; set; }
        public DbSet<CurriculumTopic> CurriculumTopics { get; set; }
        public DbSet<Student> Studenten { get; set; }

        public DbSet<CurriculumTemplate> CurriculumTemplates { get; set; }
        public DbSet<AfwezigheidModel> Afwezigheden { get; set; }
        public DbSet<AfwezigheidOptionsModel> AfwezigheidOptions { get; set; }

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
            var props = typeof(T).GetProperties();
            IQueryable<T> query = this.Set<T>();
            query = props.Aggregate(query, (current, property) =>
            {
                if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string) && current != null)
                    return current.Include(property.Name).Where(p => !p.IsDeleted);
                return current;
            });           

            if (id == null) return default;
            var result = query.Where(t => t.Id == id).FirstOrDefault();
            if (result == null || result.IsDeleted) return default;
            return result;
        }
        /// <summary>
        /// Get Collection<T> from database with includes and filtered on IsDeleted
        /// </summary>
        /// <typeparam name="T">Input type</typeparam>
        /// <returns>Collection of type T</returns>
        public ICollection<T> GetFromDatabase<T>() where T : class,IDeletable
        {
            var props = typeof(T).GetProperties();
            IQueryable<T> query = this.Set<T>();
            try
            {
                query = props.Aggregate(query, (x, y) =>
                {
                    if (!y.PropertyType.IsValueType && y.PropertyType != typeof(string) && x != null)
                        return x.Include(y.Name.ToString());
                    return x;
                });
            } catch(Exception ex)
            {
                throw ex;
            }
            return query.Where(m => !m.IsDeleted).ToList();
        }


        //public IQueryable<TEntity> Including<TEntity>(params Func<TEntity, object>[] _includeProperties) where TEntity : class
        //{
        //    IQueryable<TEntity> query = this.Set<TEntity>();
        //    return _includeProperties.Aggregate(query, (current, includeProperty) => 
        //    {
        //        var include = includeProperty.ToString();
        //        return current.Include(include);

        //        });
        //}

        /// <summary>
        /// Get Collection<T> from database with includes and filtered on IsDeleted
        /// </summary>
        /// <typeparam name="T">Input type</typeparam>
        /// <param name="includes">Includes needed</param>
        /// <returns>Collection of type T</returns>
        //public ICollection<T> GetFromDatabase<T>() where T : class, IDeletable {
        //    //var includes = GetAllIncludes<T>();
        //    //IQueryable<T> query = this.Set<T>();
        //    //if (includes != null)
        //    //{
        //    //    query = includes.Aggregate(query, (current, includedProperty) => current.Include(includedProperty));
        //    //}
        //    var result = Includes<T>().Where(t => t.IsDeleted != true);
        //    //var result = query.Where(t => t.IsDeleted != true);
        //    return result.ToList();
        //}


        /*
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
                    try
                    {
                        return base.SaveChanges();
                    }

                    catch (DbUpdateConcurrencyException ex)
                    {
                        // TODO implement exception handler
                        return default;
                    }
                    catch (DbUpdateException ex)
                    {
                        // TODO implement exception handler
                        //             ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        return default;
                    }          
                }
        */
    }
    /// <summary>
    /// Standard Interface for softDeletes
    /// </summary>
    public interface IDeletable
    {
        int Id { get; set; }
        bool IsDeleted { get; set; }
    }
}