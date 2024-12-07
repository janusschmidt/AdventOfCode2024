namespace InputParser;

public class Tools
{
    public static IEnumerable<IEnumerable<T>> MutateRemovingSingleEntries<T>(T[] ints) => 
        ints.Select((_, i) => ints[..i].Concat(ints[(i + 1)..]));
}