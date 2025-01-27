using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    public class ContextFluent : DbContext
    {
        public ContextFluent() : base("tokio2") { }

        public DbSet<PartidaAnotacio> PartidesFluent { get; set; }
        public DbSet<MonstreAnotacio> MonstresFluent { get; set; }
        public DbSet<JugadorAnotacio> JugadorsFluent { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ContextFluent>());
            base.OnModelCreating(modelBuilder);

            /**
             * Many to many en Fluent
             * 
             * modelBuilder.Entity<JugadorFluent>()
                .HasMany<VideojocFluent>(s => s.Videojocs)
                .WithMany(c => c.JugadorsFluents)
                .Map(cs =>
                {
                    cs.MapLeftKey("Jugador_ID");
                    cs.MapRightKey("Videogame_ID");
                    cs.ToTable("JugadorVideojoc");
                });

             * 
             */

            modelBuilder.Entity<PartidaAnotacio>()
                .Property(v => v.NumJugadors)
                .HasColumnName("JugadorsPartida");
                //.HasPrecision(9, 2); Això només per camps float o double

            modelBuilder.Entity<MonstreAnotacio>().ToTable("MonstresJoc"); //Canvia el nom de la taula, encara que tingui una anotació canviant-lo també

            modelBuilder.Entity<MonstreAnotacio>()
            .HasOptional(a => a.Jugador) // Relació opcional (Jugador pot ser null)
            .WithRequired(p => p.MonstresInverse) // El Jugador no pot existir sense una relació amb monstre
            .Map(m => m.MapKey("MonstreID")) //Amb això no cal que posem l'anotació ForeignKey
            /* Per vagancia poso aquest cascade perquè en el programa
            no borro l'adressa abans de borrar la persona
            */.WillCascadeOnDelete(true);

        }
    }
}
