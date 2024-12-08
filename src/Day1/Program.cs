using InputParser;

var input = FileReader.ReadLines().GetIntArrayOfColumns();

Console.WriteLine($"Distance: {GetTotalDistance(input[0], input[1])}");
Console.WriteLine($"Similarity: {GetTotalSimilarity(input[0], input[1])}");

return;

int GetTotalDistance(int[] a1, int[] a2)
{
  var zipped = a1.OrderBy(x => x).Zip(a2.OrderBy(x => x));
  return zipped.Sum(x => Math.Abs(x.First - x.Second));
}

int GetTotalSimilarity(int[] a1, int[] a2)
{
  var repeats = a2.GroupBy(x => x).ToDictionary(x => x.Key, g => g.Count());
  return a1.Sum(x => repeats.GetValueOrDefault(x, 0) * x);
}