using InputParser;

//var input = FileReader.ReadLines("testinput.txt").SplitOnEmptyLines();
var input = FileReader.ReadLines().SplitOnEmptyLines();

var rules = input[0].GetIntArrayOfRows("|");
var sectionPages = input[1].GetIntArrayOfRows(",");
Console.WriteLine($"Rules: {rules.Length}, pages: {sectionPages.Length}");

Calculation.Calculate(rules, sectionPages);


static class Calculation
{
  public static void Calculate(int[][] rules, int[][] sectionPages)
  {
    var pageLinesGroupedByCorrectness = sectionPages.ToLookup(pages => pages.SequenceEqual(GetCorrectSequence(pages, rules)));

    var part1Sum = pageLinesGroupedByCorrectness[true].Sum(GetMiddlePage);
    Console.WriteLine($"Part 1 sum: {part1Sum}");


    var part2Sum = pageLinesGroupedByCorrectness[false].Select(pages => GetCorrectSequence(pages, rules)).Sum(GetMiddlePage);
    Console.WriteLine($"Part 2 sum: {part2Sum}");
  }

  static int[] GetOrder(int[][] rules, List<int>? order = null)
  {
    order ??= [];

    if (rules.Length == 0)
      return order.ToArray();

    var rulesWithNoPrevious = rules.Where(x => rules.All(r => r[1] != x[0])).ToArray();
    var pageWithNoPrevious = rulesWithNoPrevious.Select(x => x[0]).Distinct().Single();
    order.Add(pageWithNoPrevious);
    var rulesLeft = rules.Where(r => r[0] != pageWithNoPrevious).ToArray();

    if (rulesLeft.Length != 0)
      return GetOrder(rulesLeft, order);

    order.Add(rulesWithNoPrevious.Single()[1]);
    return order.ToArray();
  }

  static int[] GetCorrectSequence(int[] pages, int[][] rules) => GetOrder(GetRelevantRules(rules, pages)).Where(pages.Contains).ToArray();
  static int[][] GetRelevantRules(int[][] pageOrderRules, int[] pages) => pageOrderRules.Where(x => pages.Contains(x[0]) && pages.Contains(x[1])).ToArray();
  static int GetMiddlePage(int[] pages) => pages[pages.Length / 2];
}