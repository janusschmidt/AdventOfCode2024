namespace InputParser;

public static class Tools
{
  public static IEnumerable<IEnumerable<T>> MutateRemovingSingleEntries<T>(this T[] ints) =>
    ints.Select((_, i) => ints[..i].Concat(ints[(i + 1)..]));

  
  public static string[][] GetRowsAsStringArrays(this string[] lines, string delimiter=" ")
  {
    return lines.Select(x => delimiter == "" ? x.ToStringArray() : x.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)).ToArray();
  }
  
  public static string?[][] GetColumnsAsStringArrays(this string[] lines, string delimiter=" ")
  {
    var arr = GetRowsAsStringArrays(lines, delimiter);
    var maxCols = arr.Max(x => x.Length);
    return Enumerable.Range(0, maxCols)
      .Select(index => arr.Select(x => x.ElementAtOrDefault(index)).ToArray())
      .ToArray();
  }
  
  public static string[] GetColumnsAsStrings(this string[] lines, string delimiter=" ") => GetColumnsAsStringArrays(lines, delimiter).Select(x => string.Concat(x)).ToArray();

  public static int[][] GetIntArrayOfRows(this string[] lines, string delimiter=" ") => GetRowsAsStringArrays(lines, delimiter).Select(x => x.Select(int.Parse).ToArray()).ToArray();

  public static int[][] GetIntArrayOfColumns(this string[] lines, string delimiter=" ") => GetColumnsAsStringArrays(lines, delimiter).Select(x => x.Select(s => int.Parse(s ?? "0")).ToArray()).ToArray();

  public static string ReverseString(this string str) => new(str.Reverse().ToArray());
  
  public static string[] ToStringArray(this string s) => s.Select(y => y.ToString()).ToArray();

  public static string[][] SplitOnEmptyLines(this string[] lines)
  {
    List<string[]> sections = [];
    List<string> agg = [];
    foreach (var line in lines)
    {
      if (line == "")
      {
        if (agg.Count != 0)
        {
          sections.Add(agg.ToArray());
          agg = [];
        }
        continue;
      }
      agg.Add(line);
    }
    if (agg.Count != 0)
      sections.Add(agg.ToArray());
    
    return sections.ToArray();
  }

  public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
  {
    return source.Select(predicate).All(x => x);
  }
}