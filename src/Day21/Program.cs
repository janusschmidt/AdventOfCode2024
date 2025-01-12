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
    
    var totalComplexity = 0L;
    foreach (var code in codes.Reverse().Take(1))
    {
      var codeAsInteger = long.Parse(code[..^1]);
      Console.WriteLine($"Code: {code}");
      var mappedStrings = numPad.Map(new[]{code}.ToList());
      
      foreach (var mappedString in mappedStrings)
      {
        var complexity = mappedString.Length * codeAsInteger;
        Console.WriteLine($"        {mappedString} (Length: {mappedString.Length}  (Complexity {complexity}))");  
      }
      
      for (var i = 0; i < 2; i++)
      {
        mappedStrings = dirPad.Map(mappedStrings);
        
        foreach (var mappedString in mappedStrings)
        {
          var complexity = mappedString.Length * codeAsInteger;
          Console.WriteLine($"i: {i}  {mappedString} (Length: {mappedString.Length}  (Complexity {complexity}))");  
        }

        Console.WriteLine();
      }
        
      // totalComplexity += complexity;
      // Console.WriteLine($"Complexity: {complexity} (length: {code.Last().Length}, code: {codeAsInteger})");
      Console.WriteLine();
    }
    
    Console.WriteLine($"Total complexity: {totalComplexity}");
    Console.WriteLine();
  }
}