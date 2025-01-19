using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
                Monstre king = new Monstre("King", 10, 0, 0, false, null);
                Monstre mekaDracron = new Monstre("MekaDracron", 10, 0, 0, false, null);
                Monstre ciberkitty = new Monstre("Ciberkitty", 10, 0, 0, false, null);
                Monstre gigazaur = new Monstre("Gigazaur", 10, 0, 0, false, null);
                Monstre penguin = new Monstre("Space Penguin", 10, 0, 0, false, null);
                Monstre alienoid = new Monstre("Alienoid", 10, 0, 0, false, null);

                Monstre aliento = new Monstre("Aliento Flamígero");
                Monstre mimetismo = new Monstre("Mimetismo");
                Monstre rayo = new Monstre("Monstruo con Rayo Reductor");
                Monstre escupidor = new Monstre("Monstruo Escupidor de Veneno");

                ctx.Monstres.Add(king);
                ctx.Monstres.Add(mekaDracron);
                ctx.Monstres.Add(ciberkitty);
                ctx.Monstres.Add(gigazaur);
                ctx.Monstres.Add(penguin);
                ctx.Monstres.Add(alienoid);

                ctx.Monstres.Add(aliento);
                ctx.Monstres.Add(mimetismo);
                ctx.Monstres.Add(rayo);
                ctx.Monstres.Add(escupidor);
                ctx.SaveChanges();

                int jugadorsPartida = 0;
                bool sortir = true;
                while(sortir)
                {
                    Console.WriteLine("Quants jugadors jugaran? (2-4)");
                    string jugadors = Console.ReadLine();

                    switch (jugadors)
                    {
                        case "2":
                            jugadorsPartida = 2;
                            sortir = false;
                            break;
                        case "3":
                            jugadorsPartida = 3;
                            sortir = false;
                            break;
                        case "4":
                            jugadorsPartida = 4;
                            sortir = false;
                            break;
                        default:
                            Console.WriteLine("Has de posar un número vàlid de jugadors :(");
                            break;
                    }
                }

                for (int i = 0; i < jugadorsPartida; i++)
                {
                    Jugador j = new Jugador("Jugador", Convert.ToString((i + 1)), 0);
                    ctx.Jugadors.Add(j);
                }
                ctx.SaveChanges();
            }
        }
    }
}
