using System.Numerics;
using InputParser;

namespace Day22;

static class Program
{
  public static void Main(string[] args)
  {
    var secrets = FileReader.ReadLines().Select(BigInteger.Parse).ToArray();
    
    var evolvedSecrets = secrets.Select(EvolveSecret2000Times).ToArray();
    var sum = evolvedSecrets.Select(x => x.lastSecret).Aggregate(BigInteger.Add);
    Console.WriteLine($"Part1 Sum: {sum}"); //19150344884

    var uniqueTriggers = evolvedSecrets.SelectMany(x => x.triggers.Keys).ToHashSet();
    var bananasFromEachTrigger = uniqueTriggers.AsParallel().Select(t => evolvedSecrets.Select(x => x.triggers.GetValueOrDefault(t)).Aggregate(BigInteger.Add));
    var maxBananas = bananasFromEachTrigger.Max();
    Console.WriteLine($"Part2 MaxBananas: {maxBananas}"); //2121
  }
  
  static (BigInteger lastSecret, Dictionary<BigInteger, BigInteger> triggers)  EvolveSecret2000Times(BigInteger secret)
  {
    var triggersAndPrices = new Dictionary<BigInteger, BigInteger>();
    var trigger = new Queue4();
    var currentPrice = Price(secret);
    for (var i = 0; i < 2000; i++)
    {
      secret = GetNextSecret(secret);
      var newPrice = Price(secret);
      var diff = newPrice - currentPrice;
      trigger.Add(diff);
      currentPrice = newPrice;
      if(i > 2)
        triggersAndPrices.TryAdd(trigger.Id, newPrice);
    }
    return (secret, triggersAndPrices);
  }

  static BigInteger GetNextSecret(BigInteger secret)
  {
    secret = Prune(Mix(secret, secret << 6));
    secret = Prune(Mix(secret, secret >> 5));
    secret = Prune(Mix(secret, secret << 11));
    return secret;
  }

  static BigInteger Price(BigInteger i) => i % new BigInteger(10);
  static readonly BigInteger PruneConstant = 16777216 - 1;
  static BigInteger Prune(BigInteger i) => i & PruneConstant;
  static BigInteger Mix(BigInteger i1, BigInteger i2) => i1 ^ i2;

  record Queue4
  {
    const long bitsToKeep = (1L << 20) - 1;
    public BigInteger Id;
    public void Add(BigInteger i)
    {
      Id = (Id << 5 | (i < 0 ? 16 + BigInteger.Abs(i): i)) & bitsToKeep;
    }
  }
}