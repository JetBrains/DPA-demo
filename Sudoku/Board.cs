using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
  public class Board
  {
    public readonly Cell[,] Cells;

    private readonly int _emptyCellsCount;
    
    public static Board CreateInitialBoard(int[][] boardData) => new Board(boardData);
    private Board(IReadOnlyList<int[]> boardData)
    {
      _emptyCellsCount = 9 * 9;
      Cells = new Cell[9,9];
      for (var row = 0; row < 9; row++)
      for (var col = 0; col < 9; col++)
        Cell(row, col) = new Cell(boardData[row][col]);

      // update all board with impossible options based on the initial numbers
      for (var row = 0; row < 9; row++)
      for (var col = 0; col < 9; col++)
      {
        var cell = Cell(row, col);
        if (cell.IsFilled) // update other affected by filled
        {
          _emptyCellsCount--;
          UpdateCellsAffectedBy(row, col, cell.Number);
        }
      }
    }

    /// <summary>
    /// MakeUpdatedCopy constructor
    /// </summary>
    private Board(Cell[,] cells, int activeRow, int activeCol, int activeNumber, int emptyCellsCount)
    {
      Cells = cells;
      _emptyCellsCount = emptyCellsCount;
      Cell(activeRow, activeCol).Number = activeNumber;
      UpdateCellsAffectedBy(activeRow, activeCol, activeNumber);
    }

    public bool IsSolved => _emptyCellsCount == 0;
    
    /// <summary>
    /// Makes a copy of board with the next possible number set into the active cell 
    /// </summary>
    /// <returns></returns>
    public Board TryNextNumber()
    {
      GetCellToTry(out var activeRow, out var activeCol);
      var number = Cell(activeRow, activeCol).PopNextNumber();
      return number != NumberEx.Unknown 
        ? new Board(CopyCells(), activeRow, activeCol, number, _emptyCellsCount - 1)  
        : null; // we tried all numbers and all branches finished by dead end
    }

    private Cell[,] CopyCells()
    {
      var cellsCopy = new Cell[9, 9];
      for (var row = 0; row < 9; row++)
      for (var col = 0; col < 9; col++)
        cellsCopy[row, col] = Cell(row, col).Copy();
      return cellsCopy;
    }

    private void UpdateCellsAffectedBy(int sourceRow, int sourceCol, int number)
    {
      var squareBeginRow = (sourceRow / 3) * 3;
      var squareBeginCol = (sourceCol / 3) * 3;

      var squareEndRow = squareBeginRow + 3;
      var squareEndCol = squareBeginCol + 3;

      for (var row = 0; row < 9; row++)
      for (var col = 0; col < 9; col++)
      {
        if (row == sourceRow || col == sourceCol ||
            ((squareBeginRow <= row && row < squareEndRow) && (squareBeginCol <= col && col < squareEndCol)))
        {
          if (!Cell(row, col).IsFilled)
            Cell(row, col).RemoveOption(number);
        }
      }
    }

    private ref Cell Cell(int row, int col) => ref Cells[row, col];
    
    /// <summary>
    /// Now it returns the first empty cell, it can be optimized
    /// </summary>
    private void GetCellToTry(out int row, out int col)
    {
      for (row = 0; row < 9; row++)
      for (col = 0; col < 9; col++)
        if (!Cell(row, col).IsFilled)
          return;
      throw new InvalidOperationException();
    }

    /// <summary>
    /// For debug purpose
    /// </summary>
    public override string ToString()
    {
      var sb = new StringBuilder();
      for (var row = 0; row < 9; row++)
      {
        for (var col = 0; col < 9; col++)
        {
          var cell = Cell(row, col);
          sb.Append(cell.Number);
          sb.Append(", ");
        }

        sb.AppendLine();
      }
        
      return sb.ToString();
    }
  }
}