using System.Diagnostics;
using InputParser;

namespace day10;

class Program
{
  static readonly int[] TwoDirections = [1, -1];

  public static void Main(string[] args)
  {
    var map = FileReader.ReadLines().GetIntArrayOfRows("");

    var sw = Stopwatch.StartNew();

    var trailHeads = GetTrailHeads(map);
    var routesPerTrailHead = trailHeads.Select(h => GetPossibleRoutes(map, h)).ToArray();
    Console.WriteLine($"Part 1: {routesPerTrailHead.Select(r => r.Distinct().Count()).Sum()} ({sw.ElapsedMilliseconds} ms)");
    Console.WriteLine($"Part 2: {routesPerTrailHead.Select(r => r.Count()).Sum()} ({sw.ElapsedMilliseconds} ms)");
  }

  static IEnumerable<Point> GetPossibleRoutes(int[][] map, Point p)
  {
    var height = map[p.X][p.Y];
    return height == 9 ? [p] : GetPossibleUphillPositions(map, p).SelectMany(pp => GetPossibleRoutes(map, pp));
  }

  static IEnumerable<Point> GetPossibleUphillPositions(int[][] map, Point head)
  {
    var newPoints = TwoDirections.Select(d => head + new Point(d,0)).Concat(TwoDirections.Select(d => head + new Point(0, d)));
    return newPoints.Where(p => IsInBounds(p, map)).Where(p => IsUpHill(map, p, head));
  }

  static bool IsUpHill(int[][] map, Point p, Point p2) => map[p.X][p.Y] == map[p2.X][p2.Y] + 1;

  static bool IsInBounds(Point p, int[][] map) => !(p.X < 0 || p.Y < 0 || p.X >= map.Length || p.Y >= map[0].Length);

  static IEnumerable<Point> GetTrailHeads(int[][] map) => map.SelectMany((row, x) => row.Select((_, y) => new Point(x, y))).Where(p => map[p.X][p.Y] == 0);

  record Point(int X, int Y)
  {
    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);  
  }
}