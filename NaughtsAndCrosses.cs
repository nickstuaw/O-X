using System;
using System.Collections.Generic;

// Also known as Tic Tac Toe
namespace NaughtsAndCrosses
{
    class Program
    {
        static void Main(string[] args)
        {
            bool playAgain = true;
            int[] board;
            int size;
            int current;
            string choice;
            int won;
            do
            {
                board = new int[] {};
                size = 3;
                current = 1;
                won = -1;
                board = empty(size);

                while(won < 0)
                {
                    writeBoard(board, size);
                    Console.WriteLine("Turn: " + getChar(current));
                    Console.WriteLine("Column, row (layout: n,n. E.g. 1,2)");
                    choice = Console.ReadLine();
                    //todo: validateChoice(choice)
                    while (!processChoice(current, choice, out board, board, size))
                    {
                        Console.WriteLine("Invalid value.");
                        choice = Console.ReadLine();
                    }
                    current = current == 1 ? 2 : 1;
                    checkForWin(board, size, out won);
                }
                writeBoard(board, size);
                Console.WriteLine(won == 0 ? "Draw!" : "The winner is player #" + won + "!");
                Console.WriteLine("Do you want to play again? (y/n...)");
                playAgain = Console.ReadKey().KeyChar == 'y';
            } while (playAgain);
        }
        static bool processChoice(int player, string choice, out int[] boardOut, int[] board, int size)
        {
            int[] newBoard = board;
            //split == 1,3
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
            } else if (!Int32.TryParse(split[0].Trim(), out col))
            {
                boardOut = newBoard;
                return false;
            }
            if(!((0 < row && row < 4) && (0 < col && col < 4)))
            {
                boardOut = board;
                return false;
            }
            row-=1;
            col-=1;
            int bidding = board[row * size + col];
            if(!checkForPlayer(bidding))
            {
                newBoard.SetValue(player,(row * size) + col);
                boardOut = newBoard;
                return true;
            } else
            {
                Console.WriteLine("That box is taken.");
            }
            boardOut = board;
            return false;
        }

        // size must be a factor of board.Length
        static void writeBoard(int[] board, int size)
        {
            Console.Clear();
            string line, spacer = new string('-', (size * 5) - 1);
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
            for(int i = 0; i < size; i++)
            {
                if (!checkForPlayer(board[i])) continue;
                if ((size % (i + 1) == 0
                    && squaresMatch(board[i], board[i + 1], board[i + 2]))
                    || squaresMatch(board[i], board[i + size], board[i + (size * 2)]))
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
