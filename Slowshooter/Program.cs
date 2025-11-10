using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Slowshooter
{
    internal class Program
    {

        static string playField = 
@"+---+   +---+
|   |   |   |
|   |   |   |
|   |   |   |
+---+   +---+";

        static bool isPlaying = true;
        static bool isUp = true;
        static bool isLeft = true;

        static Random random = new Random();

        static (int, int) ballPos = (19, 10);

        // player input 
        static int p1_x_input;
        static int p1_y_input;

        static int p2_x_input;
        static int p2_y_input;

        // player 1 pos
        static int p1_x_pos = 2;
        static int p1_y_pos = 2;

        // player 2 pos
        static int p2_x_pos = 10;
        static int p2_y_pos = 2;

        // bounds for player movement
        static (int, int) p1_min_max_x = (1, 1);
        static (int, int) p1_min_max_y = (2, 17);
        static (int, int) p2_min_max_x = (39,39);
        static (int, int) p2_min_max_y = (2, 17);

        // what turn is it? will be 0 after game is drawn the first time
        static int turn = -1;

        // contains the keys that player 1 and player 2 are allowed to press
        static (char[], char[]) allKeybindings = (new char[]{ 'W', 'A', 'S', 'D' }, new char[]{ 'J', 'I', 'L', 'K' });
        static ConsoleColor[] playerColors = { ConsoleColor.Red, ConsoleColor.Blue };

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            src = new CancellationTokenSource();
            MainRunner();


            while (isPlaying)
            {
                //ProcessInput();
                //Update();
                //Draw();
               //--zander

                //
            }
            
        }

        static void ProcessInput()
        {
            // if this isn't here, input will block the game before drawing for the first time
            if (turn == -1) return;

            // reset input
            p1_x_input = 0;
            p1_y_input = 0;
            p2_x_input = 0;
            p2_y_input = 0;

            char[] allowedKeysThisTurn; // different keys allowed on p1 vs. p2 turn

            // choose which keybindings to use
            if (turn % 2 == 0) allowedKeysThisTurn = allKeybindings.Item1;
            else allowedKeysThisTurn = allKeybindings.Item2;

            // get the current player's input
            ConsoleKey input = ConsoleKey.NoName;
            while (!allowedKeysThisTurn.Contains(((char)input)))
            {
                input = Console.ReadKey(true).Key;
            }

            // check all input keys 
            if (input == ConsoleKey.A) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;
          
            if (input == ConsoleKey.J) p2_x_input = -1;
            if (input == ConsoleKey.L) p2_x_input = 1;
            if (input == ConsoleKey.I) p2_y_input = -1;
            if (input == ConsoleKey.K) p2_y_input = 1;

        }

        static void Update()
        {
            // update players' positions based on input
            p1_x_pos += p1_x_input;
            p1_x_pos = p1_x_pos.Clamp(p1_min_max_x.Item1, p1_min_max_x.Item2);

            p1_y_pos += p1_y_input;
            p1_y_pos = p1_y_pos.Clamp(p1_min_max_y.Item1, p1_min_max_y.Item2);

            p2_x_pos += p2_x_input;
            p2_x_pos = p2_x_pos.Clamp(p2_min_max_x.Item1, p2_min_max_x.Item2);

            p2_y_pos += p2_y_input;
            p2_y_pos = p2_y_pos.Clamp(p2_min_max_y.Item1, p2_min_max_y.Item2);

            turn += 1;

        }

        static void Draw()
        {
            // draw the background (playfield)
            Console.SetCursorPosition(0, 0);
            Console.Write(playField);

            // draw player 1
            Console.SetCursorPosition(p1_x_pos, p1_y_pos);
            Console.ForegroundColor = playerColors[0];
            Console.Write("O");

            // draw player 2
            Console.SetCursorPosition(p2_x_pos, p2_y_pos);
            Console.ForegroundColor = playerColors[1];
            Console.Write("O");

            // draw the Turn Indicator
            Console.SetCursorPosition(3, 5);
            Console.ForegroundColor = playerColors[turn % 2];

            Console.Write($"PLAYER {turn % 2 + 1}'S TURN!");


            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nUSE WASD or IJKL to move");
            Console.ForegroundColor = ConsoleColor.White;
        }
  

        static CancellationTokenSource src;


        static async void PlrInputRunner()
        {
            while (isPlaying)
            {
                
                if (src.Token.IsCancellationRequested) return;
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        p2_y_pos += 1;
                        break;
                    case ConsoleKey.UpArrow:
                        p2_y_pos -= 1;
                        break;
                    case ConsoleKey.S:
                        p1_y_pos += 1;
                        break;
                    case ConsoleKey.W:
                        p1_y_pos -= 1;
                        break;
                }
                await Task.Delay(1);
            }
        }
        static int frame = 0;
        static async void MainRunner()
        {
            Update();
            DrawBenj();
            Countdown();

            int frame = 0;
            while (isPlaying)
            {
                frame++;
                Update();
                //Draw();
                DrawBenj();
                BallMovement(frame);

                await Task.Delay(10);
            }
        }

        static void BallMovement(int frame)
        {
            if (frame % 10 != 0) return;
            if(isUp == true)
            {
                ballPos.Item2 -= 1;
            }
            else
            {
                ballPos.Item2 += 1;
            }

            if (isLeft == true)
            {
                ballPos.Item1 -= 1;
            }
            else
            {
                ballPos.Item1 += 1;
            }
        }

        static void Countdown()
        {
            int upDown = random.Next(0, 2);

            if (upDown == 0)
            {
                isUp = true;
            }
            else
            {
                isUp = false;
            }

            int leftRight = random.Next(0, 2);

            if (leftRight == 0)
            {
                isLeft = true;
            }
            else
            {
                isLeft = false;
            }

            Console.SetCursorPosition(ballPos.Item1, ballPos.Item2 - 1);
            Console.Write("3");
            Thread.Sleep(1000);
            Console.Write("2");
            Thread.Sleep(1000);
            Console.Write("1");
            Thread.Sleep(1000);
            Console.SetCursorPosition(ballPos.Item1, ballPos.Item2 - 1);
            Console.Write("GO!");
            PlrInputRunner();
            ClearGo();
        }

        static async void ClearGo()
        {
            await Task.Delay(3000);
            if (src.Token.IsCancellationRequested) return;
            Console.SetCursorPosition(ballPos.Item1, ballPos.Item2 - 1);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }


        static void DrawBenj()
        {


            // draw the background (playfield)
            //Console.Clear();

            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < 40; i++) Console.Write("░");

            for (int i = 0; i < 20; i++)

            {

                Console.Write("░");

                Console.Write("\n");

            }

            for (int i = 0; i < 41; i++) Console.Write("░");

            for (int i = 0; i < 20; i++)

            {

                Console.SetCursorPosition(40, i);

                Console.Write("░");

            }
            


            //Console.Write(playField);


            for (int i = 0; i < 39; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    Console.SetCursorPosition(i + 1,j + 1);
                    Console.Write(" ");
                }

            }

            // draw player 1

            Console.SetCursorPosition(p1_x_pos, p1_y_pos);
            Console.ForegroundColor = playerColors[0];
            Console.Write("|");
            Console.SetCursorPosition(p1_x_pos, p1_y_pos + 1);
            Console.Write("|");
            Console.SetCursorPosition(p1_x_pos, p1_y_pos - 1);
            Console.Write("|");
            

            // draw player 2
            Console.SetCursorPosition(p2_x_pos, p2_y_pos);
            Console.ForegroundColor = playerColors[1];
            Console.Write("|");
            Console.SetCursorPosition(p2_x_pos, p2_y_pos + 1);
            Console.Write("|");
            Console.SetCursorPosition(p2_x_pos, p2_y_pos - 1);
            Console.Write("|");


            // draw the Turn Indicator
            //Console.SetCursorPosition(3, 5);
            //Console.ForegroundColor = playerColors[turn % 2];

            //Console.Write($"PLAYER {turn % 2 + 1}'S TURN!");

            //ball movment
            Console.SetCursorPosition(ballPos.Item1, ballPos.Item2);
            Console.Write("@");


            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(20,20);
            Console.WriteLine("\nWS for Player 1");
            Console.WriteLine("\nUp/Down for Player 2");
            Console.ForegroundColor = ConsoleColor.White;
        }

























































































































        //
    }
}
