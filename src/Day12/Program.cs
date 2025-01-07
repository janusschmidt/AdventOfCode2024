using System.Diagnostics;
using InputParser;

namespace Day12;

static class Program
{
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();
    var rows = FileReader.ReadLines().GetRowsAsStringArrays("");
    var regions = ParseRegions(rows);

    Console.WriteLine($"Part1: Fence price {ComputePrice(regions)}  {sw.TimeStamp()}");
  }

  static int ComputePrice(IEnumerable<(int perimeter, int area, string letter)> regions) => regions.Sum(x => x.area * x.perimeter);

  static IEnumerable<(int perimeter, int area, string letter)> ParseRegions(string[][] raw)
  {
    var rows = raw.Select(r => r.Select(c => (value: c, visited: false)).ToArray()).ToArray();

    var numberOfRows = rows.Length;
    var numberOfCols = rows[0].Length;

    for (int i = 0; i < numberOfRows; i++)
    {
      for (int j = 0; j < numberOfCols; j++)
      {
        if (!rows[i][j].visited)
          yield return GetRegion(i, j, rows);
      }
    }
  }

  static (int perimeter, int area, string letter) GetRegion(int i, int j, (string value, bool visited)[][] rows)
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

    var perimeter = plotsInRegion.Sum(x => NumberOfSides(x, plotsInRegion, rows.Length, rows[0].Length));
    var area = plotsInRegion.Count;
    return (perimeter, area, letter);

  }

  static int NumberOfSides((int row, int col) c, List<(int row, int col)> plotsInRegion, int rowsLength, int colsLenght)
  {
    return
      CountSide(c with { row = c.row + 1 }) +
      CountSide(c with { row = c.row - 1 }) +
      CountSide(c with { col = c.col + 1}) +
      CountSide(c with { col = c.col - 1 });

    int CountSide((int row, int col) s) => IsOutOfBounds(s, rowsLength, colsLenght ) || !plotsInRegion.Contains(s) ? 1 : 0;
  }

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