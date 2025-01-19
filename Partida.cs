using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    public class Partida
    {
        public int PartidaID { get; set; }
        public int Torn {  get; set; }
        public int NumJugadors { get; set; }
        public bool PartidaFinalitzada { get; set; }
        public ICollection<Monstre> Monstres { get; set; }

        public Partida() 
        {
            Monstres = new HashSet<Monstre>();
        }

        public Partida(int torn, int numJugadors, bool partidaFinalitzada)
        {
            Torn = torn;
            NumJugadors = numJugadors;
            PartidaFinalitzada = partidaFinalitzada;
            Monstres = new HashSet<Monstre>();
        }

        public override string ToString()
        {
            return "Id Partida: " + PartidaID + ", torn actual: " + Torn + ", número de jugadors jugant: " + NumJugadors;
        }
    }
}
