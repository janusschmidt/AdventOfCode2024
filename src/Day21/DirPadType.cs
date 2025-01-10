namespace Day21;

public class DirPadType : IPadType
{
  public char[][] Pad { get; } =
  [
    [' ', '^', 'A'],
    ['<', 'v', '>'],
  ];
  
  public int PadRows { get; } = 2;
  public int PadCols { get; } = 3;
}