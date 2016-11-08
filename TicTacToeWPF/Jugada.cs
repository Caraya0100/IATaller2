using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeWPF
{
    public class Jugada
    {
        private List<string> movimientos;
        private List<string> movimientosJ;
        private List<string> movimientosC;
        private string ganador;

        /// <summary>
        /// Constructor, recibe la jugada en formato string.
        /// </summary>
        /// <param name=""></param>
        public Jugada(string movimientos)
        {
            Movimientos = new List<string>();
            MovimientosJ = new List<string>();
            MovimientosC = new List<string>();
            string[] movs = movimientos.Split(' ');
            
            // guardamos los movimientos
            for (int i = 0; i < movs.Length; i++)
            {
                // Guardamos el ganador de la jugada, si es que esta especificado.
                if ((i == movs.Length - 1) && (movs[i] == "J" || movs[i] == "C"))
                {
                    Ganador = movs[i];
                } else
                {
                    // Guardamos el movimiento.
                    Movimientos.Add(movs[i]);
                    if (i % 2 != 0)
                        MovimientosC.Add(movs[i]);
                    else
                        MovimientosJ.Add(movs[i]);
                }
            }
        }

        /// <summary>
        /// Constructor, recibe la jugada en formato string.
        /// </summary>
        /// <param name=""></param>
        public Jugada(List<string> movimientos)
        {
            Movimientos = new List<string>();
            MovimientosJ = new List<string>();
            MovimientosC = new List<string>();

            // guardamos los movimientos
            for (int i = 0; i < movimientos.Count; i++)
            {
                // Guardamos el ganador de la jugada, si es que esta especificado.
                if ((i == movimientos.Count - 1) && (movimientos[i] == "J" || movimientos[i] == "C"))
                {
                    Ganador = movimientos[i];
                }
                else
                {
                    // Guardamos el movimiento.
                    Movimientos.Add(movimientos[i]);
                    if (i % 2 != 0)
                        MovimientosC.Add(movimientos[i]);
                    else
                        MovimientosJ.Add(movimientos[i]);
                }
            }
        }

        /// <summary>
        /// Devuelve la posicion del movimiento de la jugada.
        /// </summary>
        /// <param name="movimiento"></param>
        /// <returns></returns>
        public string MovJugada(int movimiento)
        {
            if (movimiento < (Movimientos.Count - 1))
                return Movimientos[movimiento];

            return null;
        }

        /// <summary>
        /// Devuelve la posicion del movimiento del jugador.
        /// </summary>
        /// <param name="movimiento"></param>
        /// <returns></returns>
        public string MovJugador(int movimiento)
        {
            if (movimiento < MovimientosJ.Count)
                return MovimientosJ[movimiento];

            return null;
        }

        /// <summary>
        /// Devuelve la posicion del movimiento del computador.
        /// </summary>
        /// <param name="movimiento"></param>
        /// <returns></returns>
        public string MovComputador(int movimiento)
        {
            if (movimiento < MovimientosC.Count)
                return MovimientosC[movimiento];

            return null;
        }

        public override string ToString()
        {
            string lista = "Movimientos: ";
            foreach(string mov in Movimientos)
            {
                lista += " " + mov;
            }
            lista += " Ganador: " + Ganador;
            lista += " Movimientos Jugador: ";
            foreach (string mov in MovimientosJ)
            {
                lista += " " + mov;
            }
            lista += " Movimientos Computador: ";
            foreach (string mov in MovimientosC)
            {
                lista += " " + mov;
            }

            return lista;
        }

        public List<string> Movimientos
        {
            get { return movimientos; }
            set { movimientos = value; }
        }

        public List<string> MovimientosJ
        {
            get { return movimientosJ; }
            set { movimientosJ = value; }
        }

        public List<string> MovimientosC
        {
            get { return movimientosC; }
            set { movimientosC = value; }
        }

        public string Ganador
        {
            get { return ganador; }
            set { ganador = value; }
        }
    }
}
