using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (Context ctx = new Context())
            {
                Jugador j1 = new Jugador("Prova", "2", 0);
                Partida partida = new Partida(0, 1);
                //Monstre monstre = new Monstre("Monstre", 10, 0, 0, false, null);

                ctx.Jugadors.Add(j1);
                ctx.Partides.Add(partida);
                //ctx.Monstres.Add(monstre);
                ctx.SaveChanges();
            }
        }
    }
}
