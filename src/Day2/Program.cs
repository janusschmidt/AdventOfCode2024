using InputParser;

var input = FileReader.ReadLines().GetIntArrayOfRows();

Console.WriteLine($"Safe count part1: {input.Count(CheckSequence)}");
Console.WriteLine($"Safe count part2: {input.Count(x => MutateRemovingSingleEntries(x).Any(CheckSequence))}");
return;

IEnumerable<IEnumerable<int>> MutateRemovingSingleEntries(int[] ints)
{
  yield return ints;
  foreach (var arr in Tools.MutateRemovingSingleEntries(ints))
  {
    yield return arr;
  }
}

bool CheckSequence(IEnumerable<int> ints)
{
  return ints.Aggregate(
    new Agg(true),
    (agg, i) =>
    {
      var (isSafe, dir, prev) = agg;
      var unSafe = new Agg(false);
      var safe = new Agg(true, dir, i);

      return !isSafe ? agg :
        prev == null ? safe :
        prev - i == 0 ? unSafe :
        Math.Abs(prev.Value - i) > 3 ? unSafe :
        dir == Direction.Unknown ? new Agg(true, prev - i > 0 ? Direction.Decreasing : Direction.Increasing, i) :
        prev - i > 0 && dir == Direction.Decreasing ? safe :
        prev - i < 0 && dir == Direction.Increasing ? safe :
        unSafe;
    }
  ).IsSafe;
}

enum Direction
{
  Increasing,
  Decreasing,
  Unknown
}

record Agg(bool IsSafe, Direction Direction = Direction.Unknown, int? PreviousValue = null);