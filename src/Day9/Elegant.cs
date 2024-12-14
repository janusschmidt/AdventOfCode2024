using InputParser;

namespace Day9;

static class Elegant
{
  public static long[] Defrag(long[] originalDisk, Func<long[], IEnumerable<ArraySegment<long>>> groupByBlocks)
  {
    var disk = originalDisk.ToArray();
    var blocks = groupByBlocks(disk).ToArray();
    var files = blocks.Where(x => x[0] != -1).Reverse().ToArray();
    foreach (var file in files)
    {
      var firstFreeBlock = groupByBlocks(disk).FirstOrDefault(x => x[0] == -1 && x.Count >= file.Count && x.Offset < file.Offset);
      if (firstFreeBlock == default) continue;
      for (var i = 0; i < file.Count; i++)
      {
        firstFreeBlock[i] = file[i];
        file[i] = -1;
      }
    }
    return disk;
  }

  public static IEnumerable<ArraySegment<long>> GroupByBlocks(long[] disk) => disk.GroupByBlocksOfEqualElements().Select(x => new ArraySegment<long>(disk, x.start, x.length));
  

  public static IEnumerable<ArraySegment<long>> GroupBySectors(long[] disk) => disk.Select((_,i) => new ArraySegment<long>(disk, i, 1));
}