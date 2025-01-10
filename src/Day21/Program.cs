using System.Diagnostics;
using System.Text;
using InputParser;

namespace Day21;

static class Program
{
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();
    var codes = FileReader.ReadLines("test1.txt");
    var numPad = new Pad(new NumPadType());
    var dirPad = new Pad(new DirPadType());
    
    var numpadMoves = numPad.AllMoves();
    var dirPadMoves = dirPad.AllMoves();

    var currentMoves = codes.Select(x => new[]{new string(x)}.ToList()).ToList();
    
    foreach (var moves in currentMoves)
    {
      moves.Add(NumSequenceToDirPads(moves.Last().ToCharArray(), numpadMoves));
    }

    for (var i = 0; i < 2; i++)
    {
      foreach (var moves in currentMoves)
      {
        moves.Add(NumSequenceToDirPads(moves.Last().ToCharArray(), dirPadMoves));
      }
    }

    var totalComplexity = 0L;
    foreach (var sequences in currentMoves)
    {
      foreach (var seq in sequences)
      {
        Console.WriteLine($"{seq}");
      }

      var codeAsInteger = long.Parse(sequences.First()[..^1]);
      var complexity = sequences.Last().Length * codeAsInteger;  
      totalComplexity += complexity;
      Console.WriteLine($"Complexity: {complexity} (length: {sequences.Last().Length}, code: {codeAsInteger})");
      Console.WriteLine();
    }
    
    Console.WriteLine($"Total complexity: {totalComplexity}");
    Console.WriteLine();

    // Console.WriteLine($"Part 1: {seq}");
    // Console.WriteLine($"Part1: possible designs {possibleDesigns.Count()}  {sw.TimeStamp()}ms"); //322
    //
    // Console.WriteLine($"Part1: possible designs {allDesigns}  {sw.TimeStamp()}ms"); //715514563508258
  }
    
  static string NumSequenceToDirPads(char[] code, Dictionary<Pad.Move, char[]> moves)
  {
    var sb = new StringBuilder();
    _ = code.Aggregate('A', (agg, next) =>
    {
      sb.Append(string.Join("", moves[new Pad.Move(agg, next)]));
      sb.Append('A');
      return next;
    });
    var s = sb.ToString();
    return s;
  }
}