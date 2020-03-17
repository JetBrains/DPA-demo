using System;

namespace Sudoku
{
    [Flags]
    public enum NumberSet
    {
        _1 = 1 << 1,
        _2 = 1 << 2,
        _3 = 1 << 3,
        _4 = 1 << 4,
        _5 = 1 << 5,
        _6 = 1 << 6,
        _7 = 1 << 7,
        _8 = 1 << 8,
        _9 = 1 << 9,
        _Any = _1 | _2 | _3 | _4 | _5 | _6 | _7 | _8 | _9,  
    }

    public static class NumberEx
    {
        public const int Unknown = 0;
        
        public static void Remove(this ref NumberSet numberSet, int number) => numberSet &= ~(NumberSet)(1 << number);
        
        public static bool Contains(this NumberSet numberSet, int number) => numberSet.HasFlag((NumberSet)(1 << number));
    }
}