using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstStoreFunctions;
using MovieUniverse.Abstract.Data;
using MovieUniverse.Abstract.DB;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.Abstract.Entities.UserEntityes;

namespace MovieUniverse.DB.DbContext
{
    public class MovieUniverseContext : System.Data.Entity.DbContext, IDbContext
    {

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Friend> Friends { get; set; }

        //public DbSet<MovieInvitation> MovieInvitations { get; set; }
        //public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Friend>()
                .HasRequired<AppUser>(x => x.User)
                .WithMany(x => x.Friends)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);
        }

        IDbSet<T> IDbContext.Set<T>()
        {
            throw new NotImplementedException();
        }

        public MovieUniverseContext() : base("MovieUniverseConnectionTest")
        {
            this.Configuration.LazyLoadingEnabled = false;

        }
    }
}
