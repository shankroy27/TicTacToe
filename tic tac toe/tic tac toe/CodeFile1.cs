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
        public gameboard()
        {
            foreach (int i in boardmap)
                boardmap[i] = 0;
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
            int[] player1 = new int[5];
            int[] player2 = new int[5];
            int winner = -1;
            player1 = checkboard(board, 'X');
            player2 = checkboard(board, 'O');
            if (findtrip(player1))
                winner = 1;
            else if (findtrip(player2))
                winner = 2;
            else if (main.turns >= 9)
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
        public char[,] samplemap = new char[9, 9];
        public void display()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write(samplemap[i, j]);
                Console.Write("\n");
            }
        }
        public showboard()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    samplemap[i, j] = ' ';
            for (int i = 0; i < 9; i++)
            {
                samplemap[2, i] = '_';
                samplemap[5, i] = '_';
                samplemap[i, 2] = '|';
                samplemap[i, 6] = '|';
            }
            samplemap[2, 6] = '|';
            samplemap[5, 6] = '|';
            {
                samplemap[1, 0] = '1';
                samplemap[1, 4] = '2';
                samplemap[1, 8] = '3';
                samplemap[4, 0] = '4';
                samplemap[4, 4] = '5';
                samplemap[4, 8] = '6';
                samplemap[7, 0] = '7';
                samplemap[7, 4] = '8';
                samplemap[7, 8] = '9';
            }
        }
        public void movsave(int[] board)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                    continue;
                switch (i)
                {
                    case 0:
                        samplemap[1, 0] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 1:
                        samplemap[1, 4] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 2:
                        samplemap[1, 8] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 3:
                        samplemap[4, 0] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 4:
                        samplemap[4, 4] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 5:
                        samplemap[4, 8] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 6:
                        samplemap[7, 0] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 7:
                        samplemap[7, 4] = (board[i] == 1 ? 'X' : 'O');
                        break;
                    case 8:
                        samplemap[7, 8] = (board[i] == 1 ? 'X' : 'O');
                        break;
                }
            }
        }
    }
    public static class main
    {
        public static char symbol = 'X';
        static gameboard board = new gameboard();
        static showboard key = new showboard();
        static ArrayList eboard = new ArrayList();
        static AI com = new AI();
        public static int turns=0;
        static int endturn()
        {
            turns++;
            Console.Clear();
            int win = util.checkwin(board.getboard());
            if (win == -1)
                changeside();
            return win;
        }
        static void changeside()
        {
            symbol = (symbol == 'X') ? 'O' : 'X';
        }
        public static int run(int x)
        {
            com.origdepth = 2;
            if (x == 1)
                x = com.minimax(board.getboard(), symbol, -100, 100, com.origdepth);
            else
                x = int.Parse(Console.ReadLine());
            return x;
        }
        public static void rfm()
        {
           Random rnd = new Random();
            int x=rnd.Next(2);
            if (x == 1)
                com.compside = 'X';
            else
                com.compside = 'O';
        }

        public static void Main(string[] args)
        {
            int i, win = -1,pos,move;
            eboard = util.filterboard(board.getboard());
            com.compside = 'X';
            //rfm();
            for (i = 0; i < eboard.Count; i++)
            {
                Console.WriteLine(" Turn: " + symbol);
                key.display();
                Console.Write("Enter Your Choice: ");
                move = (symbol == com.compside ? 1 : 2);
                //move = 1; com.compside = symbol;
                pos = run(move);
                pos--;
                if (!board.putmove(pos, symbol))
                {
                    i--;
                    continue;
                }
                key.movsave(board.getboard());
                //Console.ReadKey();
                win = endturn();
                if (win != -1)
                    break;
            }
            key.display();
            if (i == 0)
                Console.WriteLine("Draw!!!");
            else
                Console.WriteLine((win == 1 ? "X" : "O") + " Wins");
            Console.ReadKey();
        }
    }
}
