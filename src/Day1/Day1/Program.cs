using Day1;

var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
var input = File.ReadAllLines(Path.Combine(projectDir, "input.txt"));

var numbers = input.Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ToList();
var a1 = numbers.Select(x => int.Parse(x[0])).ToArray();
var a2 = numbers.Select(x => int.Parse(x[1])).ToArray();

Console.Write(Part1.GetTotal(a1, a2));
