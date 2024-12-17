using System.Numerics;
using InputParser;

Vector<int> a = Vector<int>.Zero.WithElement(0,1).WithElement(1,2);
Vector<int> b = Vector<int>.Zero.WithElement(0,0).WithElement(1,3);

Console.WriteLine(Vector<int>.Count);
Console.WriteLine(a);
Console.WriteLine(b);
var dims = Vector.LessThan(a,b);
Console.WriteLine(dims);

int[] ints = [1, 2, 3, 4, 5, 0, 0, 0, -1, -1];
var expandedReverse = new Stack<int>(ints.Where(x => x != -1));
var maxFreeSlotsToFill = expandedReverse.SkipWhile(x => x == -1);//.Count(x => x == -1);

for (var i = 1; i < 1; i+=2)
{
  Console.WriteLine("UPS");
}
foreach (var i in maxFreeSlotsToFill)
{
  Console.WriteLine(i);  
}

Console.WriteLine($" (-3 + 10) modulo 10 {(-3 + 10) % 10}");
// for (int i = 0; i < 1001; i+=5)
// {
//   Console.WriteLine($"{i}: {i.NumberOfDigits()}");
// }
