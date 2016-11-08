using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace TicTacToeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // true = X / false = 0
        public static bool TURN = true;
        // true = hot seat / false = AI
        public static int MODE = Constants.JUGADOR;

        private List<Button> gameButtons;
        private Computador computador;
        private List<string> movimientos;
        private string ganador;

        public MainWindow()
        {
            InitializeComponent();

            movimientos = new List<string>();
            computador = new Computador();
            gameButtons = new List<Button>();

            gameButtons.Add(A1);
            gameButtons.Add(A2);
            gameButtons.Add(A3);

            gameButtons.Add(B1);
            gameButtons.Add(B2);
            gameButtons.Add(B3);

            gameButtons.Add(C1);
            gameButtons.Add(C2);
            gameButtons.Add(C3);

            if (MODE == Constants.COMPUTADOR)
                MovimientoComputador();
        }

        private void gameAction_Click(object sender, RoutedEventArgs e)
        {
            //MODE = Constants.AI_HARD_MODE;

            // Make move
            Button pressedButton = (Button)sender;
            if(TURN) {
                pressedButton.Content = Constants.X_SYMBOL;
                pressedButton.IsEnabled = false;
                TURN = false;
            } else {
                pressedButton.Content = Constants.O_SYMBOL;
                pressedButton.IsEnabled = false;
                TURN = true;
            }

            if (pressedButton.Name == "A1") movimientos.Add("00");
            if (pressedButton.Name == "A2") movimientos.Add("01");
            if (pressedButton.Name == "A3") movimientos.Add("02");
            if (pressedButton.Name == "B1") movimientos.Add("10");
            if (pressedButton.Name == "B2") movimientos.Add("11");
            if (pressedButton.Name == "B3") movimientos.Add("12");
            if (pressedButton.Name == "C1") movimientos.Add("20");
            if (pressedButton.Name == "C2") movimientos.Add("21");
            if (pressedButton.Name == "C3") movimientos.Add("22");

            if (checkGameStatus())
            {
                return;
            }

            MovimientoComputador();
            TURN = true;

            // Move AI if necessary
            /*if (MODE == Constants.AI_EASY_MODE || MODE == Constants.AI_HARD_MODE)
            {
                //performAiMove();
                TURN = true;
            }*/

            checkGameStatus();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developed for C# course at our university"
                + Environment.NewLine
                + Environment.NewLine
                + "Henning Muzynski" + Environment.NewLine
                + "Christian Piatka" + Environment.NewLine
                + "Patrick Spitzer" + Environment.NewLine
                + "Johannes Idelhauser", "About");
        }

        private void onRestartButton_Click(object sender, EventArgs e)
        {
            // Restart the game
            // Activate all buttons and reset their texts
            foreach(Button button in gameButtons)
            {
                button.IsEnabled = true;
                button.Content = "";
            }

            movimientos = new List<string>();
            ganador = "";
            computador = new Computador();

            // Reset member variables
            TURN = true;
        }

        private void gameModeComboBox_Click(object sender, SelectionChangedEventArgs e)
        {
            // Get clicked item
            /*int selection = gameModeComboBox.SelectedIndex;
            MODE = selection;*/
            int selection = gameModeComboBox.SelectedIndex;
            MODE = selection;
            if (MODE == Constants.COMPUTADOR)
                MovimientoComputador();
        }

        private static void RealizarMovComputador(Button button)
        {
            Debug.WriteLine("boton " + button.Name);
            button.Content = Constants.O_SYMBOL;
            button.IsEnabled = false;
        }

        private void MovimientoComputador()
        {
            string mov = "";

            if (MODE == Constants.JUGADOR)
                mov = computador.Jugar(movimientos);
            else if (MODE == Constants.COMPUTADOR)
                mov = computador.JugarPrimero(movimientos);

            Debug.WriteLine("Movimiento: " + mov);

            movimientos.Add(mov);

            if (mov == "00") RealizarMovComputador(A1);
            if (mov == "01") RealizarMovComputador(A2);
            if (mov == "02") RealizarMovComputador(A3);
            if (mov == "10") RealizarMovComputador(B1);
            if (mov == "11") RealizarMovComputador(B2);
            if (mov == "12") RealizarMovComputador(B3);
            if (mov == "20") RealizarMovComputador(C1);
            if (mov == "21") RealizarMovComputador(C2);
            if (mov == "22") RealizarMovComputador(C3);
        }

        private void performAiMove()
        {
            /*if (MODE == Constants.AI_EASY_MODE)
            {
                ArtificialIntelligence.performEasyMove(gameButtons);
            }
            else if (MODE == Constants.AI_HARD_MODE)
            {
                ArtificialIntelligence.performHardMove(gameButtons);
            }*/
        }

        private bool checkGameStatus()
        {
            GameStatus status = checkHorizontal();
            if (status.isGameOver())
            {
                disableGame();
                updateStats(status);
                MessageBox.Show("Player " + status.winner + " has won the game!");
                return true;
            }

            status = checkVertical();
            if (status.isGameOver())
            {
                disableGame();
                updateStats(status);
                MessageBox.Show("Player " + status.winner + " has won the game!");
                return true;
            }

            status = checkDiagonal();
            if (status.isGameOver())
            {
                disableGame();
                updateStats(status);
                MessageBox.Show("Player " + status.winner + " has won the game!");
                return true;
            }

            if (checkForTie())
            {
                disableGame();
                updateStats(new GameStatus(true, "", true));
                MessageBox.Show("The game ended in a tie!");
                return true;
            }

            return false;
        }

        private void disableGame()
        {
            foreach(Button button in gameButtons)
            {
                button.IsEnabled = false;
            }
        }

        private void updateStats(GameStatus status)
        {
            if (status.isGameOver())
            {
                if(status.getWinner().Equals(Constants.X_SYMBOL))
                {
                    int currentWins = Convert.ToInt32(winsX.Content);
                    winsX.Content = "" + (currentWins + 1);
                }
                else if (status.getWinner().Equals(Constants.O_SYMBOL))
                {
                    int currentWins = Convert.ToInt32(winsO.Content);
                    winsO.Content = "" + (currentWins + 1);
                }
                else if (status.isTie())
                {
                    int currentTies = Convert.ToInt32(ties.Content);
                    ties.Content = "" + (currentTies + 1);
                }
            }
        }

        public void GuardarJugadaGanadora(string ganador)
        {
            string jugada = "";

            if (ganador == "X" && MODE == Constants.JUGADOR)
                ganador = "J";
            else if (ganador == "O" && MODE == Constants.JUGADOR)
                ganador = "C";
            else if (ganador == "X" && MODE == Constants.COMPUTADOR)
                ganador = "C";
            else if (ganador == "O" && MODE == Constants.COMPUTADOR)
                ganador = "J";

            foreach (string mov in movimientos)
            {
                jugada += mov + " ";
            }

            jugada += ganador;

            string[] lines = File.ReadAllLines(@"C:\Users\Sefirot\Documents\Visual Studio 2015\Projects\IATaller2\TicTacToeWPF\bin\Debug\jugadas.txt");
            bool guardar = true;

            for (int i = 0; i < lines.Length && guardar; i++)
            {
                if (jugada == lines[i])
                {
                    guardar = false;
                }
            }

            if (guardar)
            {
                File.AppendAllText(@"C:\Users\Sefirot\Documents\Visual Studio 2015\Projects\IATaller2\TicTacToeWPF\bin\Debug\jugadas.txt", jugada + Environment.NewLine);
            }
        }

        // HELPER
        private GameStatus checkHorizontal()
        {
            bool gameOver = false;
            string winner = "";
            // Horizontal check
            if (A1.Content.Equals(A2.Content)
                && A1.Content.Equals(A3.Content)
                && A2.Content.Equals(A3.Content)
                && !A1.Content.Equals(""))
            {
                // Top row won
                //MessageBox.Show("top row");
                gameOver = true;
                winner = Convert.ToString(A1.Content);
                GuardarJugadaGanadora(winner);
            }
            else if (B1.Content.Equals(B2.Content)
                    && B1.Content.Equals(B3.Content)
                    && B2.Content.Equals(B3.Content)
                    && !B1.Content.Equals(""))
            {
                // Middle row won
                //MessageBox.Show("middle row");
                gameOver = true;
                winner = Convert.ToString(B1.Content);
                GuardarJugadaGanadora(winner);
            }
            else if (C1.Content.Equals(C2.Content)
                    && C1.Content.Equals(C3.Content)
                    && C2.Content.Equals(C3.Content)
                    && !C1.Content.Equals(""))
            {
                // Bottom row won
                //MessageBox.Show("bottom row");
                gameOver = true;
                winner = Convert.ToString(C1.Content);
                GuardarJugadaGanadora(winner);
            }


            return new GameStatus(gameOver, winner, false);
        }

        private GameStatus checkVertical()
        {
            bool gameOver = false;
            string winner = "";
            // Vertical check
            if (A1.Content.Equals(B1.Content)
                && A1.Content.Equals(C1.Content)
                && B1.Content.Equals(C1.Content)
                && !A1.Content.Equals(""))
            {
                // Left column won
                //MessageBox.Show("left column");
                gameOver = true;
                winner = Convert.ToString(A1.Content);
                GuardarJugadaGanadora(winner);
            }
            else if (A2.Content.Equals(B2.Content)
                    && A2.Content.Equals(C2.Content)
                    && B2.Content.Equals(C2.Content)
                    && !A2.Content.Equals(""))
            {
                // Middle column won
                //MessageBox.Show("middle column");
                gameOver = true;
                winner = Convert.ToString(A2.Content);
                GuardarJugadaGanadora(winner);
            }
            else if (A3.Content.Equals(B3.Content)
                    && A3.Content.Equals(C3.Content)
                    && B3.Content.Equals(C3.Content)
                    && !A3.Content.Equals(""))
            {
                // Right column won
                //MessageBox.Show("right column");
                gameOver = true;
                winner = Convert.ToString(A3.Content);
                GuardarJugadaGanadora(winner);
            }
            return new GameStatus(gameOver, winner, false);
        }

        private GameStatus checkDiagonal()
        {
            bool gameOver = false;
            string winner = "";
            // Diagonal check
            if (A1.Content.Equals(B2.Content)
                && A1.Content.Equals(C3.Content)
                && B2.Content.Equals(C3.Content)
                && !A1.Content.Equals(""))
            {
                // Top left to bottom right won
                //MessageBox.Show("tl-br");
                gameOver = true;
                winner = Convert.ToString(A1.Content);
                GuardarJugadaGanadora(winner);
            }
            else if (C1.Content.Equals(B2.Content)
                    && C1.Content.Equals(A3.Content)
                    && B2.Content.Equals(A3.Content)
                    && !C1.Content.Equals(""))
            {
                // Bottom left to top right won
                //MessageBox.Show("bl-tr");
                gameOver = true;
                winner = Convert.ToString(C1.Content);
                GuardarJugadaGanadora(winner);
            }
            return new GameStatus(gameOver, winner, false);
        }

        private bool checkForTie()
        {
            bool tie = true;
            foreach (Button button in gameButtons)
            {
                if (button.IsEnabled == true)
                {
                    tie = false;
                    break;
                }
            }

            return tie;
        }
    }
}
