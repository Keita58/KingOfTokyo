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
                Monstre king = new Monstre("King", 10, 0, 0, false, false, null);
                Monstre mekaDracron = new Monstre("MekaDracron", 10, 0, 0, false, false, null);
                Monstre ciberkitty = new Monstre("Ciberkitty", 10, 0, 0, false,  false, null);
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
                List<Monstre> monstresPartida = ctx.Monstres.ToList();
                List<Jugador> jPartida = ctx.Jugadors.ToList();

                Random r = new Random();
                monstresPartida = monstresPartida.OrderBy(_ => r.Next()).ToList();
                jPartida = jPartida.OrderBy(_ => r.Next()).ToList();

                for (int i = 0; i < 4; i++)
                {
                    int num = r.Next(0, monstresPartida.Count);
                    jPartida[i].Monstres.Add(monstresPartida[num]);
                    p.Monstres.Add(monstresPartida[num]);
                    monstresPartida.Remove(monstresPartida[num]);
                    ctx.SaveChanges();
                }

                Partida pActual = ctx.Partides.Where(x => x.PartidaFinalitzada == false).FirstOrDefault();

                bool continuar = true;

                //Partida en si
                while (continuar)
                {
                    Dictionary<int, int> numDaus = new Dictionary<int, int>();
                    
                    List<Jugador> jugadorsJugant = ctx.Jugadors.OrderBy(_ => r.Next()).ToList();

                    for (int i = jugadorsJugant.Count - 1; i >= 0; i--)
                    {
                        Monstre aux = jugadorsJugant[i].Monstres.Where(x => x.IsMonstrePoder == false && x.VidesMonstre <= 0).First();
                        if (aux == null)
                        {
                            jugadorsJugant.RemoveAt(i);
                        }
                    }

                    foreach (Jugador j in jugadorsJugant)
                    {
                        Monstre monstre = j.Monstres.Where(x => x.IsMonstrePoder == false && x.VidesMonstre > 0).FirstOrDefault();
                        List<Monstre> monstresEnemics = new List<Monstre>();
                        foreach (Jugador ju in jugadorsJugant)
                        {
                            if (ju != j)
                            {
                                monstresEnemics.Add(ju.Monstres.Where(x => x.IsMonstrePoder == false && x.VidesMonstre > 0).First());
                            }
                        }

                        Console.WriteLine("Torn del jugador" + j.NomJugador + " (Torn " + pActual.Torn + ")");
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
                                    numDaus.Add(1, daus[i] + numDaus[1]);
                                    break;
                                case 2:
                                    numDaus.Add(2, daus[i] + numDaus[2]);
                                    break;
                                case 3:
                                    numDaus.Add(3, daus[i] + numDaus[3]);
                                    break;
                                case 4:
                                    monstre.EnergiaMonstre++;
                                    ctx.SaveChanges();
                                    break;
                                case 5:
                                    if (monstre.EstaTokyo)
                                    {
                                        List<Monstre> monstresNoTokyo = monstresEnemics.Where(x => x.EstaTokyo == false).ToList();
                                        foreach (Monstre m in monstresNoTokyo)
                                        {
                                            m.VidesMonstre --;
                                            ctx.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        Monstre tokyo = monstresEnemics.Where(x => x.EstaTokyo == true).First();
                                        tokyo.VidesMonstre--;
                                        ctx.SaveChanges();

                                        int intercanvi = r.Next(1, 3);
                                        if (intercanvi == 1)
                                        {
                                            tokyo.EstaTokyo = false;
                                            monstre.EstaTokyo = true;
                                            ctx.SaveChanges();
                                            Console.WriteLine("El monstre " + monstre.NomMonstre + " ara és a Tokyo. Abans hi era: "+tokyo.NomMonstre);
                                        }
                                    }
                                    break;
                                case 6:
                                    if(monstre.VidesMonstre < 10)
                                        monstre.VidesMonstre++;
                                    ctx.SaveChanges();
                                    break;
                            }
                        }

                        if (daus[1] >= 3)
                        {
                            monstre.PuntsVictoriaMonstre++;
                            daus[1] -= 3;
                            while(daus[1] > 0)
                            {
                                monstre.PuntsVictoriaMonstre++;
                            }
                        }

                        if (daus[2] >= 3)
                        {
                            monstre.PuntsVictoriaMonstre += 2;
                            daus[2] -= 3;
                            while (daus[2] > 0)
                            {
                                monstre.PuntsVictoriaMonstre++;
                            }
                        }

                        if (daus[3] >= 3)
                        {
                            monstre.PuntsVictoriaMonstre += 3;
                            daus[3] -= 3;
                            while (daus[3] > 0)
                            {
                                monstre.PuntsVictoriaMonstre++;
                            }
                        }

                        if (monstre.IdMonstreAssociat != null && monstre.EnergiaMonstre >= monstre.IdMonstreAssociat.EnergiaMonstre)
                        {
                            int tirar = r.Next(1, 3);
                            if (tirar == 1)
                            {
                                switch (monstre.IdMonstreAssociat.NomMonstre)
                                {
                                    case "Aliento Flamígero":
                                        foreach (Monstre m in monstresEnemics)
                                        {
                                            m.VidesMonstre --;
                                            ctx.SaveChanges();
                                        }

                                        monstre.EnergiaMonstre -= monstre.IdMonstreAssociat.EnergiaMonstre;
                                        
                                        monstre.IdMonstreAssociat.IdMonstreAssociat = null;
                                        monstre.IdMonstreAssociat = null;

                                        ctx.SaveChanges();
                                        break;
                                    case "Mimetismo":
                                        Monstre monstreRandomMimetismo = monstresEnemics.OrderBy(_ => r.Next()).FirstOrDefault();
                                        int aux=monstre.VidesMonstre;
                                        monstre.VidesMonstre = monstreRandomMimetismo.VidesMonstre;
                                        monstreRandomMimetismo.VidesMonstre = aux;
                                        monstre.EnergiaMonstre -= monstre.IdMonstreAssociat.EnergiaMonstre;

                                        monstre.IdMonstreAssociat.IdMonstreAssociat = null;
                                        monstre.IdMonstreAssociat = null;

                                        ctx.SaveChanges();
                                        break;
                                    case "Monstruo con Rayo Reductor":
                                        Monstre monstreRandomRayo = monstresEnemics.OrderBy(_ => r.Next()).FirstOrDefault();
                                        monstreRandomRayo.VidesMonstre--;
                                        monstre.EnergiaMonstre -= monstre.IdMonstreAssociat.EnergiaMonstre;

                                        monstre.IdMonstreAssociat.IdMonstreAssociat = null;
                                        monstre.IdMonstreAssociat = null;

                                        ctx.SaveChanges();
                                        break;
                                    case "Monstruo Escupidor de Veneno":
                                        Monstre monstreRandomVeneno = monstresEnemics.OrderBy(_ => r.Next()).FirstOrDefault();
                                        monstreRandomVeneno.PuntsVictoriaMonstre--;
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
                            List<Monstre> mPoder = ctx.Monstres.Where(x => x.IsMonstrePoder == true && x.IdMonstreAssociat == null).OrderBy(_ => r.Next()).ToList();

                            int monstresDisponibles = mPoder.Count();

                            for(int i = 0; i < monstresDisponibles; i++)
                            {
                                if(monstre.EnergiaMonstre >= mPoder[i].EnergiaMonstre)
                                {
                                    monstre.EnergiaMonstre -= mPoder[i].EnergiaMonstre;
                                    monstre.IdMonstreAssociat = mPoder[i];
                                    mPoder[i].IdMonstreAssociat = monstre;
                                }
                            }
                        }

                        List<Monstre> monstresVius = new List<Monstre>();
                        foreach (Jugador ju in jugadorsJugant)
                        {
                            if (ju != j)
                            {
                                monstresVius.Add(ju.Monstres.Where(x => x.IsMonstrePoder == false && x.VidesMonstre > 0).First());
                            }
                        }

                        if (monstresVius.Count == 0)
                        {
                            j.PartidesGuanyades++;
                            continuar = false;
                            break;
                        }
                        else
                            pActual.Torn++;
                    }
                }
            }
        }
    }
}
