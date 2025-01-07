using System.Diagnostics;
using InputParser;

namespace Day12;

static class Program
{
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();
    var rows = FileReader.ReadLines().GetRowsAsStringArrays("");
    var regions = ParseRegions(rows).ToArray();

    var part1Price = regions.Sum(x => x.Length * x.Sum(p => p.sides.Length));
    Console.WriteLine($"Part1: Fence price {part1Price}  {sw.TimeStamp()}"); // 1450422
    
    var part2Price = regions.Sum(r => r.Length * r.Sum(p => CalcSidesPart2(p, r)));
    Console.WriteLine($"Part2: Fence price {part2Price}  {sw.TimeStamp()}"); // 906606
  }

  static int CalcSidesPart2(((int row, int col) p, Side[] sides) plot, ((int row, int col) p, Side[] sides)[] region)
  {
    return plot.sides.Count(s =>
      s is Side.Bottom or Side.Top && !region.Any(p2 => p2.p.row == plot.p.row && p2.p.col == plot.p.col - 1 && p2.sides.Any(s2 => s2 == s)) || 
      s is Side.Left or Side.Right && !region.Any(p2 => p2.p.row == plot.p.row - 1 && p2.p.col == plot.p.col && p2.sides.Any(s2 => s2 == s))
    );
  }

  static IEnumerable<((int row, int col) p, Side[] sides)[]> ParseRegions(string[][] raw)
  {
    var rows = raw.Select(r => r.Select(c => (value: c, visited: false)).ToArray()).ToArray();

    var numberOfRows = rows.Length;
    var numberOfCols = rows[0].Length;

    for (var i = 0; i < numberOfRows; i++)
    {
      for (var j = 0; j < numberOfCols; j++)
      {
        if (rows[i][j].visited) 
          continue;
        
        yield return GetRegion(i, j, rows);
      }
    }
  }


  static ((int row, int col) p, Side[] sides)[] GetRegion(int i, int j, (string value, bool visited)[][] rows)
  {
    var letter = rows[i][j].value;
    var plotsToProcess = new Queue<(int row, int col)>();
    var plotsInRegion = new List<(int row, int col)>();
    plotsToProcess.Enqueue((i,j));
    rows[i][j].visited = true;

    while (plotsToProcess.TryDequeue(out var current))
    {
      plotsInRegion.Add(current);
      AddNeighbourPlotsOfSameType(current, rows, letter, plotsToProcess);
    }

    return plotsInRegion.Select(p => (p, sides: GetSides(plotsInRegion, p, rows.Length, rows[0].Length))).ToArray();
  }

  static Side[] GetSides(List<(int row, int col)> plotsInRegion, (int row, int col) plot, int rowsLength, int colsLenght)
  {
    return new[] {
      HasSide(Side.Bottom, plot with { row = plot.row + 1 }),
      HasSide(Side.Top, plot with { row = plot.row - 1 }),
      HasSide(Side.Right, plot with { col = plot.col + 1}),
      HasSide(Side.Left, plot with { col = plot.col - 1})
    }.Where(x => x != Side.None).ToArray();

    Side HasSide(Side side, (int row, int col) s) => IsOutOfBounds(s, rowsLength, colsLenght ) || !plotsInRegion.Contains(s) ? side : Side.None;
  }
  
  enum Side { Left, Right, Top, Bottom, None }

  static void AddNeighbourPlotsOfSameType(
    (int row, int col) current,
    (string value, bool visited)[][] rows,
    string letter,
    Queue<(int row, int col)> plotsToProcess)
  {
    Check(current with {row = current.row + 1});
    Check(current with {row = current.row - 1});
    Check(current with {col = current.col + 1});
    Check(current with {col = current.col - 1});
    return;

    void Check((int row, int col) coords)
    {
      if (IsOutOfBounds(coords, rows.Length, rows[0].Length))
        return;

      var plotToCheck = rows[coords.row][coords.col];
      if (plotToCheck.visited || plotToCheck.value != letter) return;

      rows[coords.row][coords.col].visited = true;
      plotsToProcess.Enqueue(coords);
    }
  }

  static bool IsOutOfBounds((int row, int col) coords, int numberOfRows, int numberOfCols)
  {
    var (row, col) = coords;
    return row < 0 || col < 0 || row >= numberOfRows || col >= numberOfCols;
  }
}