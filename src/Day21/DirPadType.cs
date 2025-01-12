namespace Day21;

public class DirPadType : IPadType
{
  public char[][] Pad { get; } =
  [
    [' ', '^', 'A'],
    ['<', 'v', '>'],
  ];
  
  public int PadRows => 2;
  public int PadCols => 3;
}
