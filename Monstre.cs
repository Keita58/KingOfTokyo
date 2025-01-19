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
        public bool EstaTokyo { get; set; }
        public bool? Eliminat { get; set; }
        public Monstre IdMonstreAssociat { get; set; }

        public Monstre() {}

        public Monstre(string nomMonstre, int videsMonstre, int puntsVictoriaMonstre, int energiaMonstre, bool estaTokyo, bool? eliminat)
        {
            NomMonstre = nomMonstre;
            VidesMonstre = videsMonstre;
            PuntsVictoriaMonstre = puntsVictoriaMonstre;
            EnergiaMonstre = energiaMonstre;
            EstaTokyo = estaTokyo;
            Eliminat = eliminat;
        }
    }
}
