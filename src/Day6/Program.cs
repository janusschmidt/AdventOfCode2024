using System.Diagnostics;
using InputParser;

class Program
{
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();

    //var input = FileReader.ReadLines("test.txt").Select(x => x.Select(y => y).ToArray()).ToArray();
    var input = FileReader.ReadLines().Select(x => x.Select(y => y).ToArray()).ToArray();

    var initialState = GetInitialState(input);
    var map = GetMap(input, null);
    var visitedStates = GameLoop(initialState, map);

    Console.WriteLine($"Part1 - Number of points: {visitedStates.Select(s => s.Position).Distinct().Count()} ({sw.ElapsedMilliseconds} milliseconds)");
    sw.Restart();

    var possibleLoopStates = visitedStates.GroupBy(x => x.Position).AsParallel().Select(x => GameLoop(initialState, GetMap(input, x.Key)));
    Console.WriteLine($"Part2 - Number of possible loop generating positions: {possibleLoopStates.Count(x => x is null)} ({sw.ElapsedMilliseconds} milliseconds)");
  }

  static HashSet<State>? GameLoop(State initialState, char[][] map)
  {
    var bounds = new Bounds(map);

    var state = initialState;
    HashSet<State> visitedStates = [];
    while (bounds.IsInBounds(state))
    {
      if (!visitedStates.Add(state))
        return null;

      var newState = MoveForward(state);

      state = !bounds.IsInBounds(newState) || map[newState.Position.X][newState.Position.Y] == '.' ? 
        newState : 
        state with { Direction = TurnRight(state) };
    }
    return visitedStates;
  }

  static State MoveForward(State state) =>
    state.Direction switch
    {
      Direction.Up => state with { Position = state.Position with { X = state.Position.X - 1 }},
      Direction.Down => state with { Position = state.Position with { X = state.Position.X + 1 }},
      Direction.Left => state with { Position = state.Position with { Y = state.Position.Y - 1 }},
      Direction.Right => state with { Position = state.Position with { Y = state.Position.Y + 1 }},
      _ => throw new ArgumentOutOfRangeException()
    };

  static Direction TurnRight(State state) =>
    state.Direction switch
    {
      Direction.Up => Direction.Right,
      Direction.Right => Direction.Down,
      Direction.Down => Direction.Left,
      Direction.Left => Direction.Up,
      _ => throw new ArgumentOutOfRangeException()
    };
  
  static State GetInitialState(char[][] cells) => cells
    .SelectMany((x, ix) => x.Select((y, iy) => new State(new Position(ix, iy), ParseDirection(y))))
    .Single(x => x.Direction != Direction.Unknown);

  static Direction ParseDirection(char c) => c switch
  {
    '^' => Direction.Up,
    '>' => Direction.Right,
    'v' => Direction.Down,
    '<' => Direction.Left,
    _ => Direction.Unknown
  };

  static char[][] GetMap(char[][] chars, Position? obstruction) => chars.Select((x, ix) => x.Select((y, iy) =>
    "^<>v".Contains(y) || y == '.'  && (obstruction is null || obstruction.X != ix || obstruction.Y != iy) ? '.' : '#').ToArray()).ToArray();
}

enum Direction { Up, Down, Left, Right, Unknown }
record Position(int X, int Y);
record State(Position Position, Direction Direction);
record Bounds
{
  readonly int maxX;
  readonly int maxY;

  public Bounds(char[][] cells)
  {
    maxX = cells.Length;
    maxY = cells[0].Length;
  }
  public bool IsInBounds(State state) => IsInBounds(state.Position);
  public bool IsInBounds(Position p) => 0 <= p.X && p.X < maxX && 0 <= p.Y && p.Y < maxY;
}