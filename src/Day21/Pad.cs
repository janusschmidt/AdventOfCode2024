namespace Day21;

public class Pad(IPadType padType)
{
  public Dictionary<Move, char[]> AllMoves()
  {
    var res = new Dictionary<Move, char[]>();

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

    Dictionary<Coordinate, char[]> GetShortestMoveSequences(int startRowIndex, int startColIndex)
    {
      Dictionary<Coordinate, (int distance, char[] sequence)> pads = [];
      Queue<CoordinateAndDistance> queue = new();
      queue.Enqueue(new CoordinateAndDistance(new Coordinate(startRowIndex, startColIndex), 0, []));

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

      return pads.ToDictionary(x => x.Key, x=> x.Value.sequence);

      IEnumerable<CoordinateAndDistance> Neighbours(CoordinateAndDistance current)
      {
        CoordinateAndDistance?[] candidates =
        [
          GetNeighbour(current.Coordinate with { Row = current.Coordinate.Row + 1 }, 'v'),
          GetNeighbour(current.Coordinate with { Row = current.Coordinate.Row + -1 }, '^'),
          GetNeighbour(current.Coordinate with { Col = current.Coordinate.Col + 1 }, '>'),
          GetNeighbour(current.Coordinate with { Col = current.Coordinate.Col + -1 }, '<'),
        ];
        return candidates.Where(x => x is not null)!;

        CoordinateAndDistance?  GetNeighbour(Coordinate n, char s) => IsValidCell(n) ? new CoordinateAndDistance(n, current.Distance + 1, current.Sequence.Concat([s]).ToArray())  : null;
        bool IsValidCell(Coordinate x) => x.Row >= 0 && x.Row < padType.PadRows && x.Col >= 0 && x.Col < padType.PadCols && padType.Pad[x.Row][x.Col] != ' ';
      }
    }
  }

  record Coordinate(int Row, int Col);

  record CoordinateAndDistance(Coordinate Coordinate, int Distance, char[] Sequence);

  public record Move(char A, char B);
}