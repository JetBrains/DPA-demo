using System;
using System.Linq;

namespace Sudoku
{
  public class Cell
  {
    /// <summary>
    /// Flags enum contains what numbers are possible to set to this cell
    /// </summary>
    private NumberSet _options;
    
    public int Number { get; set; }

    public Cell(int number) : this(number, NumberSet._Any)
    { }

    /// <summary>
    /// Copy constructor
    /// </summary>
    private Cell(int number, NumberSet options)
    {
      Number = number;
      _options = options;
    }
    public Cell Copy() => new Cell(Number, _options);

    public bool IsFilled => Number != NumberEx.Unknown;

    public void RemoveOption(int number) => _options.Remove(number);

    /// <summary>
    /// Gets first available number from the options, but not set it to <see cref="Number"/>
    /// </summary>
    public int PopNextNumber()
    {
      if (IsFilled) throw new InvalidOperationException();

      var options = _options;
      var number = Enumerable.Range(1, 9).FirstOrDefault(_ => options.Contains(_));

      // number == NumberEx.Unknown means we tried all numbers and all branches finished by dead end
      if (number != NumberEx.Unknown) 
        RemoveOption(number);

      return number; 
    }
    
    /// <summary>
    /// For debug purpose
    /// </summary>
    public override string ToString() => $"{Number}";
  }
}