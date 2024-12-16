using System.Diagnostics;
using System.Text.RegularExpressions;
using InputParser;

namespace Day13;

static class Program
{
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();
    var lines = FileReader.ReadLines();

    Console.WriteLine($"Part1 score: {ParseSets(lines).Sum(x => x.Score)}  {sw.TimeStamp()}"); // 25751
    Console.WriteLine($"Part2 score: {ParseSets(lines, 10_000_000_000_000).Sum(x => x.Score)}  {sw.TimeStamp()}"); //108528956728655
  }

  static ClawSet[] ParseSets(string[] lines, long offset = 0)
  {
    var setsRaw = lines.SplitOnEmptyLines();
    return setsRaw.Select(s => new ClawSet(s, offset)).ToArray();
  }


  record Xy(decimal X, decimal Y)
  {
    public readonly decimal Slope = 1m * Y / X;
  }

  class ClawSet
  {
    readonly Xy buttonA;
    readonly Xy buttonB;
    readonly Xy price;
    readonly bool hasMaxPresses;

    decimal IntersectionX => (price.Y - price.X * buttonB.Slope) / (buttonA.Slope - buttonB.Slope);
    decimal ButtonATimes => IntersectionX / buttonA.X;
    decimal ButtonBTimes => (price.X - IntersectionX) / buttonB.X;
    bool ACanReachIntersection => IsAlmostInteger(ButtonATimes) && (!hasMaxPresses || decimal.Round(ButtonATimes, 15) <= 100);
    bool BCanReachIntersection => IsAlmostInteger(ButtonBTimes) && (!hasMaxPresses || decimal.Round(ButtonBTimes, 15) <= 100);
    static bool IsAlmostInteger(decimal d) => Math.Abs(decimal.Round(d) - d) < (decimal) Math.Pow(10, -15);
    bool CanReachIntersection => ACanReachIntersection && BCanReachIntersection;
    public long Score => CanReachIntersection ? (long)(3 * decimal.Round(ButtonATimes, 15) + decimal.Round(ButtonBTimes, 15)) : 0;
    
    public ClawSet(string[] section, long offset)
    {
      buttonA = ParseButton(section[0]);
      buttonB = ParseButton(section[1]);
      price = ParseButton(section[2]);
      price = price with { X = price.X + offset, Y = price.Y + offset };
      hasMaxPresses = offset == 0;
    }

    static Xy ParseButton(string s)
    {
      var m = Regex.Matches(s, "([0-9]+)");
      return new Xy(long.Parse(m[0].Value), long.Parse(m[1].Value));
    }
  }
}