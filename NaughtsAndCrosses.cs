// Written by Nick. https//www.github.com/nsgwick/

using System;
using System.Collections.Generic;

// Also known as Tic Tac Toe
namespace NaughtsAndCrosses
{
    class Program
    {
        private static int[] _board;
        private static int _size;
        private static void Main(string[] args)
        {
            bool playAgain;
            do
            {
                SetSize();
                int won = -1, stop = 0, current = 1;
                _board = Empty();
                while(won < 0)
                {
                    WriteBoard();
                    Console.WriteLine("Turn: " + GetChar(current));
                    Console.WriteLine("Column, row (layout: n,n. E.g. 1,2)");
                    var choice = Console.ReadLine();
                    while (!ProcessChoice(current, choice, out stop))
                    {
                        Console.WriteLine("Invalid value.");
                        choice = Console.ReadLine();
                    }
                    if (stop > 0) break;
                    current = current == 1 ? 2 : 1;
                    CheckForWin(out won);
                }
                if (stop == 2) break;
                if (stop == 1) playAgain = true;
                else
                {
                    WriteBoard();
                    Console.WriteLine(won == 0 ? "Draw!" : "The winner is player #" + won + " (" + GetPlayer(won) + ")!");
                    Console.WriteLine("Do you want to play again? (y/n...)");
                    playAgain = Console.ReadKey().KeyChar == 'y';
                }
            } while (playAgain);
        }
        private static void SetSize()
        {
            Console.WriteLine("Choose board size (default 3, min. 3, max 9):");
            _size = !int.TryParse(Console.ReadLine(), out _size) ? 3 : _size > 9 ? 9 : _size < 3 ? 3 : _size;
        }
        private static bool ProcessChoice(int player, string choice, out int stop)
        {
            if (choice.ToLower().StartsWith("s"))
            {
                stop = 0;
                return true;
            }
            if (choice.ToLower().StartsWith("r"))
            {
                stop = 1;
                return true;
            }
            if (choice.ToLower().StartsWith("q"))
            {
                stop = 2;
                return true;
            }
            stop = 0;
            var split = choice.Split(',');
            if(split.Length < 2) return false;
            if (!int.TryParse(split[1].Trim(), out int row)) return false;
            if (!int.TryParse(split[0].Trim(), out int col)) return false;
            if(!(0 < row && row <= _size && 0 < col && col <= _size)) return false;
            row--;
            col--;
            var bidding = _board[row * _size + col];
            if(!CheckForPlayer(bidding))
            {
                _board.SetValue(player,(row * _size) + col);
                return true;
            } 
            Console.WriteLine("That box is taken.");
            return false;
        }
        // size must be a factor of board.Length
        private static void WriteBoard()
        {
            Console.Clear();
            Console.WriteLine("Type s to skip, r to restart or q to quit.\n");
            var spacer = new string('-', _size * 4 + 2);
            for(var i = 1; i <= _size; i++) Console.Write("---" + i);
            Console.Write("--\n");
            for(var i = 0; i < _size; i++)
            {
                var line = i + 1 + "|";
                for(int j = 0; j < _size; j++) line += " " + GetChar(_board[i * _size + j]) + " |";
                Console.WriteLine(line);
                Console.WriteLine(spacer);
            }
        }
        private static char GetChar(int n)
        {
            return n == 1 ? 'O' : n == 2 ? 'X' : ' ';
        }
        private static char GetPlayer(int current)
        {
            return GetChar(current);
        }
        private static void CheckForWin(out int winner)
        {
            for(var i = 0; i < _size; i++)
            {
                List<int> verticalSquares = new List<int>(),
                    horizontalSquares = new List<int>();
                for (var col = 0; col < _size; col++)
                {
                    verticalSquares.Add(_board[i + col * _size]);
                    horizontalSquares.Add(_board[(i * _size) + col]);
                }

                if (SquaresMatch(horizontalSquares.ToArray()) && CheckForPlayer(horizontalSquares[0]))
                {
                    winner = horizontalSquares[0];
                    return;
                }
                if (SquaresMatch(verticalSquares.ToArray()) && CheckForPlayer(verticalSquares[0]))
                {
                    winner = verticalSquares[0];
                    return;
                }
            }
            var squaresFromDiagonal = new List<int>();
            for(var i = 0; i < _size; i++)
            {
                squaresFromDiagonal.Add(_board[i * (_size + 1)]);
            }
            if (SquaresMatch(squaresFromDiagonal.ToArray()) && squaresFromDiagonal.TrueForAll(CheckForPlayer))
            {
                winner = squaresFromDiagonal[0];
                return;
            }
            squaresFromDiagonal = new List<int>();
            for (var i = _size - 1; i <= _size * (_size - 1); i += _size - 1) squaresFromDiagonal.Add(_board[i]);
            if (SquaresMatch(squaresFromDiagonal.ToArray()) && squaresFromDiagonal.TrueForAll(CheckForPlayer))
            {
                winner = squaresFromDiagonal[0];
                return;
            }
            if (!Array.TrueForAll(_board, CheckForPlayer))
            {
                winner = -1;
                return;
            }
            winner = 0;
        }
        private static bool CheckForPlayer(int n)
        {
            return n == 1 || n == 2;
        }
        private static bool SquaresMatch(params int[] squares)
        {
            var previous = squares[0];
            foreach (var square in squares)
            {
                if (previous != square) return false;
                previous = square;
            }
            return true;
        }
        private static int[] Empty()
        {
            var list = new List<int>();
            for(var i = 0; i < _size * _size; i++) list.Add(GetChar(0));
            return list.ToArray();
        }
    }
}
