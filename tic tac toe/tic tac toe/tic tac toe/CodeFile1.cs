using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//using System.Text.RegularExpressions;


/*Please dont change namespace, Dcoder 
 and class must not be public*/

//Compiler version 4.0, .NET Framework 4.5


namespace Dcoder
{
    public class gameboard
    {
        public int[] boardmap = new int[9];
        public void reset()
        {
            for (int i=0;i<9;i++)
                boardmap[i] = 0;
        }
        public gameboard()
        {
            reset();
        }
        public int[] getboard()
        {
            return boardmap;
        }
        public bool putmove(int pos, char symbol)
        {
            if ((pos < 0) || (pos > 8))
                return false;
            if (boardmap[pos] != 0)
                return false;
            boardmap[pos] = symbol == 'X' ? 1 : 2;

            return true;
        }
    }
    public class AI
    {
        public char compside = 'X';
        public int origdepth = -1;
        public int min_max(int[] board, char symbol)
        {
            int bestval = -1000;
            int bestmove = -1;
            ArrayList eboard = new ArrayList();
            eboard = util.filterboard(board);
            int nsymbol = (symbol == 'X' ? 1 : 2);
            for (int i = 0; i < eboard.Count; i++)
            {
                board[(int)eboard[i]] = nsymbol;
                int moveval = mini_max(board, util.changeside(symbol));
                if (moveval > bestval)
                {
                    bestmove = (int)eboard[i];
                    bestval = moveval;
                }
                board[(int)eboard[i]] = 0;
            }
            return bestmove;
        }
        public int mini_max(int[] board, char symbol)
        {
            int nsymbol = (symbol == 'X' ? 1 : 2);
            int score = util.checkwin(board);
            if (score != -1)
            {
                if (compside == 'X')
                {
                    if (score == 1)
                        return 10;
                    else if (score == 2)
                        return -10;
                    else
                        return 0;
                }
                else
                {
                    if (score == 2)
                        return 10;
                    else if (score == 1)
                        return -10;
                    else
                        return 0;
                }
            }
            //int[] newboard = new int[9];
            //Array.Copy(board, newboard, 9);
            ArrayList eboard = new ArrayList();
            eboard = util.filterboard(board);
            if (symbol==compside)
            {
                int best = -1000,tbest;
                for(int i =0;i<eboard.Count;i++)
                {
                    board[(int)eboard[i]] = nsymbol;
                    tbest = mini_max(board, util.changeside(symbol));
                    if (tbest > best)
                        best = tbest;
                    board[(int)eboard[i]] = 0;
                }
                return best;
            }
            else
            {
                int best = 1000, tbest;
                for (int i = 0; i < eboard.Count; i++)
                {
                    board[(int)eboard[i]] = nsymbol;
                    tbest = mini_max(board, util.changeside(symbol));
                    if (tbest < best)
                        best = tbest;
                    board[(int)eboard[i]] = 0;
                }
                return best;
            }
        }
        public int minimax(int[] board, char symbol, int alpha, int beta, int depth = 2)
        {
            int win = util.checkwin(board);
            int nsymbol = (symbol == 'X' ? 1 : 2);
            int mult = (symbol == compside ? 1 : -1);
            if (win != -1)
            {
                if (win == nsymbol)
                    return mult;
                else if (win != 0)
                    return (mult * -1);
                else
                    return 0;
            }
            if (depth == 0)
                return 0;
            int[] newboard = new int[9];
            Array.Copy(board, newboard, 9);
            int score, i, pos = -1;
            ArrayList emptyboard = new ArrayList();
            emptyboard = util.filterboard(newboard);
            for (i = 0; i < emptyboard.Count; i++)
            {
                if (i > 0)
                    newboard[(int)emptyboard[i - 1]] = 0;
                newboard[(int)emptyboard[i]] = nsymbol;
                score = minimax(newboard, util.changeside(symbol), alpha, beta, depth - 1);
                if (mult == 1)
                {
                    if (score > alpha)
                    {
                        alpha = score;
                        pos = (int)emptyboard[i];
                    }
                    if (alpha >= beta)
                        break;
                }
                else
                {
                    if (score < beta)
                        beta = score;
                    if (alpha >= beta)
                        break;
                }
            }
            if (depth == origdepth)
                return pos;
            if (mult == 1)
                return alpha;
            else
                return beta;
        }
    }
    public static class util
    {
        
        public static readonly int[] magicsq = { 2, 7, 6, 9, 5, 1, 4, 3, 8 };
        public static char changeside(char symbol)
        {

            return (symbol == 'X') ? 'O' : 'X';

        }
        public static bool findtrip(int[] array)
        {
            Array.Sort(array);
            for (int a = 0; a < 3; a++)
            {
                if (array[a] == 0)
                    continue;
                int l, r;
                l = a + 1;
                r = 4;
                while (l < r)
                {
                    if (array[a] + array[l] + array[r] == 15)
                    {
                        if (!((array[a] == 0) || (array[l] == 0) || (array[r] == 0)))
                            return true;
                    }
                    else if (array[a] + array[l] + array[r] < 15)
                        l++;
                    else if (array[a] + array[l] + array[r] > 15)
                        r--;
                }
            }
            return false;
        }
        public static int maxx(int[] array)
        {
            int max = -20;
            foreach (int i in array)
                if (max < array[i])
                    max = array[i];
            return max;
        }
        public static int minn(int[] array)
        {
            int min = 20;
            foreach (int i in array)
                if (min > array[i])
                    min = array[i];
            return min;
        }
        public static int checkwin(int[] board)
        {
            ArrayList eboard = new ArrayList();
            eboard = filterboard(board);
            int[] player1 = new int[5];
            int[] player2 = new int[5];
            int winner = -1;
            player1 = checkboard(board, 'X');
            player2 = checkboard(board, 'O');
            if (findtrip(player1))
                winner = 1;
            else if (findtrip(player2))
                winner = 2;
            else if (eboard.Count == 0)
                winner = 0;
            return winner;
        }
        public static int[] checkboard(int[] board, char symbol)
        {
            int j = 0;
            int playerside = (symbol == 'X' ? 1 : 2);
            int[] player = new int[5];
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == playerside)
                {
                    player[j] = magicsq[i];
                    j++;
                }
            }
            return player;
        }
        public static ArrayList filterboard(int[] board)
        {
            ArrayList emptyboard = new ArrayList();
            emptyboard.Clear();
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                    emptyboard.Add(i);
            }
            emptyboard = shuffle(emptyboard);
            return emptyboard;
        }
        public static ArrayList shuffle(ArrayList eboard)
        {
            ArrayList sort = new ArrayList();
            Random gen = new Random();
            while (eboard.Count > 0)
            {
                int pos = gen.Next(eboard.Count);
                sort.Add(eboard[pos]);
                eboard.RemoveAt(pos);
            }
            return sort;
        }
    }
    public class showboard
    {
        public char[,] samplemap = new char[3, 9];
        public char[,] realmap = new char[3, 9];
        public void display()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(realmap[i, j]);
                }
                Console.Write("          ");
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(samplemap[i, j]);
                }
                Console.Write("\n");
            }
        }
        public void reset()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 9; j++)
                {
                    samplemap[i, j] = ' ';
                    realmap[i, j] = ' ';
                }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j += 3)
                {
                    samplemap[i, j] = '[';
                    realmap[i, j] = '[';
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 2; j < 9; j += 3)
                {
                    samplemap[i, j] = ']';
                    realmap[i, j] = ']';
                }
            }
            {
                samplemap[0, 1] = '7';
                samplemap[0, 4] = '8';
                samplemap[0, 7] = '9';
                samplemap[1, 1] = '4';
                samplemap[1, 4] = '5';
                samplemap[1, 7] = '6';
                samplemap[2, 1] = '1';
                samplemap[2, 4] = '2';
                samplemap[2, 7] = '3';
            }
        }
        public showboard()
        {
            reset();
        }
        public void movsave(int[] board)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                    continue;
                switch (i)
                {
                    case 6:
                        realmap[0, 1] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 7:
                        realmap[0, 4] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 8:
                        realmap[0, 7] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 3:
                        realmap[1, 1] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 4:
                        realmap[1, 4] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 5:
                        realmap[1, 7] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 0:
                        realmap[2, 1] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 1:
                        realmap[2, 4] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 2:
                        realmap[2, 7] = (board[i] == 1 ? 'X' : 'O');
                        break;
                }
            }
        }
    }
    public static class main
    {
        public static char symbol = 'X';
        public static string pl1 = "Player 1", pl2 = "Computer",curplayer = pl1;
        static gameboard board = new gameboard();
        static showboard key = new showboard();
        static ArrayList eboard = new ArrayList();
        static AI com = new AI();
        static int endturn()
        {
            Console.Clear();
            int win = util.checkwin(board.getboard());
            if (win == -1)
                changeside();
            return win;
        }
        static void changeside()
        {
            curplayer = (curplayer == pl1 ? pl2 : pl1);
            symbol = util.changeside(symbol);
        }
        public static int run(int x)
        {
            com.origdepth = 9;
            if (x == 1)
                x = com.min_max(board.getboard(), com.compside);
            else
                x = int.Parse(Console.ReadLine()) - 1;
            return x;
        }
        public static void rfm()
        {
           Random rnd = new Random();
            int x=rnd.Next(2);
            symbol = 'X';
            if (x == 1)
            {
                com.compside = 'X';
                curplayer = "Computer";
            }
            else
            {
                com.compside = 'O';
                curplayer = "Player 1";
            }
        }
        public static void reset()
        {
            Console.Clear();
            key.reset();
            board.reset();
        }
        public static void mainfunc()
        {
            int i, win = -1, pos, move;
            eboard = util.filterboard(board.getboard());
            rfm();
            for (i = 0; i < eboard.Count; i++)
            {
                Console.WriteLine("       Turn: " + curplayer);
                key.display();
                Console.Write("Enter Your Choice: ");
                move = (symbol == com.compside ? 1 : 2);
                pos = run(move);
                if (!board.putmove(pos, symbol))
                {
                    i--;
                    continue;
                }
                key.movsave(board.getboard());
                Console.Write("\n\n");
                win = endturn();
                if (win != -1)
                    break;
            }
            key.display();
            win = util.checkwin(board.getboard());
            if (win == 1 || win == 2)
                Console.WriteLine((win == 1 ? "X" : "O") + " Wins");
            else
                Console.WriteLine("Draw!!!");
        }
        public static void Main(string[] args)
        {
            bool play = true;
            do
            {
                reset();
                mainfunc();
                Console.WriteLine("Press 1 to Play again and any other key to exit!!");
                string x = Console.ReadLine();
                if (x != "1")
                    play = false;
            } while (play);
        }   
    }
}
