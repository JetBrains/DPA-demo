using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Sudoku
{
  class Program
  {
    static void Main(string[] args)
    {

/*      if (args.Length == 0)
      {
        Console.WriteLine("Please provide input board");
        return;
      }

      var filePath = args[0];
*/

      var filePath = @"..\..\..\Data\sudoku.txt";

      if (!File.Exists(filePath))
      {
        Console.WriteLine("File doesn't exist");
        return;
      }

      var inputBoard = CreateInputBoard(filePath);

      var SW = new Stopwatch();
      SW.Start();
      var resultBoard = Solver.Solve(inputBoard, out var steps);
      SW.Stop();

      CheckSolution(resultBoard);

      Console.WriteLine($"Solved in {steps} steps, took {SW.Elapsed}");

      PrintResultBoard(resultBoard);
    }

    private static Board CreateInputBoard(string filePath)
    {
      var boardData = new int[9][];
      using var reader = File.OpenText(filePath);
      for (var i = 0; i < 9; i++)
      {
        string line;
        do
        {
          line = reader.ReadLine();
        } while (!line?.Contains(",") ?? false);

        var numbers = line.Split(",")
          .Select(s => int.TryParse(s, out var value) ? value : NumberEx.Unknown)
          .ToArray();
        
        if (numbers.Length != 9)
          throw new ArgumentException("Invalid data format");

        boardData[i] = numbers;
      }

      return Board.CreateInitialBoard(boardData);
    }

    private static void CheckSolution(Board board)
    {
      for (var row = 0; row < 9; row++)
      {
        var rowHashset = new HashSet<int>();
        for (var col = 0; col < 9; col++)
          if (!rowHashset.Add(board.Cells[row, col].Number))
            throw new InvalidOperationException();
      }

      for (var col = 0; col < 9; col++)
      {
        var colHashset = new HashSet<int>();
        for (var row = 0; row < 9; row++)
          if (!colHashset.Add(board.Cells[row, col].Number))
            throw new InvalidOperationException();
      }
    }

    private static void PrintResultBoard(Board resultBoard)
    {
      for (var row = 0; row < 9; row++)
      {
        for (var col = 0; col < 9; col++)
        {
          Console.Write(resultBoard.Cells[row, col].Number);
          Console.Write(", ");
        }

        Console.WriteLine();
      }
    }
  }
}