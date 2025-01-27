using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo
{
    public static class MainFluent
    {
        static Random r = new Random();
        /*
        static void Main(string[] args)
        {
            using (ContextFluent ctx = new ContextFluent())
            {
                MonstreAnotacio king = new MonstreAnotacio("King", 10, 0, 0, false, false, null);
                MonstreAnotacio mekaDracron = new MonstreAnotacio("MekaDracron", 10, 0, 0, false, false, null);
                MonstreAnotacio ciberkitty = new MonstreAnotacio("Ciberkitty", 10, 0, 0, false, false, null);
                MonstreAnotacio gigazaur = new MonstreAnotacio("Gigazaur", 10, 0, 0, false, false, null);
                MonstreAnotacio penguin = new MonstreAnotacio("Space Penguin", 10, 0, 0, false, false, null);
                MonstreAnotacio alienoid = new MonstreAnotacio("Alienoid", 10, 0, 0, false, false, null);

                MonstreAnotacio aliento = new MonstreAnotacio("Aliento Flamígero", true, 3);
                MonstreAnotacio mimetismo = new MonstreAnotacio("Mimetismo", true, 8);
                MonstreAnotacio rayo = new MonstreAnotacio("Monstruo con Rayo Reductor", true, 6);
                MonstreAnotacio escupidor = new MonstreAnotacio("Monstruo Escupidor de Veneno", true, 4);

                //Monstres
                ctx.MonstresFluent.Add(king);
                ctx.MonstresFluent.Add(mekaDracron);
                ctx.MonstresFluent.Add(ciberkitty);
                ctx.MonstresFluent.Add(gigazaur);
                ctx.MonstresFluent.Add(penguin);
                ctx.MonstresFluent.Add(alienoid);

                //Monstres de poder
                ctx.MonstresFluent.Add(aliento);
                ctx.MonstresFluent.Add(mimetismo);
                ctx.MonstresFluent.Add(rayo);
                ctx.MonstresFluent.Add(escupidor);
                ctx.SaveChanges();

                int jugadorsPartida = r.Next(2, 5);
                PartidaAnotacio p = new PartidaAnotacio(0, jugadorsPartida, false);
                ctx.PartidesFluent.Add(p);
                ctx.SaveChanges();

                //Començar partida amb dades anteriors
                List<MonstreAnotacio> monstresPartida = ctx.MonstresFluent.Where(x => x.IsMonstrePoder == false).ToList();

                monstresPartida.Shuffle();

                for (int i = 0; i < jugadorsPartida; i++)
                {
                    JugadorAnotacio j = new JugadorAnotacio("Jugador", Convert.ToString((i + 1)), 0);

                    int num = r.Next(0, monstresPartida.Count);
                    j.MonstresInverse = monstresPartida[num];
                    monstresPartida[num].PartidesJugades = p;
                    monstresPartida[num].Jugador = j;
                    p.MonstresPartides.Add(monstresPartida[num]);
                    ctx.JugadorsFluent.Add(j);
                    ctx.SaveChanges();
                    monstresPartida.Remove(monstresPartida[num]);
                }


                PartidaAnotacio pActual = ctx.PartidesFluent.Where(x => x.PartidaFinalitzada == false).FirstOrDefault();

                bool continuar = true;

                //Partida en si
                while (continuar)
                {
                    List<JugadorAnotacio> jugadorsJugant = ctx.JugadorsFluent.ToList();
                    jugadorsJugant.Shuffle();

                    for (int i = jugadorsJugant.Count - 1; i >= 0; i--)
                    {
                        MonstreAnotacio aux = jugadorsJugant[i].MonstresInverse;
                        if (aux.VidesMonstre <= 0)
                        {
                            jugadorsJugant.RemoveAt(i);
                        }
                    }

                    foreach (JugadorAnotacio j in jugadorsJugant)
                    {
                        Dictionary<int, int> numDaus = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 } };

                        MonstreAnotacio monstre = j.MonstresInverse;
                        if (monstre.VidesMonstre > 0)
                        {
                            List<MonstreAnotacio> monstresEnemics = new List<MonstreAnotacio>();
                            foreach (JugadorAnotacio ju in jugadorsJugant)
                            {
                                if (ju != j)
                                {
                                    if (ju.MonstresInverse.VidesMonstre > 0)
                                    {
                                        monstresEnemics.Add(ju.MonstresInverse);
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
                                            List<MonstreAnotacio> monstresNoTokyo = monstresEnemics.Where(x => x.VidesMonstre > 0 && x.EstaTokyo == false).ToList();
                                            foreach (MonstreAnotacio m in monstresNoTokyo)
                                            {
                                                m.VidesMonstre--;
                                                Console.WriteLine("Menys 1 vida a " + m.NomMonstre);
                                                ctx.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            MonstreAnotacio tokyo = monstresEnemics.Where(x => x.VidesMonstre > 0 && x.EstaTokyo == true).First();
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
                                            foreach (MonstreAnotacio m in monstresEnemics)
                                            {
                                                m.VidesMonstre--;
                                                ctx.SaveChanges();
                                            }

                                            MonstreAnotacio tokyo = monstresEnemics.Where(x => x.EstaTokyo == true).FirstOrDefault();

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
                                            MonstreAnotacio monstreRandomMimetismo = monstresEnemics.FirstOrDefault();
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

                                            MonstreAnotacio tokyo2 = monstresEnemics.Where(x => x.EstaTokyo == true).FirstOrDefault();

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
                                List<MonstreAnotacio> mPoder = ctx.MonstresFluent.Where(x => x.IsMonstrePoder == true && x.IdMonstreAssociat == null).ToList();
                                mPoder.Shuffle();

                                int monstresDisponibles = mPoder.Count();

                                if (monstre.IdMonstreAssociat == null)
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

                            List<MonstreAnotacio> monstresVius = new List<MonstreAnotacio>();
                            foreach (JugadorAnotacio ju in jugadorsJugant)
                            {
                                if (ju.MonstresInverse.VidesMonstre > 0)
                                {
                                    monstresVius.Add(ju.MonstresInverse);
                                    Console.WriteLine(ju.MonstresInverse.ToString());
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
        }*/
    }
}
