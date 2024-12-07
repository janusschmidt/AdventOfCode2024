namespace InputParser;

public class Parser
{
    string[] lines;
    
    public Parser(string filename = "input.txt")
    {
        var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
        lines = File.ReadAllLines(Path.Combine(projectDir, filename));
    }

    public string[][] GetStringArrayOfRowsAsArrays()
    {
        return lines.Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ToArray();        
    }
    
    public int[][] GetIntArrayOfRowsAsArrays()
    {
        return GetStringArrayOfRowsAsArrays().Select(x => x.Select(int.Parse).ToArray()).ToArray();        
    }

    public int[][] GetIntArrayOfColumnsAsArrays()
    {
        var arr = GetIntArrayOfRowsAsArrays();
        var maxCols = arr.Max(x => x.Length);
        return Enumerable.Range(0, maxCols).Select(index => arr.Select(x => x.ElementAtOrDefault(index)).ToArray()).ToArray();
    }
}