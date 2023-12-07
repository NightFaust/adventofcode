using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var almanac = input.Split("\n\n");
        var minLocations = new List<BigInteger>();
        var (seeds, maps) = GetData(almanac, true);

        foreach (var seed in seeds)
        {
            GetMinLocations(seed, maps, minLocations);
        }

        return minLocations.Min();
    }

    public object PartTwo(string input)
    {
        var almanac = input.Split("\n\n");
        var minLocations = new List<BigInteger>();
        var (seeds, maps) = GetData(almanac, false);
        
        foreach (var seed in seeds)
        {
            GetMinLocations(seed, maps, minLocations);
        }

        return minLocations.Min();
    }

    private static void GetMinLocations(BigInteger seed, List<Map> maps, List<BigInteger> minLocations)
    {
        var currentSeed = seed;
        for (var m = 0; m < maps.Count; m++)
        {
            currentSeed = GetSeedLocation(maps, m, currentSeed);
        }

        minLocations.Add(currentSeed);
    }

    private static BigInteger GetSeedLocation(List<Map> maps, int m, BigInteger currentSeed)
    {
        var map = maps[m];
        foreach (var line in map.Lines)
        {
            var destination = line.Values[0];
            var source = line.Values[1];
            var range = line.Values[2];

            if (currentSeed >= source && currentSeed <= source + range - 1)
            {
                // On met à jour la seed
                var offset = destination - source;
                currentSeed += offset;
                break;
            }
        }

        return currentSeed;
    }

    private static (List<BigInteger>, List<Map>) GetData(IReadOnlyList<string> almanac, bool isPartOne)
    {
        List<BigInteger> seeds = new();
        List<Map> maps = new();

        for (var i = 0; i < almanac.Count; i++)
        {
            var currentMap = almanac[i];

            if (i == 0)
            {
                seeds = GetSeeds(currentMap, isPartOne);
            }
            else
            {
                maps.Add(GetMaps(currentMap));
            }
        }

        return (seeds, maps);
    }

    private static Map GetMaps(string currentMap)
    {
        Map map = new();

        var test = currentMap.Split("\n").Skip(1).ToArray();

        // Display each lines of the map
        var lines = test
            .Select(line => new Line { Values = line.Split(" ").Select(BigInteger.Parse).ToList() })
            .ToList();
        map.Lines = lines;

        return map;
    }

    private static List<BigInteger> GetSeeds(string currentMap, bool isPartOne)
    {
        if (isPartOne)
        {
            return currentMap.Split(" ").Skip(1).Select(BigInteger.Parse).ToList();
        }
        Console.WriteLine("Hello");
        var seeds = new List<BigInteger>();
        var result = new List<BigInteger>();
        seeds = currentMap.Split(" ").Skip(1).Select(BigInteger.Parse).ToList();

        for (int i = 0; i < seeds.Count; i += 2)
        {
            var seed = seeds[i];
            var range = seeds[i + 1];
            Console.WriteLine($"Seed: {seed} - Range: {range}");
            for (int j = 0; j < range; j++)
            {
                result.Add(seed + j);
            }
        }
        Console.WriteLine($"Part 2: {result.Count}");
        return result;
    }
}

internal class Map
{
    public List<Line> Lines;
}

internal class Line
{
    public List<BigInteger> Values;
}