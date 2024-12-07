namespace Day1;

public class Part2
{
    public static int GetTotalSimilarity(int[] a1, int[] a2)
    {
        var repeats = a2.GroupBy(x => x).ToDictionary(x => x.Key, g => g.Count());
        
        return a1.Sum(x => repeats.GetValueOrDefault(x, 0) * x);
    }
}