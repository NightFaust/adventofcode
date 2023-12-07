using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day06;

[ProblemName("Wait For It")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var races = ParseInput(input);
        var numberOfWins = races.Select(GetRaceNumberOfWins).ToList();

        return numberOfWins.Aggregate(1, (x, y) => x * y);
    }

    public object PartTwo(string input)
    {
        var race = ParseInputPart2(input);
        return GetRaceNumberOfWins(race);
    }

    private static IEnumerable<Race> ParseInput(string input)
    {
        var lines = input.Split("\n");
        List<List<long>> values = new List<List<long>>();

        foreach (var line in lines)
        {
            values.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(long.Parse).ToList());
        }

        var times = values[0];
        var distances = values[1];

        return times.Select((t, i) => new Race
        {
            Time = t,
            PreviousDistanceRecord = distances[i]
        }).ToList();
    }

    private static Race ParseInputPart2(string input)
    {
        var lines = input.Split("\n");
        var values = new List<long>();

        foreach (var line in lines)
        {
            var data = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
            var temp = data.Aggregate("", (current, a) => current + a);

            values.Add(long.Parse(temp));
        }

        return new Race
        {
            Time = values[0],
            PreviousDistanceRecord = values[1]
        };
    }

    private static int GetRaceNumberOfWins(Race race)
    {
        var numberOfWins = 0;

        for (int i = 0; i < race.Time; i++)
        {
            var distance = i * (race.Time - i);
            if (distance > race.PreviousDistanceRecord)
            {
                numberOfWins++;
            }
        }

        return numberOfWins;
    }
}

public class Race
{
    public long PreviousDistanceRecord { get; init; }
    public long Time { get; init; }
}