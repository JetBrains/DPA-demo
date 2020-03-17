using System;
using System.Collections.Generic;

namespace Sudoku
{
  public static class Solver
  {
    public static Board Solve(Board inputBoard, out int steps)
    {
      steps = 0;
      
      var stack = new Stack<Board>();
      stack.Push(inputBoard);

      while (stack.Count > 0)
      {
        steps++;
        if(steps % 100_000 == 0)
          Console.WriteLine($"{steps} steps performed");
        
        var board = stack.Peek();
        var copy = board.TryNextNumber();

        if (copy == null)
          stack.Pop();
        else
        {
          if (copy.IsSolved)
            return copy;
          stack.Push(copy);
        }
      }
      
      // if we are there, sudoku is not solved
      return null;
    }
  }
}