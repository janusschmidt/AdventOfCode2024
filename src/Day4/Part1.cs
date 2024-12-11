using InputParser;

namespace Day4;

public static class Part1
{
  public static void Count()
  {
    var rows = FileReader.ReadLines();
    var columns = rows.GetColumnsAsStrings("");

    var linecount = rows.Length;
    var spaces = new string(' ', linecount);
    var columnsIndented = rows.Select((l, i) => spaces[..i] + l + spaces[i..]).ToArray().GetColumnsAsStrings("");
    var columnsReverseIndented = rows.Select((l, i) => spaces[..i] + l.ReverseString() + spaces[i..]).ToArray().GetColumnsAsStrings("");

    Console.WriteLine($"Count horizontal:");
    var horizontal = Count(rows);

    Console.WriteLine($"Count vertical:");
    var vertical = Count(columns);

    Console.WriteLine($"Count vertical indented:");
    var verticalIndented = Count(columnsIndented);

    Console.WriteLine($"Count vertical indented reverse:");
    var verticalIndentedReverse = Count(columnsReverseIndented);

    Console.WriteLine($"Part 1 Sum: {Sum(horizontal, vertical, verticalIndented, verticalIndentedReverse)}");
  }

  static int CountXmas(string s) => s.Split("XMAS").Length - 1;

  static (int normalCount, int reverseCount) Count(string[] strings)
  {
    var count = strings.Sum(CountXmas);
    var countReverse = strings.Select(x => x.ReverseString()).Sum(CountXmas);

    Console.WriteLine($"Count: {count}");
    Console.WriteLine($"Count reverse: {countReverse}");

    return (normalCount: count, reverseCount: countReverse);
  }

  static int Sum(params (int normalCount, int reverseCount)[]  c) => c.Sum(x => x.normalCount + x.reverseCount);
}