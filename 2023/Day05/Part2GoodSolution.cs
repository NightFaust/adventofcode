namespace AdventOfCode.Y2023.Day05;

/// <summary>
/// Credits to https://topaz.github.io/paste/#XQAAAQCuCQAAAAAAAAA6nMlWi076alCx9N1TsRv/nE/5+HEJqrfgdiKbnpiCU30HOqctQj7Jp3T1Xt0zuyBzJjB+ise8j8R3bmPwIDTyo8c++CYGfLfshnb0jcBhyL/6GbQ1a8Qtge1cQaPvZR3+xe3RZDpCKCOWiFPJPImoJaIv3pnwNtjg2obpk/dPK02M9A2hxkZd/N5weImKnXk5rvmjdEgOMfaRpJas/b+kM41Xpp7e/43hP/fTVinr/M3K2Kq8Gz0k43OrCba4eaSxjWl4lqI9Z58GkRGp+zttskYw5R707lpxp1bpHIKRYc5WoPlZp6pzW5zfyjTuDZ5oDMyvcve1HW2mTqtjVbh49A0soaVCTXNB343GcmTi4R81Zs09idT7fxw5EpuVztX7rgEUJK8z172X4cMvaNfoDmoADuXK0qHUFpxP8lmZjL/JRY+zyPto6KsCnOvRvuC8N7C4YjJDW3EfWy44sIEcbIuD1C1tDGXCHL+Z6xIoedxLAZ9BaEYTUHhgyoQBvfJtNt7N2VbhmIpFoMxe3Ir+YHPwWaLT/YpJHsJz2/B3/+OcwJ9eUF/8qXWIjRM8abM3HjM5Rx/vDalZoJQ0zRBu4vRYwhWIwUIH5mgRHVrVsMWambDbsmKfhFeJ+TUsMqY6AI2p4l6OsRG+4om2ccJ7V4SJXykFXOOfB4iazn9U02VWUiV1Ac+mnovRunLCMuN6KHITD0mH18+x7Le5r9Wv7kBO+KDGMIxowVAinxfdfeNgamFzGOFR/BEd89OmXr6ftGokpHfg7YiCaC/ISuidD+eBrdRs+db4xu2d/9sIlWR1jDcygVHCauYDFKo8lGgbYjOTVqaFG7XSylOc3UaZi6DUWB4OcUyEC59Eraugm5tYK2ih2FqaS9s38BI7vbQmJkWJ6qOR1yGVKkQolTUfRGcqA+Ps+O9b6HwTW/6yIRfvw4CLqS8CDUiTFqYLQ2EDRqpqz2DsYfg6MraPqaHwQYrdqi6CKB/nD7op7fqvuRC7aMiMR7fWtLBP7AumvNyFBsxUhLE7cZBxJ1Jt0UL7zq9TY4psV0FZpfbIbl87vMSL348OEhIstut7z6opBRdeh//ZPcoH
/// </summary>
public class Part2GoodSolution
{
    /*
     * using System.Diagnostics;
       var input = File.ReadAllLines("input.txt");
       
       var watch = Stopwatch.StartNew();
       
       var seeds = input[0].Split(' ').Skip(1).Select(x => long.Parse(x)).ToList();
       var maps = new List<List<(long from, long to, long adjustment)>>();
       List<(long from, long to, long adjustment)>? currmap = null;
       foreach (var line in input.Skip(2))
       {
       if (line.EndsWith(':'))
       {
       currmap = new List<(long from, long to, long adjustment)>();
       continue;
       }
       else if (line.Length == 0 && currmap != null)
       {
       maps.Add(currmap!);
       currmap = null;
       continue;
       }
       
       var nums = line.Split(' ').Select(x => long.Parse(x)).ToArray();
       currmap!.Add((nums[1], nums[1] + nums[2] - 1, nums[0] - nums[1]));
       }
       if (currmap != null)
       maps.Add(currmap);
       
       // Part 1
       var result1 = long.MaxValue;
       foreach (var seed in seeds)
       {
       var value = seed;
       foreach (var map in maps)
       {
       foreach (var item in map)
       {
       if (value >= item.from && value <= item.to)
       {
       value += item.adjustment;
       break;
       }
       }
       }
       result1 = Math.Min(result1, value);
       }
       
       // Part 2
       var ranges = new List<(long from, long to)>();
       for (int i = 0; i < seeds.Count; i += 2)
       ranges.Add((from: seeds[i], to: seeds[i] + seeds[i + 1] - 1));
       
       foreach (var map in maps)
       {
       var orderedmap = map.OrderBy(x => x.from).ToList();
       
       var newranges = new List<(long from, long to)>();
       foreach (var r in ranges)
       {
       var range = r;
       foreach (var mapping in orderedmap)
       {
       if (range.from < mapping.from)
       {
       newranges.Add((range.from, Math.Min(range.to, mapping.from - 1)));
       range.from = mapping.from;
       if (range.from > range.to)
       break;
       }
       
       if (range.from <= mapping.to)
       {
       newranges.Add((range.from + mapping.adjustment, Math.Min(range.to, mapping.to) + mapping.adjustment));
       range.from = mapping.to + 1;
       if (range.from > range.to)
       break;
       }
       }
       if (range.from <= range.to)
       newranges.Add(range);
       }
       ranges = newranges;
       }
       var result2 = ranges.Min(r => r.from);
       
       watch.Stop();
       
       Console.WriteLine($"Result1 = {result1}");
       Console.WriteLine($"Result2 = {result2}");
       Console.WriteLine($"Took = {watch.ElapsedMilliseconds}ms");
     */
}