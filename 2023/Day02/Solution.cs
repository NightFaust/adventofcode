using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

    private List<Game> ExtractGames(string[] lines)
    {
        List<Game> result = new();

        foreach (var line in lines)
        {
            // On retire le prefix
            var parts = line.Split(": ");
            var prefixPart = parts[0].Split(" ");

            Game game = new()
            {
                Number = int.Parse(prefixPart[1])
            };

            // On gère les sets
            List<Set> sets = new();
            foreach (var set in parts[1].Split("; "))
            {
                Set s = new()
                {
                    Blue = 0,
                    Green = 0,
                    Red = 0
                };
                var cubes = set.Split(", ");
                foreach (var cube in cubes)
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

                sets.Add(s);
            }

            game.Sets = sets;
            result.Add(game);
        }

        return result;
    }

    private int CheckGames(List<Game> games)
    {
        var result = 0;
        foreach (var game in games)
        {
            var isPossible = true;
            foreach (var set in game.Sets)
            {
                if (set.Blue > BlueMax || set.Green > GreenMax || set.Red > RedMax)
                {
                    isPossible = false;
                    break;
                }
            }

            if (isPossible)
            {
                result += game.Number;
            }
        }

        return result;
    }

    private List<int> GetMinimumCubesSetPower(List<Game> games)
    {
        List<int> result = new();

        foreach (var game in games)
        {
            Set min = new()
            {
                Blue = game.Sets.Max(s => s.Blue),
                Green = game.Sets.Max(s => s.Green),
                Red = game.Sets.Max(s => s.Red)
            };

            result.Add(min.Blue * min.Green * min.Red);
        }

        return result;
    }

    private class Game
    {
        public int Number { get; set; }
        public List<Set> Sets { get; set; }
    }

    private class Set
    {
        public int Blue { get; set; }
        public int Green { get; set; }
        public int Red { get; set; }
    }
}