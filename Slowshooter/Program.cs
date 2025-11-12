using System;
using System.Linq;

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

        //P1 traps
        static char P1Trap1State = '3';
        static int P1Trap1X = 0;
        static int P1trap1Y = 0;
        static char P1Trap2State = '3';
        static int P1Trap2X = 0;
        static int P1trap2Y = 0;
        
        static int P1CurrentTrap = 1;

        //P2 traps
        static char P2Trap1State = '3';
        static int P2Trap1X = 0;
        static int P2trap1Y = 0;
        static char P2Trap2State = '3';
        static int P2Trap2X = 0;
        static int P2trap2Y = 0;
        
        static int P2CurrentTrap = 1;

        // bounds for player movement
        static (int, int) p1_min_max_x = (1, 3);
        static (int, int) p1_min_max_y = (1, 3);
        static (int, int) p2_min_max_x = (9, 11);
        static (int, int) p2_min_max_y = (1, 3);

        // what turn is it? will be 0 after game is drawn the first time
        static int turn = -1;

        // contains the keys that player 1 and player 2 are allowed to press
        static (char[], char[]) allKeybindings = (new char[]{ 'W', 'A', 'S', 'D','E' }, new char[]{ 'J', 'I', 'L', 'K','O' });
        static ConsoleColor[] playerColors = { ConsoleColor.Red, ConsoleColor.Blue };


        //trap hit veriable (changes to player number when spike is hit)
        static int trapHit = 0;
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            while(isPlaying)
            {
                ProcessInput();
                Update();
                Draw();
                
            }
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"player {trapHit} wins\n\n\n\n\n\n\n\n\n\n\n");
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
            if (input == ConsoleKey.E)
            {
                if (P1CurrentTrap == 1)
                {
                    P1Trap1State = '3';
                    P1Trap1X = p1_x_pos + 8;
                    P1trap1Y = p1_y_pos;
                    P1CurrentTrap = 2;
                    P1Trap1State = '3';
                }
                else if (P1CurrentTrap == 2)
                {
                    P1Trap2State = '3';
                    P1Trap2X = p1_x_pos + 8;
                    P1trap2Y = p1_y_pos;
                    P1CurrentTrap = 1;
                    P1Trap2State = '3';
                }
                
            }

            if (input == ConsoleKey.J) p2_x_input = -1;
            if (input == ConsoleKey.L) p2_x_input = 1;
            if (input == ConsoleKey.I) p2_y_input = -1;
            if (input == ConsoleKey.K) p2_y_input = 1;
            if (input == ConsoleKey.O)
            {
                if (P2CurrentTrap == 1)
                {
                    P2Trap1State = '3';
                    P2Trap1X = p2_x_pos - 8;
                    P2trap1Y = p2_y_pos;
                    P2CurrentTrap = 2;
                    P2Trap1State = '3';
                }
                else if (P2CurrentTrap == 2)
                {
                    P2Trap2State = '3';
                    P2Trap2X = p2_x_pos - 8;
                    P2trap2Y = p2_y_pos;
                    P2CurrentTrap = 1;
                    P2Trap2State = '3';
                }
            }

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

            //trap count down
            if (P1Trap1X != 0)
            {
                if (P1Trap1State == '3')
                {
                    P1Trap1State = '2';
                }
                else if (P1Trap1State == '2')
                {
                    P1Trap1State = '1';
                }
                else if (P1Trap1State == '1')
                {
                    P1Trap1State = '^';
                }

            }
            if (P1Trap2X != 0)
            {
                if (P1Trap2State == '3')
                {
                    P1Trap2State = '2';
                }
                else if (P1Trap2State == '2')
                {
                    P1Trap2State = '1';
                }
                else if (P1Trap2State == '1')
                {
                    P1Trap2State = '^';
                }

            }
            
            if (P2Trap1X != 0)
            {
                if (P2Trap1State == '3')
                {
                    P2Trap1State = '2';
                }
                else if (P2Trap1State == '2')
                {
                    P2Trap1State = '1';
                }
                else if (P2Trap1State == '1')
                {
                    P2Trap1State = '^';
                }

            }
            if (P2Trap2X != 0)
            {
                if (P2Trap2State == '3')
                {
                    P2Trap2State = '2';
                }
                else if (P2Trap2State == '2')
                {
                    P2Trap2State = '1';
                }
                else if (P2Trap2State == '1')
                {
                    P2Trap2State = '^';
                }

            }
            

            //trap cheak
            if (P1Trap1State == '^')
            {
                if (p2_x_pos == P1Trap1X)
                {
                    if (p2_y_pos == P1trap1Y)
                    {
                        trapHit = 1;
                    }
                }
            }
            if (P1Trap2State == '^')
            {
                if (p2_x_pos == P1Trap2X)
                {
                    if (p2_y_pos == P1trap2Y)
                    {
                        trapHit = 1;
                    }
                }
            }
            

            if (P2Trap1State == '^')
            {
                if (p1_x_pos == P2Trap1X)
                {
                    if (p1_y_pos == P2trap1Y)
                    {
                        trapHit = 2;
                    }
                }
            }
            if (P2Trap2State == '^')
            {
                if (p1_x_pos == P2Trap2X)
                {
                    if (p1_y_pos == P2trap2Y)
                    {
                        trapHit = 2;
                    }
                }
            }
            
            if (trapHit > 0)
            {
                isPlaying = false;
                
            }


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
            Console.WriteLine("\nUSE WASD or IJKL to move\n USE E or O to place traps");
            Console.ForegroundColor = ConsoleColor.White;

            if (P1Trap1X != 0)
            {
                Console.SetCursorPosition(P1Trap1X, P1trap1Y);
                Console.Write(P1Trap1State);
            }
            if (P1Trap2X != 0)
            {
                Console.SetCursorPosition(P1Trap2X, P1trap2Y);
                Console.Write(P1Trap2State);
            }
            if (P2Trap1X != 0)
            {
                Console.SetCursorPosition(P2Trap1X, P2trap1Y);
                Console.Write(P1Trap1State);
            }
            if (P2Trap2X != 0)
            {
                Console.SetCursorPosition(P2Trap2X, P2trap2Y);
                Console.Write(P1Trap2State);
            }

        }
    }
}
