using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day02;

[ProblemName("Cube Conundrum")]
class Solution : Solver
{
    private const string Blue = "blue";
    private const string Green = "green";
    private const string Red = "red";
    private const int BlueMax = 14;
    private const int GreenMax = 13;
    private const int RedMax = 12;

    public object PartOne(string input)
    {
        var lines = input.Split("\n");
        var games = ExtractGames(lines);

        return CheckGames(games);
    }

    public object PartTwo(string input)
    {
        var lines = input.Split("\n");
        var games = ExtractGames(lines);
        var minCubes = GetMinimumCubesSetPower(games);
        return minCubes.Sum();
    }

    private static List<Game> ExtractGames(string[] lines)
    {
        List<Game> result = new();

        foreach (var line in lines)
        {
            var parts = HandlePrefix(line, out var game);

            game.Sets = HandleSets(parts);
            result.Add(game);
        }

        return result;
    }

    private static string[] HandlePrefix(string line, out Game game)
    {
        var parts = line.Split(": ");
        var prefixPart = parts[0].Split(" ");

        game = new Game
        {
            Number = int.Parse(prefixPart[1])
        };
        return parts;
    }

    private static List<Set> HandleSets(string[] parts)
    {
        List<Set> sets = new();
        foreach (var set in parts[1].Split("; "))
        {
            var s = CreateSet();
            var cubes = set.Split(", ");
            foreach (var cube in cubes)
            {
                HandleCube(cube, s);
            }

            sets.Add(s);
        }

        return sets;
    }

    private static Set CreateSet()
    {
        Set s = new()
        {
            Blue = 0,
            Green = 0,
            Red = 0
        };
        return s;
    }

    private static void HandleCube(string cube, Set s)
    {
        var colors = cube.Split(" ");
        switch (colors[1])
        {
            case Blue:
                s.Blue += int.Parse(colors[0]);
                break;
            case Green:
                s.Green += int.Parse(colors[0]);
                break;
            case Red:
                s.Red += int.Parse(colors[0]);
                break;
            default:
                throw new InvalidOperationException("Invalid color in set");
        }
    }

    private static int CheckGames(IEnumerable<Game> games)
    {
        return games.Aggregate(0, ValidateGame);
    }

    private static int ValidateGame(int result, Game game)
    {
        var isPossible = game.Sets.All(set => set.Blue <= BlueMax && set.Green <= GreenMax && set.Red <= RedMax);

        if (isPossible)
        {
            result += game.Number;
        }

        return result;
    }

    private IEnumerable<int> GetMinimumCubesSetPower(List<Game> games)
    {
        List<int> result = new();

        foreach (var game in games)
        {
            AddSetPow(game, result);
        }

        return result;
    }

    private static void AddSetPow(Game game, ICollection<int> result)
    {
        Set min = new()
        {
            Blue = game.Sets.Max(s => s.Blue),
            Green = game.Sets.Max(s => s.Green),
            Red = game.Sets.Max(s => s.Red)
        };

        result.Add(min.Blue * min.Green * min.Red);
    }

    private class Game
    {
        public int Number { get; init; }
        public List<Set> Sets { get; set; }
    }

    private class Set
    {
        public int Blue { get; set; }
        public int Green { get; set; }
        public int Red { get; set; }
    }
}