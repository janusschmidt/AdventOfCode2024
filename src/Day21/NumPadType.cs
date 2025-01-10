namespace Day21;

public class NumPadType : IPadType
{
  public char[][] Pad { get; } =
  [
    ['7','8','9'],
    ['4','5','6'],
    ['1','2','3'],
    [' ','0','A']
  ];
  
  public int PadRows { get; } = 4;
  public int PadCols { get; } = 3;
}