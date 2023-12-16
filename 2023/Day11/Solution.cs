using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day11;

[ProblemName("Cosmic Expansion")]
class Solution : Solver
{
    private const char GalaxySymbol = '#';

    public object PartOne(string input)
    {
        var lines = ParseLines(input);
        var emptyLineIndexes = GetEmptyLineIndexes(lines);
        var emptyColumnIndexes = GetEmptyColumnIndexes(lines);

        var galaxies = GetGalaxies(lines);
        var pairs = GetPairs(galaxies, emptyColumnIndexes, emptyLineIndexes);

        return pairs.Sum(pair => pair.Distance + pair.NumberOfEmptySpacesToBeExpandedCrossed);
    }

    public object PartTwo(string input)
    {
        var lines = ParseLines(input);
        var emptyLineIndexes = GetEmptyLineIndexes(lines);
        var emptyColumnIndexes = GetEmptyColumnIndexes(lines);

        var galaxies = GetGalaxies(lines);
        var pairs = GetPairs(galaxies, emptyColumnIndexes, emptyLineIndexes);

        return pairs.Sum(pair => pair.Distance + (999_999 * pair.NumberOfEmptySpacesToBeExpandedCrossed));
    }

    private static IEnumerable<Pair> GetPairs(
        IReadOnlyList<Position> galaxies,
        IReadOnlyList<int> emptyColumnIndexes,
        IReadOnlyList<int> emptyLineIndexes)
    {
        List<Pair> pairs = [];
        for (var index = 0; index < galaxies.Count; index++)
        {
            var galaxy = galaxies[index];
            for (var j = index + 1; j < galaxies.Count; j++)
            {
                var otherGalaxy = galaxies[j];
                var distance = ComputeDistance(galaxy, otherGalaxy);
                var numberOfEmptySpacesToBeExpandedCrossed =
                    emptyColumnIndexes.Count(emptyColumnIndex => IsBetween(emptyColumnIndex, galaxy.X, otherGalaxy.X)) +
                    emptyLineIndexes.Count(emptyLineIndex => IsBetween(emptyLineIndex, galaxy.Y, otherGalaxy.Y));

                pairs.Add(new Pair(distance, numberOfEmptySpacesToBeExpandedCrossed));
            }
        }

        return pairs;
    }

    private static bool IsBetween(int value, int end1, int end2) =>
        value > Math.Min(end1, end2) && value < Math.Max(end1, end2);

    private static List<Position> GetGalaxies(IReadOnlyList<string> linesList)
    {
        List<Position> galaxies = [];
        for (var i = 0; i < linesList.Count; i++)
        {
            var line = linesList[i];
            for (var j = 0; j < line.Length; j++)
            {
                if (line[j] == GalaxySymbol)
                {
                    galaxies.Add(new Position(j, i));
                }
            }
        }

        return galaxies;
    }

    private static List<int> GetEmptyColumnIndexes(IReadOnlyList<string> lines)
    {
        List<int> emptyColumnIndexes = [];
        for (var j = 0; j < lines[0].Length; j++)
        {
            var emptyColumn = lines.All(line => line[j] != GalaxySymbol);

            if (emptyColumn)
            {
                emptyColumnIndexes.Add(j);
            }
        }

        return emptyColumnIndexes;
    }

    private static List<int> GetEmptyLineIndexes(IReadOnlyList<string> lines)
    {
        List<int> emptyLineIndexes = [];

        for (var i = 0; i < lines.Count; i++)
        {
            if (!lines[i].Contains(GalaxySymbol))
            {
                emptyLineIndexes.Add(i);
            }
        }

        return emptyLineIndexes;
    }

    private static double ComputeDistance(Position galaxy, Position otherGalaxy)
    {
        return Math.Abs(galaxy.X - otherGalaxy.X) + Math.Abs(galaxy.Y - otherGalaxy.Y);
    }


    private static List<string> ParseLines(string input)
    {
        return input.Split("\n").ToList();
    }
}

internal readonly struct Position(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
}

internal readonly struct Pair(
    double distance,
    double numberOfEmptySpacesToBeExpandedCrossed)
{
    public double Distance { get; } = distance;
    public double NumberOfEmptySpacesToBeExpandedCrossed { get; } = numberOfEmptySpacesToBeExpandedCrossed;
}