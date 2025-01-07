using System.Diagnostics;
using InputParser;

namespace Day19;

static class Program
{
  static readonly Dictionary<string, bool> Cache = new();
  static readonly Dictionary<string, long> CachePossibilities = new();
  
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();
    var lines = FileReader.ReadLines();
    var towels = lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    var designs = lines.Skip(2).ToArray();

    //Console.WriteLine($"Possible {IsPossible(designs[0], towels)}");
    
    var possibleDesigns = designs.Where(x => IsPossible(x, towels));
    Console.WriteLine($"Part1: possible designs {possibleDesigns.Count()}  {sw.TimeStamp()}ms"); //322
    
    var allDesigns = designs.Sum(x => NumberOfPossibilities(x, towels));
    Console.WriteLine($"Part1: possible designs {allDesigns}  {sw.TimeStamp()}ms"); //715514563508258
  }

  static bool IsPossible(string s, string[] towels)
  {
    if (Cache.TryGetValue(s, out var result))
      return result;

    var res = s.Length == 0 || towels.Any(t => s.StartsWith(t) && IsPossible(s[t.Length..], towels));

    Cache[s] = res;
    return res;
  }
  
  static long NumberOfPossibilities(string s, string[] towels)
  {
    if (CachePossibilities.TryGetValue(s, out var result))
      return result;

    var res = s.Length == 0 ? 1 : towels.Where(s.StartsWith).Sum(t => NumberOfPossibilities(s[t.Length..], towels));

    CachePossibilities[s] = res;
    return res;
  }
}