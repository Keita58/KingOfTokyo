using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    [Table("Jugador")]
    public class JugadorAnotacio
    {
        [Key]
        [Column("Jugador_ID")]
        public int JugadorID { get; set; }

        [Required]
        [StringLength(30)]
        public string NomJugador { get; set; }
        public string CognomsJugador { get; set; }
        public int PartidesGuanyades { get; set; }
        public MonstreAnotacio MonstresInverse { get; set; }

        public JugadorAnotacio()
        {
        }

        public JugadorAnotacio(string nom, string cognoms, int guanyades)
        {
            NomJugador = nom;
            CognomsJugador = cognoms;
            PartidesGuanyades = guanyades;
        }

        public override string ToString()
        {
            return "Nom jugador: " + NomJugador + " " + CognomsJugador + ", partides guanyades: " + PartidesGuanyades;
        }
    }
}
