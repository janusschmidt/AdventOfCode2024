using Day1;

var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
var input = File.ReadAllLines(Path.Combine(projectDir, "input.txt"));

var numbers = input.Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ToList();
var a1 = numbers.Select(x => int.Parse(x[0])).ToArray();
var a2 = numbers.Select(x => int.Parse(x[1])).ToArray();

Console.WriteLine($"Distance: {Part1.GetTotalDistance(a1, a2)}");

Console.WriteLine($"Similarity: {Part2.GetTotalSimilarity(a1, a2)}");
