using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Media.Dom;

namespace AdventOfCode.Y2023.Day12;

[ProblemName("Hot Springs")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var lines = ParseLines(input);
        var answer = 0;

        foreach (var line in lines)
        {
            var splits = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var dots = splits[0];
            var numbers = splits[1];

            var blocks = numbers.Split(",")
                .Select(int.Parse);

            var score = Resolve(dots, blocks, 0);
            answer += score;
        }
        
        return answer;
    }

    public object PartTwo(string input)
    {
        var lines = ParseLines(input);
        return 0;
    }

    private static string[] ParseLines(string input) =>
        input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
    
    private static int Resolve(string dots, IEnumerable<int> blocks, int index)
    {
        if (index == dots.Length)
        {
            return IsValid(dots, blocks) ? 1 : 0;
        }

        if (dots[index] == '?')
        {
            return Resolve($"{dots[..index]}#{dots[(index+1)..]}", blocks, index+1) +
                   Resolve($"{dots[..index]}.{dots[(index+1)..]}", blocks, index+1);
        }

        return Resolve(dots, blocks, index+1);
    }

    private static bool IsValid(string dots, IEnumerable<int> blocks)
    {
        var current = 0;
        List<int> seen = [];

        foreach (var dot in dots)
        {
            if (dot == '.')
            {
                if (current > 0)
                {
                    seen.Add(current);
                }

                current = 0;
            }
            else if (dot == '#')
            {
                current++;
            }
            else
            {
                throw new Exception("Invalid dot");
            }
        }

        if (current > 0)
        {
            seen.Add(current);
        }

        return seen.SequenceEqual(blocks);
    }
}