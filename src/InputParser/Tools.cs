using System.Diagnostics;
using System.Numerics;

namespace InputParser;

public static class Tools
{
  public static IEnumerable<IEnumerable<T>> MutateRemovingSingleEntries<T>(this T[] ints) =>
    ints.Select((_, i) => ints[..i].Concat(ints[(i + 1)..]));


  public static char[][] GetRowsAsCharArrays(this string[] lines)
  {
    return lines.Select(r => r.ToArray()).ToArray();
  }

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

  public static int[][] GetIntArrayOfRows(this string[] lines, string delimiter=" ") => GetArrayOfRows(lines, int.Parse, delimiter);
  public static long[][] GetLongArrayOfRows(this string[] lines, string delimiter=" ") => GetArrayOfRows(lines, long.Parse, delimiter);
  
  public static T[][] GetArrayOfRows<T>(this string[] lines, Func<string, T> parse, string delimiter=" ") => GetRowsAsStringArrays(lines, delimiter).Select(x => x.Select(parse).ToArray()).ToArray();
  

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

  public static (T x, T y)[] CartesianProduct<T>(this IEnumerable<T> s) where T : IComparable<T>
  {
    var a = s.ToArray();
    return a.SelectMany(x => a.Where(y => y.CompareTo(x) < 0).Select(y => (x, y)).ToArray()).ToArray();
  }
  
  public static IEnumerable<(int start, int length, T value)> GroupByBlocksOfContiguousEqualElements<T>(this T[] disk) where T : IEquatable<T>
  {
    if (disk.Length==0) 
      yield break;
    
    var currentFileId = disk[0];
    var startPos = 0;
    for(var i = 0; i < disk.Length; i++)
    {
      if (disk[i].Equals(currentFileId)) 
        continue;
      
      if (i != 0) yield return (startPos, i - startPos, currentFileId);
      currentFileId = disk[i];
      startPos = i;
    }
    yield return (startPos, disk.Length - startPos, currentFileId);
  }

  public static int NumberOfDigits(this int i) => i switch { 0 => 1, _ => (int)Math.Log10(i) + 1 };
  public static int NumberOfDigits(this long i) => i switch { 0 => 1, _ => (int)Math.Log10(i) + 1 };
  
  public static string TimeStamp(this Stopwatch sw)
  {
    var elapsedMs = sw.ElapsedMilliseconds;
    sw.Restart();
    return $"({elapsedMs}ms)";
  }
  
  public static void Repeat(int count, Action<int> action)
  {
    for (var i = 0; i < count; i++)
      action(i);
  }

  public static void Do<T>(this IEnumerable<T> seq, Action<T> action)
  {
    foreach (var i in seq)
    {
      action(i);
    }
  }
  
  public static IEnumerable<BigInteger> Range(BigInteger start, BigInteger count)
  {
    var max = start + count;
    for (var i = start; i < max; i++)
      yield return i;
  }
}