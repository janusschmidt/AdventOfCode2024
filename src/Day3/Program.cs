// Part1 sum: 187825547
// Part2 sum: 85508223

using System.Text.RegularExpressions;
using InputParser;

namespace Day3
{
  partial class Program
  {
    public static void Main(string[] args)
    {
      var text = FileReader.ReadTextFile();
      var matches = MultRegex().Matches(text);
      var sum = matches
        .Select(m => (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)))
        .Sum(x => x.Item1 * x.Item2);

      Console.WriteLine($"Part1 sum: {sum}");
      Console.WriteLine($"Part2 sum: {Part2Sum(text)}");
    }

    static int Part2Sum(string text)
    {
      var sum = 0;
      var doDont = true;

      for (var i = 0; i < text.Length; i++)
      {
        var remainingText = text[i..];
        var mulMatch = MultAtStartOfTextRegex().Match(remainingText);
        var doMatch = DoRegex().Match(remainingText);
        var dontMatch = DontRegex().Match(remainingText);

        if (doMatch.Success)
          doDont = true;

        if (dontMatch.Success)
          doDont = false;

        if (doDont && mulMatch.Success)
          sum += int.Parse(mulMatch.Groups[1].Value) * int.Parse(mulMatch.Groups[2].Value);
      }

      return sum;
    }

    [GeneratedRegex(@"^mul\(([0-9]{1,3}),([0-9]{1,3})\)")]
    private static partial Regex MultAtStartOfTextRegex();

    [GeneratedRegex(@"^do\(\)")]
    private static partial Regex DoRegex();

    [GeneratedRegex(@"^don't\(\)")]
    private static partial Regex DontRegex();

    [GeneratedRegex(@"mul\(([0-9]{1,3}),([0-9]{1,3})\)")]
    private static partial Regex MultRegex();
  }
}