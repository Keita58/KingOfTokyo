using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    [Table("Monstre")]
    public class MonstreAnotacio
    {
        [Key]
        public int MonstreID { get; set; }

        [Required]
        [StringLength(50)]
        public string NomMonstre { get; set; }
        public int VidesMonstre { get; set; }
        public int PuntsVictoriaMonstre { get; set; }
        public int EnergiaMonstre { get; set; }
        public bool IsMonstrePoder { get; set; }
        public bool EstaTokyo { get; set; }
        public bool? Eliminat { get; set; }
        public MonstreAnotacio IdMonstreAssociat { get; set; }
        public JugadorAnotacio Jugador { get; set; }

        //[NotMapped] //No es guarda a la base de dades
        [InverseProperty("MonstresPartides")] //Relació amb la variable Monstres de la classe PartidaAnotacio
        public PartidaAnotacio PartidesJugades { get; set; }

        public MonstreAnotacio() { }

        public MonstreAnotacio(string nomMonstre, bool monstrePoder, int energia)
        {
            NomMonstre = nomMonstre;
            IsMonstrePoder = monstrePoder;
            EnergiaMonstre = energia;
        }

        public MonstreAnotacio(string nomMonstre, int videsMonstre, int puntsVictoriaMonstre, int energiaMonstre, bool monstrePoder, bool estaTokyo, bool? eliminat)
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
            return "Nom del monstre: " + NomMonstre + ", vides actuals: " + VidesMonstre + ", punts de victòria: " + PuntsVictoriaMonstre + ", està a Tokyo? " + EstaTokyo + ", energia del monstre: " + EnergiaMonstre + (IdMonstreAssociat != null ? ", monstre associat: " + IdMonstreAssociat.NomMonstre : "");
        }
    }
}
