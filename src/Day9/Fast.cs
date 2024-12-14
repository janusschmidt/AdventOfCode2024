using System.Diagnostics;

namespace Day9;

static class Fast
{
  public static long[] Part1(long[] disk)
  {
    var i = 0;
    var j = disk.Length - 1;
    var newDisk = new long[disk.Length];

    while (i <= j)
    {
      var front = disk[i];
      var end = disk[j];
      
      if (front != -1) newDisk[i++] = front;
      else
      {
        if (end != -1) newDisk[i++] = end;
        j--;
      }
    }
    return newDisk;
  }

  public static long[] Part2(long[] disk)
  {
    var newDisk = disk.ToArray();
    var i = 0;
    var j = newDisk.Length - 1;

    while (i <= j)
    { 
      var front = newDisk[i];
      var end = newDisk[j];

      if (front != -1)
      {
        i++;
        continue;
      }
      
      if (end != -1)
      {
        var k = j;
        while (newDisk[j] == end) j--;
        var blockSize = k - j++;
        for (var i2 = i; i2 <= k; i2++)
        {
          if (newDisk[i2] != -1) continue;
            
          var l = i2;
          while (newDisk[i2] == -1) i2++;
          var freeBlockSize = i2 - l;
          if (freeBlockSize < blockSize) continue;
          
          for (var k2 = j; k2 <= k; k2++)
            newDisk[k2] = -1;
              
          for (var l2 = 0; l2 < blockSize; l2++)
            newDisk[l + l2] = end;
            
          break;
        }
      }
      j--;
    }
    return newDisk;
  }
}