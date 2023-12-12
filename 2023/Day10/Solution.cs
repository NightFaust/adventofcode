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
        return 0;
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
