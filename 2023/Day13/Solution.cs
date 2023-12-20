using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Utils;

namespace AdventOfCode.Y2023.Day13;

[ProblemName("Point of Incidence")]
class Solution : Solver
{
    // Thx to https://github.com/dpvdberg/aoc2023/blob/master/AdventOfCode2023/Days/Day13/Day13Solution.cs
    // Elegant solution, I was trying to do something similar but I was not able to get it to work
    private readonly Dictionary<int, (int num, bool isHorizontal)> mirrors = new();

    public object PartOne(string input)
    {
        var blocks = ParseBlocks(input);
        var result = 0;
        for (var i = 0; i < blocks.Length; i++)
        {
            TryFindMirror(blocks[i], i, out var res);
            result += res;
        }

        return result;
    }

    private void TryFindMirror(string block, int key, out int result)
    {
        var lines = block.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var columns = block.SplitIntoColumns().ToList();

        for (var i = 1; i < lines.Length; i++)
        {
            if (lines.Take(i).Reverse().Zip(lines.Skip(i)).All(x => x.First == x.Second))
            {
                if (mirrors.TryGetValue(key, out (int num, bool isHorizontal) value))
                {
                    if (value.isHorizontal && value.num == i) continue;
                }

                mirrors[key] = (i, true);
                result = i * 100;
                return;
            }
        }

        for (var i = 1; i < columns.Count; i++)
        {
            if (columns.Take(i).Reverse().Zip(columns.Skip(i)).All(x => x.First == x.Second))
            {
                if (mirrors.TryGetValue(key, out (int num, bool isRow) value))
                {
                    if (!value.isRow && value.num == i) continue;
                }

                mirrors[key] = (i, false);
                result = i;
                return;
            }
        }

        result = 0;
    }

    public object PartTwo(string input)
    {
        return 0;
    }

    private static string[] ParseBlocks(string input) =>
        input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
}