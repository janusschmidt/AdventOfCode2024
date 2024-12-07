namespace Day1;

public class Part1
{
    public static int GetTotalDistance(int[] a1, int[] a2)
    {
        var arr1Sorted = a1.OrderBy(x => x).ToList();
        var arr2Sorted = a2.OrderBy(x => x).ToList();

        var zipped = arr1Sorted.Zip(arr2Sorted);

        var totalDistance = zipped.Sum(x => Math.Abs(x.First - x.Second));
        
        return totalDistance;
    }
}