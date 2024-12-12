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

    var powers = Enumerable.Range(0, 10).ToDictionary(x => x, x => (long) Math.Pow(10, x));
    Operator[] operatorsPart1 = [new((i, j) => i + j), new((i, j) => i * j)];
    var operatorsPart2 = operatorsPart1.Concat([new((i, j) => i * powers[LengthOfLong(j)] + j)]).ToArray();

    
    CalculateSum(maxOperands, operatorsPart1, equations, sw);
    CalculateSum(maxOperands, operatorsPart2, equations, sw);

    // Sum: 1298300076754 (127 milliseconds)
    // Sum: 248427118972289 (908 milliseconds)
  }

  static int LengthOfLong(long l) => (int)Math.Log10(l) + 1;

  static void CalculateSum(int maxOperands, Operator[] operators, Equation[] equations, Stopwatch sw)
  {
    sw.Restart();
    var combinationsOfDifferentLengths = Combinations(maxOperands, operators).ToArray();
    var sum = equations.Where(equation => CheckEquation(equation, combinationsOfDifferentLengths, operators.Length)).Sum(x => x.Sum);
    Console.WriteLine($"Sum: {sum} ({sw.ElapsedMilliseconds} milliseconds)");
  }

  static bool CheckEquation(Equation e, Operator[][] combinationsOfDifferentLengths, int numberOfOperators)
  {
    return combinationsOfDifferentLengths[..(int)Math.Pow(numberOfOperators, e.Operands.Length - 1)].Any(
      c =>
      {
        var sum = e.Operands[0];
        for(var i = 0; i < e.Operands.Length - 1; i++)
        {
          sum = c[i].Calculate(sum, e.Operands[i + 1]); 
        }
        return sum == e.Sum;
      });
  }


  static Operator[][] Combinations(int depth, Operator[] operators)
  {
    if (depth <= 1)
      return operators.Select(x => new[] {x}).ToArray();

    return Combinations(depth-1, operators).SelectMany(c => operators.Select(o => new [] {o}.Concat(c).ToArray())).ToArray();
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