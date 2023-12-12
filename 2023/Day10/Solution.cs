using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AdventOfCode.Y2023.Day10;

[ProblemName("Pipe Maze")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var lines = ParseLines(input);
        var pipesMap = new Dictionary<(int, int), Direction>();
        // We find where the 'S' is
        var startingPoint = FindStartingPoint(lines);
        pipesMap.Add(startingPoint.Key, startingPoint.Value);

        while (true)
        {
            var currentPoint = new ValueTuple<int, int>();
            var newDirection = Direction.Down;

            if (pipesMap.Last().Value == Direction.Right)
            {
                currentPoint = (pipesMap.Last().Key.Item1, pipesMap.Last().Key.Item2 + 1);
                newDirection = GetNewDirection(Direction.Right, lines[currentPoint.Item1][currentPoint.Item2]);
            }
            else if (pipesMap.Last().Value == Direction.Left)
            {
                currentPoint = (pipesMap.Last().Key.Item1, pipesMap.Last().Key.Item2 - 1);
                newDirection = GetNewDirection(Direction.Left, lines[currentPoint.Item1][currentPoint.Item2]);
            }
            else if (pipesMap.Last().Value == Direction.Up)
            {
                currentPoint = (pipesMap.Last().Key.Item1 - 1, pipesMap.Last().Key.Item2);
                newDirection = GetNewDirection(Direction.Up, lines[currentPoint.Item1][currentPoint.Item2]);
            }
            else if (pipesMap.Last().Value == Direction.Down)
            {
                currentPoint = (pipesMap.Last().Key.Item1 + 1, pipesMap.Last().Key.Item2);
                newDirection = GetNewDirection(Direction.Down, lines[currentPoint.Item1][currentPoint.Item2]);
            }

            if (newDirection == Direction.End)
            {
                // We went back to the previous point
                break;
            }

            pipesMap.Add(currentPoint, newDirection);
        }

        return (pipesMap.Count) / 2;
    }

    public object PartTwo(string input)
    {
        var lines = ParseLines(input);
        var pipesMap = new Dictionary<(int, int), Direction>();
        var visitedTiles = new HashSet<(int, int)>();
        // We find where the 'S' is
        var startingPoint = FindStartingPoint(lines);

        pipesMap.Add(startingPoint.Key, startingPoint.Value);
        visitedTiles.Add(startingPoint.Key);

        while (true)
        {
            var currentPoint = new ValueTuple<int, int>();
            var newDirection = Direction.Down;

            if (pipesMap.Last().Value == Direction.Right)
            {
                currentPoint = (pipesMap.Last().Key.Item1, pipesMap.Last().Key.Item2 + 1);
                newDirection = GetNewDirection(Direction.Right, lines[currentPoint.Item1][currentPoint.Item2]);
            }
            else if (pipesMap.Last().Value == Direction.Left)
            {
                currentPoint = (pipesMap.Last().Key.Item1, pipesMap.Last().Key.Item2 - 1);
                newDirection = GetNewDirection(Direction.Left, lines[currentPoint.Item1][currentPoint.Item2]);
            }
            else if (pipesMap.Last().Value == Direction.Up)
            {
                currentPoint = (pipesMap.Last().Key.Item1 - 1, pipesMap.Last().Key.Item2);
                newDirection = GetNewDirection(Direction.Up, lines[currentPoint.Item1][currentPoint.Item2]);
            }
            else if (pipesMap.Last().Value == Direction.Down)
            {
                currentPoint = (pipesMap.Last().Key.Item1 + 1, pipesMap.Last().Key.Item2);
                newDirection = GetNewDirection(Direction.Down, lines[currentPoint.Item1][currentPoint.Item2]);
            }

            if (newDirection == Direction.End)
            {
                // We went back to the previous point
                break;
            }

            pipesMap.Add(currentPoint, newDirection);
            visitedTiles.Add(currentPoint);
        }

        // We modify Starting point for algo
        var index = startingPoint.Key.Item2;
        var str = lines[startingPoint.Key.Item1];
        var newStr = str.Remove(index, 1).Insert(index, "L");
        lines[startingPoint.Key.Item1] = newStr;

        var insideCount = 0;
        for (int y = 1; y < lines.Length - 1; y++)
        {
            for (int x = 1; x < lines[0].Length - 1; x++)
            {
                if (visitedTiles.Contains((y, x)))
                {
                    continue;
                }

                var crossings = 0;

                for (int i = x + 1; i < lines[0].Length; i++)
                {
                    var tile = lines[y][i];
                    // If we find a wall
                    var symbols = new[] { '|', 'J', 'L' };

                    if (visitedTiles.Contains((y, i)) && symbols.Contains(tile))
                    {
                        crossings++;
                    }
                }

                if (crossings % 2 == 1)
                {
                    insideCount++;
                }
            }
        }

        return insideCount;
    }

    private static string[] ParseLines(string input) =>
        input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

    private static KeyValuePair<(int, int), Direction> FindStartingPoint(IReadOnlyList<string> lines)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == 'S')
                {
                    // Hardcoded starting direction
                    return new KeyValuePair<(int, int), Direction>((i, j), Direction.Right);
                }
            }
        }

        return new KeyValuePair<(int, int), Direction>();
    }

    private static Direction GetNewDirection(Direction previousDirection, char pipe)
    {
        if (pipe == 'S')
        {
            return Direction.End;
        }

        return previousDirection switch
        {
            Direction.Right => pipe switch
            {
                '-' => Direction.Right,
                '7' => Direction.Down,
                'J' => Direction.Up,
                _ => throw new InvalidEnumArgumentException()
            },
            Direction.Left => pipe switch
            {
                '-' => Direction.Left,
                'L' => Direction.Up,
                'F' => Direction.Down,
                _ => throw new InvalidEnumArgumentException()
            },
            Direction.Up => pipe switch
            {
                '|' => Direction.Up,
                '7' => Direction.Left,
                'F' => Direction.Right,
                _ => throw new InvalidEnumArgumentException()
            },
            _ => pipe switch
            {
                '|' => Direction.Down,
                'L' => Direction.Right,
                'J' => Direction.Left,
                _ => throw new InvalidEnumArgumentException()
            }
        };
    }
}

internal enum Direction
{
    Up,
    Down,
    Left,
    Right,
    End
}