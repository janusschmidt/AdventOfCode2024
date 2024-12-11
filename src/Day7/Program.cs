using System.Diagnostics;

using InputParser;

namespace Day7;

static class Program
{
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();

    //var input = FileReader.ReadLines("test.txt");
    var input = FileReader.ReadLines();

    var equations = input.Select(ParseEquation).ToArray();
    var maxOperands = equations.Select(x => x.Operands.Length).Max();

    Operator[] operatorsPart1 = [new((i, j) => i + j), new((i, j) => i * j)];
    var operatorsPart2 = operatorsPart1.Concat([new((i, j) => long.Parse(i.ToString() + j.ToString()))]).ToArray();

    CalculateSum(maxOperands, operatorsPart1, equations, sw);
    CalculateSum(maxOperands, operatorsPart2, equations, sw);

    // Sum: 6392012777720 (504 milliseconds)
    // Sum: 61561126043536 (9268 milliseconds)
  }

  static void CalculateSum(int maxOperands, Operator[] operators, Equation[] equations, Stopwatch sw)
  {
    sw.Restart();
    var combinationsOfDifferentLengths = Enumerable.Range(1, maxOperands).Select(l => Combinations(l, operators)).ToArray();
    var sum = equations.Where(equation => CheckEquation(equation, combinationsOfDifferentLengths)).Sum(x => x.Sum);
    Console.WriteLine($"Sum: {sum} ({sw.ElapsedMilliseconds} milliseconds)");
  }

  static bool CheckEquation(Equation e, IEnumerable<IEnumerable<Operator>>[] combinationsOfDifferentLengths)
  {
    return combinationsOfDifferentLengths[e.Operands.Length-1].AsParallel().Any(
      c => c.Aggregate((sum: (long)0, Index: 0), (agg, op) => (sum: op.Calculate(agg.sum,  e.Operands[agg.Index]), Index: agg.Index + 1)).sum == e.Sum);
  }

  static IEnumerable<IEnumerable<Operator>> Combinations(int depth, Operator[] operators)
  {
    if (depth <= 1)
      return operators.Select(x => new[] {x});

    return Combinations(depth-1, operators).SelectMany(c => operators.Select(o => c.Concat([o])));
  }

  static Equation ParseEquation(string line)
  {
    var sections = line.Split(':');
    return new(long.Parse(sections[0]), sections[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray());
  }

  record Equation(long Sum, long[] Operands);

  class Operator(Func<long, long, long> method)
  {
    public long Calculate(long i, long j) => method(i, j);
  }
}