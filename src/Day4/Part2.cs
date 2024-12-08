using InputParser;

namespace Day4;

public class Part2
{
  public void Count()
  {
    var lines = FileReader.ReadLines();
    var arr = lines.GetRowsAsStringArrays("");
    var total = 0;
    for(var i = 1; i < arr.Length - 1; i++)
    for (var j = 1; j < arr[i].Length - 1; j++)
    {
      if (arr[i][j] != "A")
        continue;
      
      var diag1 = arr[i-1][j-1] + arr[i+1][j+1];
      var diag2 = arr[i-1][j+1] + arr[i+1][j-1];

      if (diag1 is "MS" or "SM" && diag2 is "MS" or "SM")
        total++;
    }

    Console.WriteLine($"Part 2 Sum: {total}");
  }
}