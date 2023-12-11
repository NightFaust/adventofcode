using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AdventOfCode.Y2018.Day13;

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
                Console.WriteLine("We went right\n" + lines[currentPoint.Item1][currentPoint.Item2]);
                newDirection = GetNewDirection(Direction.Right, lines[currentPoint.Item1][currentPoint.Item2]);
            }
            else if (pipesMap.Last().Value == Direction.Left)
            {
                currentPoint = (pipesMap.Last().Key.Item1, pipesMap.Last().Key.Item2 - 1);
                Console.WriteLine("We went left\n" + lines[currentPoint.Item1][currentPoint.Item2]);
                newDirection = GetNewDirection(Direction.Left, lines[currentPoint.Item1][currentPoint.Item2]);
            }
            else if (pipesMap.Last().Value == Direction.Up)
            {
                currentPoint = (pipesMap.Last().Key.Item1 - 1, pipesMap.Last().Key.Item2);
                Console.WriteLine("We went up\n" + lines[currentPoint.Item1][currentPoint.Item2]);
                newDirection = GetNewDirection(Direction.Up, lines[currentPoint.Item1][currentPoint.Item2]);
            }
            else if (pipesMap.Last().Value == Direction.Down)
            {
                currentPoint = (pipesMap.Last().Key.Item1 + 1, pipesMap.Last().Key.Item2);
                Console.WriteLine("We went Down\n" + lines[currentPoint.Item1][currentPoint.Item2]);
                newDirection = GetNewDirection(Direction.Down, lines[currentPoint.Item1][currentPoint.Item2]);
            }
        
            if (newDirection == Direction.End)
            {
                // We went back to the previous point
                Console.WriteLine("We went back to the starting point");
                break;
            }
            
            // Just to be sure
            var actualChar = lines[currentPoint.Item1][currentPoint.Item2];
            Console.WriteLine("==> " + actualChar);
            switch (pipesMap.Last().Value)
            {
                case Direction.Down:
                {
                    var down = new[] { '|', 'L', 'J' };
                    if (!down.Contains(actualChar))
                        throw new InvalidOperationException("We went down but there is no correct pipe");
                    break;
                }
                case Direction.Up:
                {
                    var up = new[] { '|', '7', 'F' };
                    if (!up.Contains(actualChar))
                        throw new InvalidOperationException("We went up but there is no correct pipe");
                    break;
                }
                case Direction.Left:
                {
                    var left = new[] { '-', 'F', 'L' };
                    if (!left.Contains(actualChar))
                        throw new InvalidOperationException("We went left but there is no correct pipe");
                    break;
                }
                case Direction.Right:
                {
                    var right = new[] { '-', 'J', '7' };
                    if (!right.Contains(actualChar))
                        throw new InvalidOperationException("We went right but there is no correct pipe");
                    break;
                }
            }
            
            pipesMap.Add(currentPoint, newDirection);
        
            // Display pipesMap.Last() data
            Console.WriteLine($"Step {pipesMap.Count} Key: {string.Join(", ", pipesMap.Last().Key)}, Value: {pipesMap.Last().Value}");
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
                    Console.WriteLine($"We found the starting point ({i}, {j})");
                    // Hardcoded starting direction
                    return new KeyValuePair<(int, int), Direction>((i, j), Direction.Right);
                }
            }
        }

        return new KeyValuePair<(int, int), Direction>();
    }

    // private static Direction GetStartingDirection(IReadOnlyList<string> lines, int lineIndex, int pipeIndex)
    // {
    //     var newDirections = new List<Direction>();
    //     if (lineIndex > 0)
    //     {
    //         var newDirection = GetNewDirection(Direction.Up, lines[lineIndex - 1][pipeIndex]);
    //         if (newDirection != Direction.Down)
    //         {
    //             newDirections.Add(newDirection);
    //         }
    //     }
    //
    //     if (lineIndex < lines.Count - 1)
    //     {
    //         var newDirection = GetNewDirection(Direction.Down, lines[lineIndex + 1][pipeIndex]);
    //         if (newDirection != Direction.Up)
    //         {
    //             newDirections.Add(newDirection);
    //         }
    //     }
    //
    //     if (pipeIndex > 0)
    //     {
    //         var newDirection = GetNewDirection(Direction.Left, lines[lineIndex][pipeIndex - 1]);
    //         if (newDirection != Direction.Right)
    //         {
    //             newDirections.Add(newDirection);
    //         }
    //     }
    //
    //     if (pipeIndex < lines[lineIndex].Length - 1)
    //     {
    //         var newDirection = GetNewDirection(Direction.Right, lines[lineIndex][pipeIndex + 1]);
    //         if (newDirection != Direction.Left)
    //         {
    //             newDirections.Add(newDirection);
    //         }
    //     }
    //
    //     return newDirections.Last();
    // }

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

class Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

class Pipe
{
    public char symbol;
    public int x;
    public int y;

    public bool north;
    public bool east;
    public bool south;
    public bool west;

    public bool scheduleToRemove;

    public Pipe(char symbol, int x, int y)
    {
        this.symbol = symbol;
        this.x = x;
        this.y = y;
        SetConnections();
    }

    public void ScheduleToRemove()
    {
        scheduleToRemove = true;
    }

    public void CleanUp()
    {
        if (scheduleToRemove)
        {
            symbol = '.';
            north = false;
            east = false;
            south = false;
            west = false;
        }
    }

    private void SetConnections()
    {
        switch (symbol)
        {
            case 'S':
                north = true;
                east = true;
                south = true;
                west = true;
                break;
            case 'F':
                north = false;
                east = true;
                south = true;
                west = false;
                break;
            case '-':
                north = false;
                east = true;
                south = false;
                west = true;
                break;
            case '7':
                north = false;
                east = false;
                south = true;
                west = true;
                break;
            case '|':
                north = true;
                east = false;
                south = true;
                west = false;
                break;
            case 'J':
                north = true;
                east = false;
                south = false;
                west = true;
                break;
            case 'L':
                north = true;
                east = true;
                south = false;
                west = false;
                break;
        }
    }
}