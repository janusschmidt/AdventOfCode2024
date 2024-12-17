using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;
using InputParser;

namespace Day17;

static class Program
{
  public static void Main(string[] args)
  {
    var sw = Stopwatch.StartNew();
    var lines = FileReader.ReadLines();
    
    var register = new Register(lines);
    var program = Regex.Match(lines[4], @"Program: ((?:[0-9]+,)*\d+)").Groups[1].Value.Split(',').Select(int.Parse).ToArray();

    Part1(register, program, sw);
    Console.WriteLine($"Part2: Register A value = {GetValuesReplicatingProgram(0, new Register(lines), program, 1).Min()} {sw.TimeStamp()}");
    
    // Part1: output = 4,1,5,3,1,5,3,5,7 (24ms)
    // Part2: Register A value = 164542125272765 (81ms)
  }

  static BigInteger[] GetValuesReplicatingProgram(BigInteger soFar, Register register, int[] program, int depth)
  {
    if (depth > program.Length)
      return [soFar];

    var toMatch = program[^depth..];

    var res = Tools.Range(0, 8).Select(i =>
      {
        var v = soFar << 3 ^ i;
        var r = new Register(v, register.B, register.C);
        var sm1 = new StateMachine(r, program);
        sm1.Run();
        return (Candidate: v, sm1.Output);
      })
      .Where(x => x.Output.Select(y => (int)y).SequenceEqual(toMatch))
      .ToArray();
    
    return res.Length == 0 ? [] : res.SelectMany(x => GetValuesReplicatingProgram(x.Candidate, register, program, depth + 1)).ToArray();
  }

  static void Part1(Register register, int[] program, Stopwatch sw)
  {
    var sm = new StateMachine(register, program);
    sm.Run();
    Console.WriteLine($"Part1: output = {string.Join(",", sm.Output)} {sw.TimeStamp()}");
  }
}

class StateMachine(Register register, int[] program)
{
  public readonly List<BigInteger> Output = new();
  
  public void Run()
  {
    var instructionPointer = 0;
    while (instructionPointer < program.Length)
    {
      var opcode = program[instructionPointer];
      var operand = program[instructionPointer + 1];

      switch ((Opcodes)opcode)
      {
        case Opcodes.Adv:
          register.A >>= (int)GetCombo(operand);
          break;
        case Opcodes.Bxl:
          register.B ^= operand;
          break;
        case Opcodes.Bst:
          register.B = GetCombo(operand) & 7;
          break;
        case Opcodes.Jnz when register.A == 0:
          break;
        case Opcodes.Jnz:
          instructionPointer = operand - 2;
          break;
        case Opcodes.Bxc:
          register.B ^= register.C;
          break;
        case Opcodes.Out:
          var output = GetCombo(operand) & 7;
          Output.Add(output);
          break;
        case Opcodes.Bdv:
          register.B =  register.A >> (int)GetCombo(operand);
          break;
        case Opcodes.Cdv:
          register.C = register.A >> (int)GetCombo(operand);
          break;
      }

      instructionPointer += 2;
    }
  }
  
  BigInteger GetCombo(int operand) => operand switch
  {
    < 4 => operand,
    4 => register.A,
    5 => register.B,
    6 => register.C,
    _ => throw new Exception($"Unknown opcode {operand}")
  };
}

enum Opcodes
{
  Adv = 0,
  Bxl = 1,
  Bst = 2,
  Jnz = 3,
  Bxc = 4,
  Out = 5,
  Bdv = 6,
  Cdv = 7
}

class Register
{
  public BigInteger A;
  public BigInteger B;
  public BigInteger C;
  
  public Register(string[] lines)
  {
    A = BigInteger.Parse(Regex.Match(lines[0], @"Register A: (\d+)").Groups[1].Value);
    B = BigInteger.Parse(Regex.Match(lines[1], @"Register B: (\d+)").Groups[1].Value);
    C = BigInteger.Parse(Regex.Match(lines[2], @"Register C: (\d+)").Groups[1].Value);
  }

  public Register(BigInteger a, BigInteger b, BigInteger c)
  {
    A = a;
    B = b;
    C = c;
  }
};

// 2,4 bst B = A & 7
// 1,1 bxl B = B ^ 1
// 7,5 cdv C = A >> B
// 1,5 bxl B = B ^ 5
// 0,3 adv A = A >> B  
// 4,3 bxc B = B ^ C
// 5,5 out => B & 7
// 3,0 jnz => hop til 0 (kun hvis A er != 0)
