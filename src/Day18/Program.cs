using System.Diagnostics;
using InputParser;

namespace Day18;

static class Program
{
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();
    var lines = FileReader.ReadLines().GetIntArrayOfRows(",");

    var xDimension = 71;
    var yDimension = 71;

    var map = new int[xDimension, yDimension];
    
    DropBytesAndReCalculateMap(xDimension, yDimension, map, 1024, lines);
    Console.WriteLine($"Part1: Shortest distance {map[0,0]}  {sw.TimeStamp()}ms");

    var res = FindFirstMazeBlockingByte(lines, xDimension, yDimension, map);

    Console.WriteLine($"Part2: First byte to block maze ({res.x},{res.y}) index {res.index}. Cor  {sw.TimeStamp()}ms");
  }

  static (int index, int x, int y) FindFirstMazeBlockingByte(int[][] lines, int xDimension, int yDimension, int[,] map)
  {
    var i = 1025;
    for (; i < lines.Length; i++)
    {
      DropBytesAndReCalculateMap(xDimension, yDimension, map, i, lines);
      if (map[0, 0] == 10000)
        break;
    }

    i--;

    var highlight = lines[i];
    PrintSolution(map, highlight);
    return (i, highlight[0], highlight[1]);
  }

  static void DropBytesAndReCalculateMap(int xDimension, int yDimension, int[,] map, int numberOfBytesToDrop, int[][] lines)
  {
    for (var i = 0; i < xDimension; i++)
    {
      for (var j = 0; j < yDimension; j++)
        map[i, j] = 10000;
    }
    
    for (var i = 1; i <= numberOfBytesToDrop; i++)
    {
      var p = lines[i-1];
      map[p[0], p[1]] = -1;
    }
    
    UpdateDistances(map, map.GetLength(0), map.GetLength(1), 0);
  }

  static void UpdateDistances(int[,] map, int xDimension, int yDimension, int distanceFromEnd)
  {

    var toProcess = new Queue<(int x, int y, int dist)>();
    toProcess.Enqueue((xDimension - 1, xDimension - 1, distanceFromEnd));
    
    while (toProcess.Count > 0)
    {
      var n = toProcess.Dequeue();

      if (map[n.x, n.y] <= n.dist)
        continue;
      
      map[n.x, n.y] = n.dist;
      CheckedAdd(n.x + 1, n.y, n.dist);
      CheckedAdd(n.x - 1, n.y, n.dist);
      CheckedAdd(n.x, n.y + 1, n.dist);
      CheckedAdd(n.x, n.y - 1, n.dist);
    }

    return;

    void CheckedAdd(int x, int y, int dist)
    {
      if (x < 0 || x >= xDimension || y < 0 || y >= yDimension || map[x, y] <= dist) return;
      toProcess.Enqueue((x, y, dist + 1));
    }
  }
  
  static void PrintSolution(int[,] map, int[] highlight)
  {
    var oldColor = Console.ForegroundColor;
    for(int i = 0, xmax = map.GetLength(0); i < xmax; i++)
    {
      for (int j = 0, ymax = map.GetLength(1); j < ymax; j++)
      {
        char c;
        switch (map[i, j])
        {
          case -1:
            Console.ForegroundColor = ConsoleColor.Blue;
            c = 'X';
            break;
          case 10000:
            Console.ForegroundColor = ConsoleColor.Black;
            c = ' ';
            break;
          default:
            Console.ForegroundColor = ConsoleColor.Yellow;
            c = '.';
            break;
        }

        if (highlight.Length != 0 && highlight[0] == i && highlight[1] == j)
          Console.ForegroundColor = ConsoleColor.Red;

        Console.Write(c);
      }
      Console.WriteLine();
    }
    Console.ForegroundColor = oldColor;
  }
}