using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day12;

[ProblemName("Hot Springs")]
class Solution : Solver
{
    private readonly Dictionary<(long, long, long), long> dp = new();

    public object PartOne(string input)
    {
        long answer = 0;
        foreach (var line in ParseLines(input))
        {
            var splits = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var dots = splits[0];
            var numbers = splits[1];
            var blocks = numbers.Split(",")
                .Select(int.Parse)
                .ToList();
            
            dp.Clear();
            
            var score = Resolve(dots, blocks, 0, 0, 0);
            answer += score;
        }

        return answer;
    }

    public object PartTwo(string input)
    {
        long answer = 0;
        foreach (var line in ParseLines(input))
        {
            var splits = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var dots = string.Join("?", Enumerable.Repeat(splits[0], 5));
            var numbers = string.Join(",", Enumerable.Repeat(splits[1], 5));
            var blocks = numbers.Split(",")
                .Select(int.Parse)
                .ToList();
            
            dp.Clear();
            
            var score = Resolve(dots, blocks, 0, 0, 0);
            answer += score;
        }

        return answer;
    }

    private static string[] ParseLines(string input) =>
        input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

    private long Resolve(string dots, IReadOnlyList<int> blocks, int index, int blockIndex, long current)
    {
        var key = (index, blockIndex, current);

        if (dp.TryGetValue(key, out long value))
        {
            return value;
        }

        if (index == dots.Length)
        {
            if (blockIndex == blocks.Count && current == 0)
            {
                return 1;
            }

            if (blockIndex == blocks.Count - 1 && blocks[(int)blockIndex] == current)
            {
                return 1;
            }

            return 0;
        }

        long answer = 0;

        foreach (var c in new[] { '.', '#' })
        {
            if (dots[(int)index] == c || dots[(int)index] == '?')
            {
                if (c == '.' && current == 0)
                {
                    answer += Resolve(dots, blocks, index + 1, blockIndex, 0);
                }
                else if (c == '.' && current > 0 && blockIndex < blocks.Count() && blocks[(int)blockIndex] == current)
                {
                    answer += Resolve(dots, blocks, index + 1, blockIndex + 1, 0);
                }
                else if (c == '#')
                {
                    answer += Resolve(dots, blocks, index + 1, blockIndex, current + 1);
                }
            }
        }

        dp.Add(key, answer);
        
        return answer;
    }
}