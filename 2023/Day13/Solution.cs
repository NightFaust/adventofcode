using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            _ = TryFindMirror(blocks[i], i, out var res);
            result += res;
        }

        return result;
    }

    private bool TryFindMirror(string block, int key, out int result)
    {
        var lines = block.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var columns = block.SplitIntoColumns().ToList();

        for (var i = 1; i < lines.Length; i++)
        {
            if (lines.Take(i).Reverse().Zip(lines.Skip(i)).All(x => x.First == x.Second))
            {
                if (mirrors.TryGetValue(key, out var value))
                {
                    if (value.isHorizontal && value.num == i) continue;
                }

                mirrors[key] = (i, true);
                result = i * 100;
                return true;
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
                return true;
            }
        }

        result = 0;
        return false;
    }

    public object PartTwo(string input)
    {
        var blocks = ParseBlocks(input);
        var result = 0;
        for (var j = 0; j < blocks.Length; j++)
        {
            var block = blocks[j];
            for (var i = 0; i < block.Length; i++)
            {
                if (!".#".Contains(block[i])) continue;
                var sb = new StringBuilder(block);

                sb[i] = sb[i] == '.' ? '#' : '.';
                if (TryFindMirror(sb.ToString(), j, out var res))
                {
                    result += res;
                    break;
                }
            }
        }

        return result;
    }

    private static string[] ParseBlocks(string input) =>
        input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
}