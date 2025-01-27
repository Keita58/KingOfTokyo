using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    public static class Program
    {
        static Random r = new Random();
        
        static void Main(string[] args)
        {
            using (Context ctx = new Context())
            {
                Monstre king = new Monstre("King", 10, 0, 0, false, false, null);
                Monstre mekaDracron = new Monstre("MekaDracron", 10, 0, 0, false, false, null);
                Monstre ciberkitty = new Monstre("Ciberkitty", 10, 0, 0, false, false, null);
                Monstre gigazaur = new Monstre("Gigazaur", 10, 0, 0, false, false, null);
                Monstre penguin = new Monstre("Space Penguin", 10, 0, 0, false, false, null);
                Monstre alienoid = new Monstre("Alienoid", 10, 0, 0, false, false, null);

                Monstre aliento = new Monstre("Aliento Flamígero", true, 3);
                Monstre mimetismo = new Monstre("Mimetismo", true, 8);
                Monstre rayo = new Monstre("Monstruo con Rayo Reductor", true, 6);
                Monstre escupidor = new Monstre("Monstruo Escupidor de Veneno", true, 4);

                //Monstres
                ctx.Monstres.Add(king);
                ctx.Monstres.Add(mekaDracron);
                ctx.Monstres.Add(ciberkitty);
                ctx.Monstres.Add(gigazaur);
                ctx.Monstres.Add(penguin);
                ctx.Monstres.Add(alienoid);

                //Monstres de poder
                ctx.Monstres.Add(aliento);
                ctx.Monstres.Add(mimetismo);
                ctx.Monstres.Add(rayo);
                ctx.Monstres.Add(escupidor);
                ctx.SaveChanges();

                int jugadorsPartida = r.Next(2, 5);
                Partida p = new Partida(0, jugadorsPartida, false);
                ctx.Partides.Add(p);
                ctx.SaveChanges();

                for (int i = 0; i < jugadorsPartida; i++)
                {
                    Jugador j = new Jugador("Jugador", Convert.ToString((i + 1)), 0);
                    ctx.Jugadors.Add(j);
                }
                ctx.SaveChanges();

                //Començar partida amb dades anteriors
                List<Monstre> monstresPartida = ctx.Monstres.Where(x => x.IsMonstrePoder == false).ToList();
                List<Jugador> jPartida = ctx.Jugadors.ToList();

                monstresPartida.Shuffle();
                jPartida.Shuffle();

                for (int i = 0; i < jugadorsPartida; i++)
                {
                    int num = r.Next(0, monstresPartida.Count);
                    p.Monstres.Add(monstresPartida[num]);
                    monstresPartida[num].IdJugador = jPartida[i];
                    monstresPartida.Remove(monstresPartida[num]);
                    ctx.SaveChanges();
                }

                Partida pActual = ctx.Partides.Where(x => x.PartidaFinalitzada == false).FirstOrDefault();

                bool continuar = true;

                //Partida en si
                while (continuar)
                {
                    List<Jugador> jugadorsJugant = ctx.Jugadors.ToList();
                    jugadorsJugant.Shuffle();

                    List<Monstre> auxMonstres = ctx.Monstres.ToList();

                    for (int i = jugadorsJugant.Count - 1; i >= 0; i--)
                    {
                        Monstre aux = auxMonstres.Where(x => x.IdJugador != null && x.IdJugador.Equals(jugadorsJugant[i])).First();
                        if (aux.VidesMonstre <= 0)
                        {
                            jugadorsJugant.RemoveAt(i);
                        }
                    }

                    foreach (Jugador j in jugadorsJugant)
                    {
                        Dictionary<int, int> numDaus = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 } };

                        Monstre monstre = auxMonstres.Where(x => x.IdJugador != null && x.IdJugador.Equals(j)).First();
                        if (monstre.VidesMonstre > 0) 
                        {
                            List<Monstre> monstresEnemics = new List<Monstre>();
                            foreach (Jugador ju in jugadorsJugant)
                            {
                                if (ju != j)
                                {
                                    Monstre aux = auxMonstres.Where(x => x.IdJugador != null && x.IdJugador.Equals(ju)).First();
                                    if (aux.VidesMonstre > 0)
                                    {
                                        monstresEnemics.Add(aux);
                                    }
                                }
                            }

                            Console.WriteLine();
                            Console.WriteLine("Torn del jugador " + j.NomJugador + " " + j.CognomsJugador + " amb el monstre " + monstre.NomMonstre + " (Torn " + pActual.Torn + ")");
                            if (pActual.Torn == 0)
                            {
                                monstre.EstaTokyo = true;
                                ctx.SaveChanges();
                            }

                            //Tirada dels daus del jugador
                            int[] daus = new int[6];
                            for (int i = 0; i < 6; i++)
                            {
                                daus[i] = r.Next(1, 7);
                                Console.WriteLine("El dau " + (i + 1) + " ha sortir amb el valor " + daus[i]);

                                switch (daus[i])
                                {
                                    case 1:
                                        numDaus[1]++;
                                        break;
                                    case 2:
                                        numDaus[2]++;
                                        break;
                                    case 3:
                                        numDaus[3]++;
                                        break;
                                    case 4:
                                        monstre.EnergiaMonstre++;
                                        Console.WriteLine("El monstre ha guanyat un punt d'energia");
                                        ctx.SaveChanges();
                                        break;
                                    case 5:
                                        //Peta aquí quan es tiren dos 5 seguits o quan es canvia de monstre a Tokyo
                                        if (monstre.EstaTokyo)
                                        {
                                            List<Monstre> monstresNoTokyo = monstresEnemics.Where(x => x.VidesMonstre > 0 && x.EstaTokyo == false).ToList();
                                            foreach (Monstre m in monstresNoTokyo)
                                            {
                                                m.VidesMonstre--;
                                                Console.WriteLine("Menys 1 vida a " + m.NomMonstre);
                                                ctx.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            Monstre tokyo = monstresEnemics.Where(x => x.VidesMonstre > 0 && x.EstaTokyo == true).First();
                                            tokyo.VidesMonstre--;
                                            Console.WriteLine("He fet mal al monstre " + tokyo.NomMonstre + " que està a Tokyo");
                                            ctx.SaveChanges();

                                            int intercanvi = r.Next(1, 3);
                                            if (tokyo.VidesMonstre <= 0)
                                            {
                                                tokyo.EstaTokyo = false;
                                                monstre.EstaTokyo = true;
                                                ctx.SaveChanges();
                                                Console.WriteLine("El monstre " + monstre.NomMonstre + " ara és a Tokyo. Abans hi era: " + tokyo.NomMonstre + " i ha mort :(");
                                            }
                                            else if (intercanvi == 1)
                                            {
                                                tokyo.EstaTokyo = false;
                                                monstre.EstaTokyo = true;
                                                ctx.SaveChanges();
                                                Console.WriteLine("El monstre " + monstre.NomMonstre + " ara és a Tokyo. Abans hi era: " + tokyo.NomMonstre);
                                            }
                                            else
                                                Console.WriteLine("El monstre ha fet mal al monstre de Tokyo i ha decidit no canviar.");
                                        }
                                        break;
                                    case 6:
                                        if (monstre.VidesMonstre < 10)
                                        {
                                            monstre.VidesMonstre++;
                                            Console.WriteLine("Monstre del " + j.NomJugador + " " + j.CognomsJugador + " guanya 1 vida");
                                            ctx.SaveChanges();
                                        }
                                        else
                                        {
                                            Console.WriteLine("No pot superar els 10 punts de vida!");
                                        }
                                        break;
                                }
                            }

                            if (numDaus[1] >= 3)
                            {
                                monstre.PuntsVictoriaMonstre++;
                                Console.WriteLine("El monstre ha guanyat 1 punt de victòria al tenir tres 1 iguals");
                                numDaus[1] -= 3;
                                while (numDaus[1] > 0)
                                {
                                    monstre.PuntsVictoriaMonstre++;
                                    Console.WriteLine("Tenia un altre número igual! +1 punt de victòria");
                                    numDaus[1]--;
                                }
                                ctx.SaveChanges();
                            }

                            if (numDaus[2] >= 3)
                            {
                                monstre.PuntsVictoriaMonstre += 2;
                                Console.WriteLine("El monstre ha guanyat 2 punts de victòria al tenir tres 2 iguals");
                                numDaus[2] -= 3;
                                while (numDaus[2] > 0)
                                {
                                    monstre.PuntsVictoriaMonstre++;
                                    Console.WriteLine("Tenia un altre número igual! +1 punt de victòria");
                                    numDaus[2]--;
                                }
                                ctx.SaveChanges();
                            }

                            if (numDaus[3] >= 3)
                            {
                                monstre.PuntsVictoriaMonstre += 3;
                                Console.WriteLine("El monstre ha guanyat 3 punts de victòria al tenir tres 3 iguals");
                                numDaus[3] -= 3;
                                while (numDaus[3] > 0)
                                {
                                    monstre.PuntsVictoriaMonstre++;
                                    Console.WriteLine("Tenia un altre número igual! +1 punt de victòria");
                                    numDaus[3]--;
                                }
                                ctx.SaveChanges();
                            }

                            if (monstre.IdMonstreAssociat != null && monstre.EnergiaMonstre >= monstre.IdMonstreAssociat.EnergiaMonstre)
                            {
                                int tirar = r.Next(1, 3);
                                if (tirar == 1)
                                {
                                    monstresEnemics.Shuffle();
                                    switch (monstre.IdMonstreAssociat.NomMonstre)
                                    {
                                        case "Aliento Flamígero":
                                            Console.WriteLine("Jugador fa servir Aliento Flamígero!");
                                            foreach (Monstre m in monstresEnemics)
                                            {
                                                m.VidesMonstre--;
                                                ctx.SaveChanges();
                                            }

                                            Monstre tokyo = monstresEnemics.Where(x => x.EstaTokyo == true).FirstOrDefault();

                                            if (tokyo != null && tokyo.VidesMonstre <= 0)
                                            {
                                                tokyo.EstaTokyo = false;
                                                monstre.EstaTokyo = true;
                                                ctx.SaveChanges();
                                                Console.WriteLine("El monstre " + monstre.NomMonstre + " ara és a Tokyo. Abans hi era: " + tokyo.NomMonstre);
                                            }

                                            monstre.EnergiaMonstre -= monstre.IdMonstreAssociat.EnergiaMonstre;

                                            monstre.IdMonstreAssociat.IdMonstreAssociat = null;
                                            monstre.IdMonstreAssociat = null;

                                            ctx.SaveChanges();
                                            break;
                                        case "Mimetismo":
                                            Console.WriteLine("Jugador fa servir Aliento Mimetismo!");
                                            Monstre monstreRandomMimetismo = monstresEnemics.FirstOrDefault();
                                            int aux = monstre.VidesMonstre;
                                            monstre.VidesMonstre = monstreRandomMimetismo.VidesMonstre;
                                            monstreRandomMimetismo.VidesMonstre = aux;
                                            monstre.EnergiaMonstre -= monstre.IdMonstreAssociat.EnergiaMonstre;

                                            monstre.IdMonstreAssociat.IdMonstreAssociat = null;
                                            monstre.IdMonstreAssociat = null;

                                            ctx.SaveChanges();
                                            break;
                                        case "Monstruo con Rayo Reductor":
                                            Console.WriteLine("Jugador fa servir Monstruo con Rayo Reductor!");
                                            monstresEnemics.Shuffle();
                                            monstresEnemics.First().VidesMonstre--;

                                            Monstre tokyo2 = monstresEnemics.Where(x => x.EstaTokyo == true).FirstOrDefault();

                                            if (tokyo2 != null && tokyo2.VidesMonstre <= 0)
                                            {
                                                tokyo2.EstaTokyo = false;
                                                monstre.EstaTokyo = true;
                                                ctx.SaveChanges();
                                                Console.WriteLine("El monstre " + monstre.NomMonstre + " ara és a Tokyo. Abans hi era: " + tokyo2.NomMonstre);
                                            }

                                            monstre.EnergiaMonstre -= monstre.IdMonstreAssociat.EnergiaMonstre;

                                            monstre.IdMonstreAssociat.IdMonstreAssociat = null;
                                            monstre.IdMonstreAssociat = null;

                                            ctx.SaveChanges();
                                            break;
                                        case "Monstruo Escupidor de Veneno":
                                            Console.WriteLine("Jugador fa servir Monstruo con Rayo Reductor!");
                                            monstresEnemics.Shuffle();
                                            if (monstresEnemics.First().PuntsVictoriaMonstre > 0)
                                                monstresEnemics.First().PuntsVictoriaMonstre--;
                                            monstre.EnergiaMonstre -= monstre.IdMonstreAssociat.EnergiaMonstre;

                                            monstre.IdMonstreAssociat.IdMonstreAssociat = null;
                                            monstre.IdMonstreAssociat = null;

                                            ctx.SaveChanges();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                List<Monstre> mPoder = ctx.Monstres.Where(x => x.IsMonstrePoder == true && x.IdMonstreAssociat == null).ToList();
                                mPoder.Shuffle();

                                int monstresDisponibles = mPoder.Count();

                                if(monstre.IdMonstreAssociat == null)
                                {
                                    for (int i = 0; i < monstresDisponibles; i++)
                                    {
                                        if (monstre.EnergiaMonstre >= mPoder[i].EnergiaMonstre)
                                        {
                                            monstre.EnergiaMonstre -= mPoder[i].EnergiaMonstre;
                                            monstre.IdMonstreAssociat = mPoder[i];
                                            mPoder[i].IdMonstreAssociat = monstre;
                                            Console.WriteLine("El jugador " + j.NomJugador + " " + j.CognomsJugador + " ha comprat el monstre amb poder " + mPoder[i].NomMonstre);
                                            break;
                                        }
                                    }
                                    ctx.SaveChanges();
                                }
                                
                            }

                            List<Monstre> monstresVius = new List<Monstre>();
                            foreach (Jugador ju in jugadorsJugant)
                            {
                                Monstre aux = auxMonstres.Where(x => x.IdJugador != null && x.IdJugador.Equals(ju)).First();
                                if (aux.VidesMonstre > 0)
                                {
                                    monstresVius.Add(aux);
                                    Console.WriteLine(aux.ToString());
                                }
                            }

                            if (monstresVius.Count == 1)
                            {
                                Console.WriteLine("Ha guanyat el jugador " + j.NomJugador + " " + j.CognomsJugador);
                                j.PartidesGuanyades++;
                                continuar = false;
                                ctx.SaveChanges();
                                break;
                            }
                            else
                            {
                                pActual.Torn++;
                                ctx.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
