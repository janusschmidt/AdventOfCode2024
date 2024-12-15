using System.Diagnostics;
using System.Numerics;
using InputParser;

namespace day11;

static class Program
{
  static readonly Dictionary<long, long[]> Computed25 = new();

  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();
    var numbers = FileReader.ReadLines().GetLongArrayOfRows()[0].ToArray();
    
    Console.WriteLine(string.Join(" ", numbers));

    Add25LevelsToDict(numbers, sw);
    Add25LevelsToDict(Computed25.Values.SelectMany(x =>x).ToArray(), sw);
    Add25LevelsToDict(Computed25.Values.SelectMany(x =>x).ToArray(), sw);
    
    Console.WriteLine($"Part1: {ComputeCount(numbers, 1)}  {sw.TimeStamp()}");  //185894
    Console.WriteLine($"Part2: {ComputeCount(numbers, 3)}  {sw.TimeStamp()}");  //221632504974231
  }

  static BigInteger ComputeCount(long[] numbers, int maxDepth, int depth = 1)
  {
    return depth >= maxDepth ? 
      numbers.Select(n => (BigInteger) Computed25[n].Length).Aggregate(BigInteger.Add) : 
      numbers.AsParallel().Select(n => ComputeCount(Computed25[n], maxDepth, depth + 1)).Aggregate(BigInteger.Add);
  }
  
  static void Add25LevelsToDict(long[] numbers, Stopwatch sw)
  {
    foreach (var number in numbers)
    {
      if (Computed25.ContainsKey(number))
        continue;
    
      long[] expanded = [number];
      for (var i = 0; i < 25; i++)
      {
        expanded = Expand(expanded).ToArray();
      }
      Computed25[number] = expanded;
    }

    Console.WriteLine($"Sofar: {Computed25.Count}, unique values: {Computed25.Values.SelectMany(x => x).Distinct().Count()}  {sw.TimeStamp()}");
  }

  static IEnumerable<long> Expand(IEnumerable<long> numbers)
  {
    foreach (var n in numbers)
    {
      if (n == 0)
      {
        yield return 1;
      }
      else
      {
        var noDigits = n.NumberOfDigits();
        if (n.NumberOfDigits() % 2 == 0)
        {
          var fact = (long) Math.Pow(10, noDigits / 2);
          var first = n / fact;
          var second = n - first * fact;
          yield return first;
          yield return second;
        }
        else
        {
          yield return n * 2024;
        }
      }
    }
  }
}