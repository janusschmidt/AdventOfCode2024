namespace InputParser;

public class FileReader
{
    public static string[] ReadLines(string filename = "input.txt")
    {
        var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
        return File.ReadAllLines(Path.Combine(projectDir, filename));
    }
    
    public static string ReadTextFile(string filename = "input.txt")
    {
        var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
        return File.ReadAllText(Path.Combine(projectDir, filename));
    }
}