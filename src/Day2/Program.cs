using InputParser;

var parser = new Parser();
var input = parser.GetIntArrayOfRowsAsArrays();

var safeCount = CountSafe(input);

Console.WriteLine($"Safe count: {safeCount}");

int CountSafe(int[][] ints)
{
    return ints.Count(x => x.Aggregate(
        new Agg(true, Direction.Unknown, null),
        (agg, i) =>
        {
            var (isSafe, direction, previousValue) = agg;
            var unSafe = new Agg(false, Direction.Unknown, i);
            var safe = new Agg(true, direction, i);
            
            return !isSafe ? agg : 
                previousValue == null ? safe :
                previousValue - i == 0 ? unSafe :
                Math.Abs(previousValue.Value - i) > 3 ? unSafe :
                direction == Direction.Unknown ? new Agg(true, previousValue - i > 0 ? Direction.Decreasing : Direction.Increasing, i) :
                previousValue - i > 0 && direction == Direction.Decreasing ? safe :
                previousValue - i < 0 && direction == Direction.Increasing ? safe :
                unSafe;
        }).IsSafe);
}

enum Direction
{
    Increasing,
    Decreasing,
    Unknown
}

record Agg(bool IsSafe, Direction Direction, int? PreviousValue);