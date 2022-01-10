// Written by Nick. https//www.github.com/nsgwick/

using System;
using System.Collections.Generic;

// Also known as Tic Tac Toe
namespace NaughtsAndCrosses
{
    class Program
    {
        static void Main(string[] args)
        {
            bool playAgain;
            int[] board;
            int size;
            int current;
            string choice;
            int won, stop;
            do
            {
                setSize(out size);
                current = 1;
                won = -1;
                board = empty(size);
                stop = 0;
                while(won < 0)
                {
                    writeBoard(board, size);
                    Console.WriteLine("Turn: " + getChar(current));
                    Console.WriteLine("Column, row (layout: n,n. E.g. 1,2)");
                    choice = Console.ReadLine();
                    while (!processChoice(current, choice, out board, board, size, out stop))
                    {
                        Console.WriteLine("Invalid value.");
                        choice = Console.ReadLine();
                    }
                    if (stop > 0)
                        break;
                    current = current == 1 ? 2 : 1;
                    checkForWin(board, size, out won);
                }

                if (stop == 2)
                    break;
                else if (stop == 1)
                {
                    playAgain = true;
                }
                else
                {
                    writeBoard(board, size);
                    Console.WriteLine(won == 0 ? "Draw!" : "The winner is player #" + won + "!");
                    Console.WriteLine("Do you want to play again? (y/n...)");
                    playAgain = Console.ReadKey().KeyChar == 'y';
                }
            } while (playAgain);
        }

        static void setSize(out int size)
        {
            Console.WriteLine("Choose board size (default 3, min. 3, max 9):");
            if (!Int32.TryParse(Console.ReadLine(), out size))
            {
                size = 3;
            }
            else if (size > 9)
            {
                size = 9;
            }
            else if (size < 3)
            {
                size = 3;
            }
        }
        static bool processChoice(int player, string choice, out int[] boardOut, int[] board, int size, out int stop)
        {
            int[] newBoard = board;
            if (choice.ToLower().StartsWith("s"))
            {
                boardOut = newBoard;
                stop = 0;
                return true;
            }
            if (choice.ToLower().StartsWith("r"))
            {
                boardOut = newBoard;
                stop = 1;
                return true;
            }
            if (choice.ToLower().StartsWith("q"))
            {
                boardOut = newBoard;
                stop = 2;
                return true;
            }
            stop = 0;
            string[] split = choice.Split(',');
            if(split.Length < 2)
            {
                boardOut = newBoard;
                return false;
            }
            int row,
                col;
            if(!Int32.TryParse(split[1].Trim(), out row)) {
                boardOut = newBoard;
                return false;
            } 
            else if (!Int32.TryParse(split[0].Trim(), out col))
            {
                boardOut = newBoard;
                return false;
            }
            if(!((0 < row && row <= size) && (0 < col && col <= size)))
            {
                boardOut = board;
                return false;
            }
            row--;
            col--;
            int bidding = board[row * size + col];
            if(!checkForPlayer(bidding))
            {
                newBoard.SetValue(player,(row * size) + col);
                boardOut = newBoard;
                return true;
            } 
            Console.WriteLine("That box is taken.");
            boardOut = board;
            return false;
        }

        // size must be a factor of board.Length
        static void writeBoard(int[] board, int size)
        {
            Console.Clear();
            Console.WriteLine("Type s to skip, r to restart or q to quit.\n");
            string line, spacer = new string('-', (size * 4) + 2);
            for(int i = 1; i <= size; i++)
            {
                Console.Write("---" + i);
            }
            Console.Write("--\n");
            for(int i = 0; i < size; i++)
            {
                line = i + 1 + "|";
                for(int j = 0; j < size; j++)
                {
                    line += " " + getChar(board[(i * size) + j]) + " |";
                }
                Console.WriteLine(line);
                Console.WriteLine(spacer);
            }
        }
        static char getChar(int n)
        {
            switch(n)
            {
                case 1:
                    return 'O';
                case 2:
                    return 'X';
            }
            return ' ';
        }
        static char getPlayer(int current)
        {
            return getChar(current);
        }
        static bool checkForWin(int[] board, int size, out int winner)
        {
            List<int> verticalSquares, horizontalSquares;
            for(int i = 0; i < size; i++)
            {
                verticalSquares = new List<int>();
                horizontalSquares = new List<int>();
                for (int j = 0; j < size; j++)
                {
                    verticalSquares.Add(board[i + size * j]);
                    horizontalSquares.Add(board[i + j]);
                }
                if (!checkForPlayer(board[i])) continue;
                if ((size % (i + 1) == 0
                    && squaresMatch(horizontalSquares.ToArray()))
                    || squaresMatch(verticalSquares.ToArray()))
                {
                    winner = getChar(board[i]) == 'O' ? 1 : 2;
                    return true;
                }
            }
            List<int> squaresFromDiagonal = new List<int>();
            for(int i = 0; i < size; i++)
            {
                squaresFromDiagonal.Add(board[i]);
            }
            if (squaresMatch(squaresFromDiagonal.ToArray()) && checkForPlayer(squaresFromDiagonal[0]))
            {
                winner = squaresFromDiagonal[0];
                return true;
            }
            squaresFromDiagonal = new List<int>();
            for (int i = size - 1; i >= 0; i--)
            {
                squaresFromDiagonal.Add(board[i]);
            }
            if (squaresMatch(squaresFromDiagonal.ToArray()) && checkForPlayer(squaresFromDiagonal[size - 1]))
            {
                winner = squaresFromDiagonal[size - 1];
                return true;
            }
            foreach(int i in board)
            {
                if(!checkForPlayer(i))
                {
                    winner = -1;
                    return false;
                }
            }
            winner = 0;
            return true;
        }
        static bool checkForPlayer(int n)
        {
            return n == 1 || n == 2;
        }
        static bool squaresMatch(params int[] squares)
        {
            int previous = squares[0];
            for (int i = 0; i < squares.Length; i++)
            {
                if (previous != squares[i]) return false;
                previous = squares[i];
            }
            return true;
        }
        static int[] empty(int size)
        {
            List<int> list = new List<int>();
            for(int i = 0; i < size * size; i++)
            {
                list.Add(getChar(0));
            }
            return list.ToArray();
        }
    }
}
