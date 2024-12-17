using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;
using InputParser;

namespace Day14;

static class Program
{
  const int movesToTry = 20000;
  
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();

    // var lines = FileReader.ReadLines("test1.txt");
    // var bounds = new Xy(11,7);

    var lines = FileReader.ReadLines();
    var bounds = new Xy(101, 103); //new Xy(11,7));

    var robots = lines.Select(x => new Robot(x)).ToArray();
    Part1(bounds, robots, sw); //216772608

    Console.WriteLine($"Part 2:");
    
    robots = lines.Select(x => new Robot(x)).ToArray(); //reset
    var movesToGetTree = CalcMovesToFirstTree(robots, bounds); //6888

    robots = lines.Select(x => new Robot(x)).ToArray(); //reset
    MoveAndPrintTree(robots, bounds, movesToGetTree, sw);
  }
  
  static void Part1(Xy bounds, Robot[] robots, Stopwatch sw)
  {
    Tools.Repeat(100, _ => robots.Do(r => r.Move(bounds)));
    var calcPart1Score = CalcPart1Score(bounds, robots).ToArray();
    var numberOfRobotsInQuadrants = calcPart1Score.Aggregate((x, y) => x * y);
    Console.WriteLine($"Part1: Safety factor {numberOfRobotsInQuadrants} {sw.TimeStamp()}");
  }

  static int CalcMovesToFirstTree(Robot[] robots, Xy bounds)
  {
    var w = Enumerable.Range(1, movesToTry).Select(i =>
    {
      robots.Do(r => r.Move(bounds));
      return (Index: i, score: Math.Floor(BigInteger.Log10(CalcScorePerSector(bounds, robots, 10).Aggregate((x, y) => x * y))));
    }).ToArray();

    var mean = w.Sum(x => x.score) / movesToTry;
    Console.WriteLine($"Mean score: {mean}");
    
    var firstOfLowestScoring = w.OrderBy(x => x.score).ThenBy(x => x.Index).First();
    Console.WriteLine($"Index: {firstOfLowestScoring.Index} has a score of {firstOfLowestScoring.score}, which is {100 - 100 * firstOfLowestScoring.score / mean} percent away from the mean score.");
    
    return firstOfLowestScoring.Index;
  }

  static void MoveAndPrintTree(Robot[] robots, Xy bounds, int moves, Stopwatch sw)
  {
    Tools.Repeat(moves, _ => robots.Do(r => r.Move(bounds)));
    PrintMap(bounds, robots, sw);
  }

  static IEnumerable<BigInteger> CalcScorePerSector(Xy bounds, Robot[] robots, int divisions)
  {
    var xInterval = (decimal) bounds.X / divisions;
    var yInterval = (decimal) bounds.Y / divisions;
    for (var x = 0; x < divisions; x++)
    {
      var robotsInThisXRange = robots.Where(r => r.Position.X >= x * xInterval && r.Position.X < xInterval * (x + 1)).ToArray();
      for (var y = 0; y < divisions; y++)
      {
        var dims = robotsInThisXRange.Count(r => r.Position.Y >= y * yInterval && r.Position.Y < yInterval * (y + 1)) + 1;
        yield return dims;
      }
    }
  }
  
  static IEnumerable<int> CalcPart1Score(Xy bounds, Robot[] robots)
  {
    yield return robots.Count(r => r.Position.X < bounds.X / 2 && r.Position.Y < bounds.Y / 2);
    yield return robots.Count(r => r.Position.X < bounds.X / 2 && r.Position.Y > bounds.Y / 2);
    yield return robots.Count(r => r.Position.X > bounds.X / 2 && r.Position.Y < bounds.Y / 2);
    yield return robots.Count(r => r.Position.X > bounds.X / 2 && r.Position.Y > bounds.Y / 2);
  }

  static void PrintMap(Xy bounds, Robot[] robots, Stopwatch sw)
  {
    Console.WriteLine(string.Join("",Enumerable.Repeat(1, bounds.X).Select(_ => '-')));
    for (var y = 0; y < bounds.Y; y++)
    {
      var robotsOnX = robots.Where(r => r.Position.Y == y).ToArray();
      for(var x = 0; x < bounds.X; x++)
      {
        Console.Write(robotsOnX.Count(r => r.Position.X == x));
      }
      Console.WriteLine();
    }
    Console.WriteLine($"(elapsed: {sw.ElapsedMilliseconds} ms)");
    sw.Restart();
  }

  class Robot
  {
    public Xy Position;
    readonly Xy velocity;
    
    public Robot(string line)
    {
      var matches = Regex.Matches(line,"p=(.*),(.*) v=(.*),(.*)");
      Position = new Xy(Get(1), Get(2));
      velocity = new Xy(Get(3), Get(4));
      return;
      int Get(int i) => int.Parse(matches[0].Groups[i].Value);
    }

    public void Move(Xy bounds)
    {
      var newPosition = Position + velocity;
      Position = new ((newPosition.X + bounds.X) % bounds.X, (newPosition.Y + bounds.Y) % bounds.Y);
    }
  }

  record Xy(int X, int Y)
  {
    public static Xy operator +(Xy a, Xy b) => new(a.X + b.X, a.Y + b.Y);
  };
}