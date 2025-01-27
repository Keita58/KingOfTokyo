using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    public class Jugador
    {
        public int JugadorID { get; set; }
        public string NomJugador { get; set; }
        public string CognomsJugador { get; set; }
        public int PartidesGuanyades { get; set; }

        public Jugador() 
        {
        }

        public Jugador(string nom, string cognoms, int guanyades)
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
