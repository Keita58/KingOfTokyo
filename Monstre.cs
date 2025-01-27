using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    public class Monstre
    {
        public int MonstreID { get; set; }
        public string NomMonstre { get; set; }
        public int VidesMonstre { get; set; }
        public int PuntsVictoriaMonstre { get; set; }
        public int EnergiaMonstre { get; set; }
        public bool IsMonstrePoder { get; set; }
        public bool EstaTokyo { get; set; }
        public bool? Eliminat { get; set; }
        public Monstre IdMonstreAssociat { get; set; }
        public Jugador IdJugador { get; set; }

        public Monstre() {}

        public Monstre(string nomMonstre, bool monstrePoder, int energia)
        {
            NomMonstre = nomMonstre;
            IsMonstrePoder = monstrePoder;
            EnergiaMonstre = energia;
        }

        public Monstre(string nomMonstre, int videsMonstre, int puntsVictoriaMonstre, int energiaMonstre, bool monstrePoder, bool estaTokyo, bool? eliminat)
        {
            NomMonstre = nomMonstre;
            VidesMonstre = videsMonstre;
            PuntsVictoriaMonstre = puntsVictoriaMonstre;
            EnergiaMonstre = energiaMonstre;
            IsMonstrePoder = monstrePoder;
            EstaTokyo = estaTokyo;
            Eliminat = eliminat;
        }

        public override string ToString()
        {
            return "Nom del monstre: " + NomMonstre + ", vides actuals: " + VidesMonstre + ", punts de victòria: " + PuntsVictoriaMonstre + ", està a Tokyo? " + EstaTokyo +  ", energia del monstre: " + EnergiaMonstre + (IdMonstreAssociat != null ? ", monstre associat: " + IdMonstreAssociat.NomMonstre : "");
        }
    }
}
