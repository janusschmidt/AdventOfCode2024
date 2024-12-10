using InputParser;

//var input = FileReader.ReadLines("test.txt").Select(x => x.Select(y => y).ToArray()).ToArray();

class Program
{
  public static void Main(string[] args)
  {
    var input = FileReader.ReadLines().Select(x => x.Select(y => y).ToArray()).ToArray();
    var visitedStates = GameLoop(input);
    Console.WriteLine($"Part1 - Number of points: {visitedStates.Select(s => (s.X, s.Y)).Distinct().Count()}");
  }

  static State GetInitialState(char[][] cells) => cells
    .SelectMany((x, ix) => x.Select((y, iy) => new State(ix, iy, GetDirection(y))))
    .Single(x => x.Direction != Direction.Unknown);

  static Direction GetDirection(char c) => c switch
  {
    '^' => Direction.Up,
    '>' => Direction.Right,
    'v' => Direction.Down,
    '<' => Direction.Left,
    _ => Direction.Unknown
  };

  static char[][] ClearGuardFromInput(char[][] chars) => chars.Select(x => x.Select(y => "^<>v.".Contains(y) ? '.' : '#').ToArray()).ToArray();
  
  static HashSet<State> GameLoop(char[][] input)
  {
    var state = GetInitialState(input);
    input = ClearGuardFromInput(input);
    var bounds = new Bounds(input);
    
    HashSet<State> visitedStates = [];
    while (bounds.IsInBounds(state))
    {
      visitedStates.Add(state);
      var newState = MoveForward(state);

      state = !bounds.IsInBounds(newState) || input[newState.X][newState.Y] == '.' ? 
        newState : 
        state with { Direction = TurnRight(state) };
    }
    return visitedStates;
  }

  static State MoveForward(State state) =>
    state.Direction switch
    {
      Direction.Up => state with { X = state.X - 1 },
      Direction.Down => state with { X = state.X + 1 },
      Direction.Left => state with { Y = state.Y - 1 },
      Direction.Right => state with { Y = state.Y + 1 },
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
}

enum Direction { Up, Down, Left, Right, Unknown }
record State(int X, int Y, Direction Direction);

record Bounds
{
  readonly int maxX;
  readonly int maxY;

  public Bounds(char[][] cells)
  {
    maxX = cells.Length;
    maxY = cells[0].Length;
  }
  public bool IsInBounds(State state) => 0 <= state.X && state.X < maxX && 0 <= state.Y && state.Y < maxY;
}