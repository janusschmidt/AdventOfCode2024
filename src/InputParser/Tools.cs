namespace InputParser;

public static class Tools
{
  public static IEnumerable<IEnumerable<T>> MutateRemovingSingleEntries<T>(this T[] ints) =>
    ints.Select((_, i) => ints[..i].Concat(ints[(i + 1)..]));

  public static string[][] GetStringArrayOfRowsAsArrays(this string[] lines)
  {
    return lines.Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ToArray();
  }

  public static int[][] GetIntArrayOfRowsAsArrays(this string[] lines)
  {
    return GetStringArrayOfRowsAsArrays(lines).Select(x => x.Select(int.Parse).ToArray()).ToArray();
  }

  public static int[][] GetIntArrayOfColumnsAsArrays(this string[] lines)
  {
    var arr = GetIntArrayOfRowsAsArrays(lines);
    var maxCols = arr.Max(x => x.Length);
    return Enumerable.Range(0, maxCols).Select(index => arr.Select(x => x.ElementAtOrDefault(index)).ToArray())
      .ToArray();
  }
}