using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeWPF
{
    public class Computador
    {
        private List<Jugada> jugadas;
        private Jugada actual;
        private int movActual;
        private List<string> lineas;
        private string tipoJugada;

        public Computador()
        {
            Jugadas = new List<Jugada>();
            actual = null;
            movActual = 0;
            tipoJugada = "C";
            lineas = new List<string>();

            lineas.Add("00 01 02");
            lineas.Add("10 11 12");
            lineas.Add("20 21 22");
            lineas.Add("00 10 20");
            lineas.Add("01 11 21");
            lineas.Add("02 12 22");
            lineas.Add("00 11 22");
            lineas.Add("20 11 02");

            LeerJugadas();
        }

        /// <summary>
        /// Lee todas las jugadas desde la base de conocimientos (jugadas.txt)
        /// </summary>
        public void LeerJugadas()
        {
            string[] lines = File.ReadAllLines(@"C:\Users\Sefirot\Documents\Visual Studio 2015\Projects\IATaller2\TicTacToeWPF\bin\Debug\jugadas.txt");
            foreach (string line in lines)
            {
                Jugada jugada = new Jugada(line);
                Debug.WriteLine(jugada);
                Jugadas.Add(new Jugada(line));
            }
        }

        public List<Jugada> Jugadas
        {
            get { return jugadas; }
            set { jugadas = value; }
        }

        /// <summary>
        /// Realiza un movimiento en el tablero.
        /// </summary>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        public string Jugar(List<string> movimientos)
        {

            Jugada j = new Jugada(movimientos);
            Debug.WriteLine("Jugada actual");
            Debug.WriteLine(j);
            string mov = EnLinea(j, "C");
            Debug.WriteLine("Mov C:" + mov);

            if (mov != "")
            {
                Debug.WriteLine("En linea C");
                return mov;
            }

            mov = EnLinea(j, "J");
            Debug.WriteLine("Mov J:" + mov);

            if (mov != "") {
                Debug.WriteLine("En linea J");
                return mov;
            }

            if (actual != null) {
                Debug.WriteLine("Jugada existente");
                if (tipoJugada == "C")
                {
                    Debug.WriteLine("Copia jugada computador");
                    if (Igual(movimientos, actual.Movimientos))
                    {
                        Debug.WriteLine("Copia jugada computador existente");
                        return RealizarMov(actual.MovimientosC[movActual]);
                    } else
                    {
                        List<Jugada> js = new List<Jugada>();
                        // seleccionamos las jugadas con ganador computador.
                        foreach (Jugada jugada in Jugadas)
                        {
                            if (Igual(movimientos, jugada.Movimientos))
                            {
                                Debug.WriteLine("Nueva Copia jugada computador");
                                js.Add(jugada);
                            }
                        }

                        // si hay jugadas, seleccionamos una.
                        if (js.Count > 0)
                        {
                            // Actualizamos las jugadas.
                            Jugadas = js;
                            Debug.WriteLine("Nueva Jugada computador");
                            actual = SeleccionRandom(js);
                            return RealizarMov(actual.MovimientosC[movActual]);
                        } else
                        {
                            Debug.WriteLine("Copia jugada computador random");
                            return RealizarMov(JugadaRandom(movimientos));
                        }
                    }
                }
                else if (tipoJugada == "J")
                {
                    Debug.WriteLine("Copia jugada jugador");
                    if (PosicionVacia(actual.MovimientosJ[movActual], movimientos))
                    {
                        Debug.WriteLine("Copia jugada jugador existente");
                        return RealizarMov(actual.MovimientosJ[movActual]);

                    }
                    else
                    {
                        Debug.WriteLine("Copia jugada jugador Random");
                        return RealizarMov(JugadaRandom(movimientos));
                    }
                }
            } else if (actual == null && Jugadas.Count > 0)
            {
                Debug.WriteLine("Jugada inicial");
                mov = "";
                if (tipoJugada == "C")
                {
                    List<Jugada> js = new List<Jugada>();
                    // seleccionamos las jugadas con ganador computador.
                    foreach (Jugada jugada in Jugadas)
                    {
                        if (!PosicionVacia(jugada.Movimientos[0], movimientos) && jugada.Ganador == tipoJugada)
                            js.Add(jugada);
                    }

                    // si hay jugadas, seleccionamos una.
                    if (js.Count > 0)
                    {
                        // Actualizamos las jugadas.
                        Jugadas = js;
                        // seleccionamos la actual jugada a realizar y realizamos el primer movimiento.
                        Debug.WriteLine("Jugada computador");
                        actual = SeleccionRandom(js);
                        return RealizarMov(actual.MovimientosC[0]);
                    }
                    else
                    {
                        tipoJugada = "J";
                    }
                }
                if (tipoJugada == "J")
                {
                    List<Jugada> js = new List<Jugada>();
                    // seleccionamos las jugadas con ganador jugador.
                    foreach (Jugada jugada in Jugadas)
                    {
                        if (PosicionVacia(jugada.Movimientos[0], movimientos) && jugada.Ganador == tipoJugada)
                            js.Add(jugada);
                    }

                    if (js.Count > 0)
                    {
                        // Actualizamos las jugadas.
                        Jugadas = js;
                        Debug.WriteLine("Jugada Jugador (" + js.Count + "jugadas)");
                        // seleccionamos la actual jugada a realizar y realizamos el primer movimiento.
                        actual = SeleccionRandom(js);
                        return RealizarMov(actual.MovimientosJ[0]);
                    }
                    else
                    {
                        Debug.WriteLine("Jugada inicial random");
                        return RealizarMov(JugadaRandom(movimientos));
                    }

                }

                return mov;
            } else
            {
                Debug.WriteLine("Jugada random");
                return RealizarMov(JugadaRandom(movimientos));
            }

            return mov;
        }

        /// <summary>
        /// Realiza un movimiento en el tablero.
        /// </summary>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        public string JugarPrimero(List<string> movimientos)
        {
            tipoJugada = "J";
            Jugada j = new Jugada(movimientos);
            Debug.WriteLine("Jugada actual");
            Debug.WriteLine(j);
            string mov = EnLinea(j, "J");
            Debug.WriteLine("Mov J:" + mov);

            if (mov != "")
            {
                Debug.WriteLine("En linea J");
                return mov;
            }

            mov = EnLinea(j, "C");
            Debug.WriteLine("Mov C:" + mov);

            if (mov != "")
            {
                Debug.WriteLine("En linea C");
                return mov;
            }

            if (actual != null)
            {
                Debug.WriteLine("Jugada existente");
                Debug.WriteLine("Copia jugada jugador");
                if (Igual(movimientos, actual.Movimientos))
                {
                    Debug.WriteLine("Copia jugada jugador existente");
                    return RealizarMov(actual.MovimientosJ[movActual]);
                }
                else
                {
                    List<Jugada> js = new List<Jugada>();
                    // seleccionamos las jugadas con ganador jugador.
                    foreach (Jugada jugada in Jugadas)
                    {
                        if (Igual(movimientos, jugada.Movimientos))
                        {
                            Debug.WriteLine("Nueva Copia jugada jugador");
                            js.Add(jugada);
                        }
                    }

                    // si hay jugadas, seleccionamos una.
                    if (js.Count > 0)
                    {
                        // Actualizamos las jugadas.
                        Jugadas = js;
                        Debug.WriteLine("Nueva Jugada jugador");
                        actual = SeleccionRandom(js);
                        return RealizarMov(actual.MovimientosJ[movActual]);
                    }
                    else
                    {
                        Debug.WriteLine("Copia jugada jugador random");
                        return RealizarMov(JugadaRandom(movimientos));
                    }
                }
            }
            else if (actual == null && Jugadas.Count > 0)
            {
                Debug.WriteLine("Jugada inicial");
                mov = "";
                List<Jugada> js = new List<Jugada>();
                // seleccionamos las jugadas con ganador jugador.
                foreach (Jugada jugada in Jugadas)
                {
                    if (jugada.Ganador == tipoJugada)
                        js.Add(jugada);
                }

                if (js.Count > 0)
                {
                    // Actualizamos las jugadas.
                    Jugadas = js;
                    Debug.WriteLine("Jugada Jugador (" + js.Count + "jugadas)");
                    // seleccionamos la actual jugada a realizar y realizamos el primer movimiento.
                    actual = SeleccionRandom(js);
                    return RealizarMov(actual.MovimientosJ[0]);
                }
                else
                {
                    Debug.WriteLine("Jugada inicial random");
                    return RealizarMov(JugadaRandom(movimientos));
                }
            }
            else
            {
                Debug.WriteLine("Jugada random");
                return RealizarMov(JugadaRandom(movimientos));
            }
        }

        /// <summary>
        /// Obtiene los movimientos realizados por el computador.
        /// </summary>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        public List<string> MovsComputador(List<string> movimientos)
        {
            List<string> movs = new List<string>();

            // guardamos los movimientos
            for (int i = 0; i < movimientos.Count; i++)
            {
                if (i % 2 == 0)
                    movs.Add(movs[i]);
            }

            return movs;
        }

        /// <summary>
        /// Comprueba si el jugador puede ganar la jugada con los
        /// movimientos actuales.
        /// </summary>
        /// <param name="jugada"></param>
        /// <param name="jugador"></param>
        /// <returns></returns>
        private string EnLinea(Jugada jugada, string jugador)
        {
            string posicion = "";
            List<string> movimientos = new List<string>();
            int i = 0;

            if (jugador == "J")
                movimientos = jugada.MovimientosJ;
            else if (jugador == "C")
                movimientos = jugada.MovimientosC;

            while (posicion == "" && i < lineas.Count)
            {
                posicion = Linea(lineas[i].Split(' '), jugada, movimientos);
                i += 1;
            }

            return posicion;
        }

        public string Linea(string[] linea, Jugada jugada, List<string> movs)
        {
            int l = 0;
            string pos = "";

            foreach (string mov in linea)
            {
                if (!PosicionVacia(mov, jugada.Movimientos) && !PosicionVacia(mov, movs))
                {
                    l += 2;
                } else if (PosicionVacia(mov, jugada.Movimientos)) {
                    pos = mov;
                    l += 1;
                }
            }

            if (l == 5)
            {
                Debug.WriteLine("posicion: " + pos);
                return pos;
            } else
            {
                return "";
            }
        }

        /// <summary>
        /// Devuelve una jugada ganadores simple.
        /// </summary>
        /// <returns></returns>
        private List<int[]> GetJugada(string linea)
        {
            List<int[]> jugada = new List<int[]>();
            string[] movs = linea.Split(' ');

            foreach (string mov in movs)
            {
                int[] posicion = new int[2];
                posicion[0] = mov[0];
                posicion[1] = mov[1];
                jugada.Add(posicion);
            }

            return jugada;
        }

        /// <summary>
        /// Realiza una jugada de forma aleatoria.
        /// </summary>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        private string JugadaRandom(List<string> movimientos)
        {
            RRandom r = new RRandom();
            string mov = "";

            int x = 0;
            int y = 0;

            x = r.Next(0, 2);
            y = r.Next(0, 2);
            mov = "" + x + y;

            if (PosicionVacia(mov, movimientos))
            {
                return mov;
            }
            else
            {
                return PrimerMovVacio(movimientos);
            }
        }

        /// <summary>
        /// Verifica si dos jugadas son iguales o si una contiene a la otra.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        public bool Igual(List<string> actual, List<string> movimientos)
        {
            bool iguales = true;
            if (movimientos.Count >= actual.Count)
            {
                for (int i = 0; i < actual.Count && iguales; i++)
                {
                    if (actual[i] != movimientos[i])
                        iguales = false;
                }
            } else
            {
                iguales = false;
            }

            return iguales;
        }

        public string PrimerMovVacio(List<string> movimientos)
        {
            string[] movs = { "00", "01", "02", "10", "11", "12", "20", "21", "22" };
            string pos = "";

            foreach (string mov in movs)
            {
                if( pos == "" && PosicionVacia(mov, movimientos) )
                {
                    pos = mov;
                }
            }

            return pos;
        }

        /// <summary>
        /// Selecciona una jugada de forma aleatoria.
        /// </summary>
        /// <param name="jugadas"></param>
        /// <returns></returns>
        public Jugada SeleccionRandom(List<Jugada> jugadas)
        {
            RRandom r = new RRandom();
            int jugada = r.Next(0, jugadas.Count);
            Debug.WriteLine(jugada);

            return jugadas[jugada];
        }

        /// <summary>
        /// Realiza un movimiento de la actual jugada.
        /// </summary>
        /// <param name="movimiento"></param>
        /// <returns></returns>
        public string RealizarMov(string movimiento)
        {
            Debug.WriteLine("Mov realizado: " + movimiento);
            movActual += 1;

            return movimiento;
        }

        /// <summary>
        /// Comprueba si una posicion del tablero esta vacia.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        private bool PosicionVacia(string pos, List<string> movimientos)
        {
            bool vacia = true;
            foreach (string mov in movimientos)
            {
                if (mov == pos)
                    vacia = false;
            }

            return vacia;
        }
    }
}
