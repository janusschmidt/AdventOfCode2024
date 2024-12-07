using System.Text.RegularExpressions;
using InputParser;

var text = FileReader.ReadTextFile();
var matches = new Regex(@"mul\(([0-9]{1,3}),([0-9]{1,3})\)").Matches(text);
var sum = matches
    .Select(m => (int.Parse(m.Groups[1].Value),int.Parse(m.Groups[2].Value)))
    .Sum(x => x.Item1 * x.Item2);

Console.WriteLine(sum);

// foreach (Match match in matches)
// {
//     Console.WriteLine(match.Groups[0].Value);
//     Console.WriteLine(match.Groups[1].Value);
//     Console.WriteLine(match.Groups[2].Value);
// }
