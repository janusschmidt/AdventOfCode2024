using System.Net;
using System.Text;

namespace Day21;

public class Pad
{
  readonly IPadType padType;
  Dictionary<Move, char[][]> allMoves; 
  
  
  public Pad(IPadType padType)
  {
    this.padType = padType;
    allMoves = AllMoves();
  }
  
  public Dictionary<Move, char[][]> AllMoves()
  {
    var res = new Dictionary<Move, char[][]>();

    for (var rowIndex = 0; rowIndex < padType.PadRows; rowIndex++)
    {
      var row = padType.Pad[rowIndex];
      for (var colIndex = 0; colIndex < padType.PadCols; colIndex++)
      {
        var key = row[colIndex];
        if (key == ' ')
          continue;

        var moves = GetShortestMoveSequences(rowIndex, colIndex);
        foreach (var move in moves)
        {
          res[new Move(key, padType.Pad[move.Key.Row][move.Key.Col])] = move.Value;
        }
      }
    }

    return res;

    Dictionary<Coordinate, char[][]> GetShortestMoveSequences(int startRowIndex, int startColIndex)
    {
      Dictionary<Coordinate, (int distance, char[] sequence)> pads = [];
      Queue<CoordinateAndDistance> queue = new();
      var startCoord = new Coordinate(startRowIndex, startColIndex, Direction.Down);
      queue.Enqueue(new CoordinateAndDistance(startCoord with {Direction = Direction.Down}, 0, []));
      queue.Enqueue(new CoordinateAndDistance(startCoord with {Direction = Direction.Up}, 0, []));
      queue.Enqueue(new CoordinateAndDistance(startCoord with {Direction = Direction.Left}, 0, []));
      queue.Enqueue(new CoordinateAndDistance(startCoord with {Direction = Direction.Right}, 0, []));

      while (queue.TryDequeue(out var current))
      {
        if (pads.TryGetValue(current.Coordinate, out var existing) && existing.distance <= current.Distance)
          continue;

        pads.Add(current.Coordinate, (current.Distance, current.Sequence));
        foreach (var n in Neighbours(current))
        {
          if (!pads.TryGetValue(n.Coordinate, out var v) || v.distance > n.Distance) queue.Enqueue(n);
        }
      }

      return pads
        .GroupBy(x => (x.Key.Row, x.Key.Col))
        .ToDictionary(g => 
          new Coordinate(g.Key.Row, g.Key.Col, Direction.Down), 
          g => g
          .GroupBy(c => c.Value.distance)
          .MinBy(x => x.Key)
          .Select(x => x.Value.sequence)
          .Distinct()
          .ToArray());

      IEnumerable<CoordinateAndDistance> Neighbours(CoordinateAndDistance current)
      {
        CoordinateAndDistance?[] candidates =
        [
          GetNeighbour(current.Coordinate with { Row = current.Coordinate.Row + 1 }, 'v'),
          GetNeighbour(current.Coordinate with { Row = current.Coordinate.Row + -1 }, '^'),
          GetNeighbour(current.Coordinate with { Col = current.Coordinate.Col + 1 }, '>'),
          GetNeighbour(current.Coordinate with { Col = current.Coordinate.Col + -1 }, '<'),
          
          GetDirectionChange(Direction.Up),
          GetDirectionChange(Direction.Down),
          GetDirectionChange(Direction.Left),
          GetDirectionChange(Direction.Right)
        ];

        return candidates.Where(x => x is not null)!;

        CoordinateAndDistance? GetDirectionChange(Direction dir) => dir == current.Coordinate.Direction ? null : current with { Coordinate = current.Coordinate with { Direction = dir }, Distance = current.Distance + 10 };
        CoordinateAndDistance?  GetNeighbour(Coordinate n, char s) => IsValidCell(n, s) ? new CoordinateAndDistance(n, current.Distance + 1, current.Sequence.Concat([s]).ToArray())  : null;
        bool IsValidCell(Coordinate x, char c) => x.Row >= 0 && x.Row < padType.PadRows && x.Col >= 0 && x.Col < padType.PadCols && padType.Pad[x.Row][x.Col] != ' ' && AllowedMoves[x.Direction] == c;
      }
    }
  }

  enum Direction { Up, Down, Left, Right }
  
  Dictionary<Direction, char> AllowedMoves = new()
  {
    [Direction.Up] = '^',
    [Direction.Down] = 'v',
    [Direction.Left] = '<',
    [Direction.Right] = '>'
  };

  record Coordinate(int Row, int Col, Direction Direction);

  record CoordinateAndDistance(Coordinate Coordinate, int Distance, char[] Sequence);

  public record Move(char A, char B);

  readonly Dictionary<string, string> shortestMap = new();
  readonly StringBuilder sb = new();
  public List<string> Map(List<string> prev)
  {
    return prev.SelectMany(MapAll).ToList();
   
    // sb.Clear();
    // var matches = s.Split('A', StringSplitOptions.RemoveEmptyEntries);
    // foreach (var seq in matches)
    // {
    //   var word = seq + 'A';
    //   // if (shortestMap.TryGetValue(word, out var mapped))
    //   //   sb.Append(mapped);
    //   // else
    //   {
    //     var newMap = MapAll(word).MinBy(x => x.Length);
    //     sb.Append(newMap);
    //     // shortestMap.Add(word, newMap);
    //   }
    // }
    //
    // return sb.ToString();
  }
  
  public List<string> MapAll(string code)
  {
    var result = new List<string> {""};
    _ = code.Aggregate('A', (agg, next) =>
    {
      result = allMoves[new Move(agg, next)].SelectMany(x => result.Select(r => $"{r}{string.Join("", x)}A")).ToList();
      return next;
    });
    return result;
  }
}