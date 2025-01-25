using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    [Table("Partida")]
    public class PartidaAnotacio
    {
        [Key]
        [Column("Partida_ID")]
        public int PartidaID { get; set; }
        public int Torn { get; set; }
        public int NumJugadors { get; set; }
        public bool PartidaFinalitzada { get; set; }
        public ICollection<MonstreAnotacio> MonstresPartides { get; set; }

        public PartidaAnotacio()
        {
            MonstresPartides = new HashSet<MonstreAnotacio>();
        }

        public PartidaAnotacio(int torn, int numJugadors, bool partidaFinalitzada)
        {
            Torn = torn;
            NumJugadors = numJugadors;
            PartidaFinalitzada = partidaFinalitzada;
            MonstresPartides = new HashSet<MonstreAnotacio>();
        }

        public override string ToString()
        {
            return "Id Partida: " + PartidaID + ", torn actual: " + Torn + ", número de jugadors jugant: " + NumJugadors;
        }
    }
}
