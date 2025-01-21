using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    public class Context : DbContext
    {
        public Context():base("tokio") { }

        public DbSet<Partida> Partides { get; set; }
        public DbSet<Monstre> Monstres { get; set; }
        public DbSet<Jugador> Jugadors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<Context>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
