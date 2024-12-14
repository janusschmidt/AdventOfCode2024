using System.Diagnostics;
using InputParser;

namespace Day9;

static class Program
{
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();

    //var input = FileReader.ReadLines("test.txt")[0];
    var input = FileReader.ReadLines()[0];
    var raw = input.Select(x => long.Parse(x.ToString())).ToArray();

    var disk = Expand(raw);
    Console.WriteLine($"Result part1 fast: {Fast.Part1(disk).Select(CheckSum).Sum()}({sw.ElapsedMilliseconds} milliseconds)"); sw.Restart();
    Console.WriteLine($"Result part2 fast: {Fast.Part2(disk).Select(CheckSum).Sum()}({sw.ElapsedMilliseconds} milliseconds)"); sw.Restart();
    Console.WriteLine($"Result part1 elegant: {Elegant.Defrag(disk, Elegant.GroupBySectors).Select(CheckSum).Sum()}({sw.ElapsedMilliseconds} milliseconds)"); sw.Restart();
    Console.WriteLine($"Result part2 elegant: {Elegant.Defrag(disk, Elegant.GroupByBlocks).Select(CheckSum).Sum()}({sw.ElapsedMilliseconds} milliseconds)"); sw.Restart();
    
    //Result part1 fast: 6291146824486(31 milliseconds)
    //Result part2 fast: 6307279963620(61 milliseconds)
    //Result part1 elegant: 6291146824486(43084 milliseconds)
    //Result part2 elegant: 6307279963620(3462 milliseconds)
  }

  static long CheckSum(long x, int i) => x < 1 ? 0 : x * i;
  static long[] Expand(long[] raw) => raw.SelectMany((v, i) => Enumerable.Range(0, (int)v).Select(_ => i % 2 == 0 ?  i/2 : -1L)).ToArray();
}