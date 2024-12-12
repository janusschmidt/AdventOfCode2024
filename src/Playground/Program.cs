using System.Numerics;

Vector<int> a = Vector<int>.Zero.WithElement(0,1).WithElement(1,2);
Vector<int> b = Vector<int>.Zero.WithElement(0,0).WithElement(1,3);

Console.WriteLine(Vector<int>.Count);
Console.WriteLine(a);
Console.WriteLine(b);
var dims = Vector.LessThan(a,b);
		
Console.WriteLine(dims);