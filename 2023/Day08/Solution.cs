using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day08;

[ProblemName("Haunted Wasteland")]
class Solution : Solver
{
    private static readonly string[] Separator = { " = " };

    public object PartOne(string input)
    {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var directions = lines[0].ToArray();
        var map = ParseMap(lines.Skip(1).ToList());

        return CompleteCamelTravel(directions, map);
    }

    public object PartTwo(string input)
    {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var directions = lines[0].ToArray();
        var map = ParseMap(lines.Skip(1).ToList());

        return CompleteGhostsTravel(directions, map);
    }

    private static Dictionary<string, (string, string)> ParseMap(List<string> lines)
    {
        var map = new Dictionary<string, (string, string)>();
        foreach (var line in lines)
        {
            var parts = line.Split(Separator, StringSplitOptions.None);
            var key = parts[0];
            var values = parts[1].Trim('(', ')').Split(',');

            map[key] = (values[0].Trim(), values[1].Trim());
        }

        return map;
    }

    private static int CompleteCamelTravel(char[] directions, Dictionary<string, (string, string)> map)
    {
        var currentLocation = "AAA";
        int result = 0;
        bool shouldContinue = true;

        while (shouldContinue && currentLocation != "ZZZ")
        {
            foreach (var direction in directions)
            {
                var nextLocation = direction == 'L'
                    ? map[currentLocation].Item1
                    : map[currentLocation].Item2;
                currentLocation = nextLocation;
                result++;

                if (currentLocation == "ZZZ")
                {
                    shouldContinue = false;
                    break;
                }
            }
        }

        return result;
    }

    private static long CompleteGhostTravel(char[] directions, Dictionary<string, (string, string)> map,
        string currentLocation)
    {
        long result = 0;
        bool shouldContinue = true;

        while (shouldContinue && !currentLocation.EndsWith('Z'))
        {
            foreach (var direction in directions)
            {
                var nextLocation = direction == 'L'
                    ? map[currentLocation].Item1
                    : map[currentLocation].Item2;
                currentLocation = nextLocation;
                result++;

                if (currentLocation.EndsWith('Z'))
                {
                    shouldContinue = false;
                    break;
                }
            }
        }

        return result;
    }

    private static long CompleteGhostsTravel(char[] directions, Dictionary<string, (string, string)> map)
    {
        var startingPoints = map.Where(m => m.Key.EndsWith('A')).Select(kv => kv.Key);
        var results = new List<long>();

        foreach (var startingPoint in startingPoints)
        {
            results.Add(CompleteGhostTravel(directions, map, startingPoint));
        }

        return LCM(results.ToArray());
    }

    static long LCM(long[] numbers)
    {
        return numbers.Aggregate(lcm);
    }

    static long lcm(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }

    static long GCD(long a, long b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
}