using System.Diagnostics;
using System.Numerics;
using InputParser;

namespace Day8;

static class Program
{
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();

    //var input = FileReader.ReadLines("test.txt");
    var input = FileReader.ReadLines();
    
    var bounds = GetBounds(input);

    var antennas = input
      .SelectMany((x, ix) => x.Select((c, iy) => (ix, iy, c)))
      .Where(x => !".#".Contains(x.c)).Select(x => new Antenna(x.ix, x.iy, x.c))
      .ToArray();
    
    var antennasGrouped = antennas.GroupBy(x => x.AntennaType).ToDictionary(x => x.Key, g => g.CartesianProduct().Select(t => new AntennaPair(t)).ToArray());
    var part1AntiNodes = antennasGrouped.SelectMany(g => g.Value.SelectMany(p=>p.Part1AntiNodes(bounds))).Distinct().ToArray();
    var part2AntiNodes = antennasGrouped.SelectMany(g => g.Value.SelectMany(p=>p.Part2AntiNodes(bounds))).Distinct().ToArray();

    Console.WriteLine($"Part1 Sum: {part1AntiNodes.Length}");
    Console.WriteLine($"Part2 Sum: {part2AntiNodes.Length}");
    Console.WriteLine($"({sw.ElapsedMilliseconds} milliseconds)");
    
    // Part1 Sum: 308
    // Part2 Sum: 1147
    // (21 milliseconds)
  }

  static bool IsInBounds(Vector<int> v, Vector<int> bounds)
  {
    var lessThanOrEqualAll = Vector.LessThanOrEqualAll(Vector<int>.Zero, v);
    var greaterThanOrEqualAll = Vector.LessThanOrEqualAll(v, bounds);
    return lessThanOrEqualAll && greaterThanOrEqualAll;
  }

  static Vector<int> GetBounds(string[] input) => Vector<int>.Zero.WithElement(0,input.Length - 1).WithElement(1, input[0].Length - 1);

  record Antenna : IComparable<Antenna>
  {
    readonly int x;
    readonly int y;
    public readonly char AntennaType;
    public readonly Vector<int> Vector;

    public Antenna(int x, int y, char antennaType)
    {
      this.x = x;
      this.y = y;
      AntennaType = antennaType;
      Vector = Vector<int>.Zero.WithElement(0,x).WithElement(1,y);
    }


    public int CompareTo(Antenna? other)
    {
      if (ReferenceEquals(this, other)) return 0;
      if (other is null) return 1;
      var xComparison = x.CompareTo(other.x);
      if (xComparison != 0) return xComparison;
      return y.CompareTo(other.y);
    }
  }

  record AntennaPair
  {
    public readonly Antenna A1;
    public readonly Antenna A2;
    readonly Vector<int> diff;

    public AntennaPair((Antenna a1, Antenna a2) t) 
    {
      A1 = t.a1;
      A2 = t.a2;
      diff = A1.Vector - A2.Vector;
    }

    public Vector<int>[] Part1AntiNodes(Vector<int> bounds)
    {
      return ((Vector<int>[])[A1.Vector + diff, A2.Vector - diff])
        .Where(x => IsInBounds(x, bounds))
        .ToArray();
    }
    
    public Vector<int>[] Part2AntiNodes(Vector<int> bounds)
    {
      return Enumerable.Range(-bounds.GetElement(0), bounds.GetElement(0) * 2 + 1)
        .Select(i => A1.Vector + i * diff)
        .Where(v => IsInBounds(v, bounds))
        .ToArray();  
    }
  }
}