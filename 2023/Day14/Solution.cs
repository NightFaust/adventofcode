using System;
using System.Collections.Generic;
using adventofcode.Utils;

namespace AdventOfCode.Y2023.Day14;

[ProblemName("Parabolic Reflector Dish")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var lines = input.SplitIntoColumns();
        var length = lines[0].Length;
        var result = 0;

        foreach (var line in lines)
        {
            Console.WriteLine("=============");
            var index = 0;
            var poundIndexes = new List<int>();
            while ((index = line.IndexOf('#', index)) != -1)
            {
                poundIndexes.Add(index + 1);
                index++;
            }

            Console.WriteLine(string.Join(", ", poundIndexes));
            var sum = 0;
            var poundPart = line.TrimEnd('#');
            Console.WriteLine($"poundPart: {poundPart}");

            if (poundIndexes.Count == 0 || !poundPart.Contains('#'))
            {
                var counter = 0;
                for (var i = 0; i < poundPart.Length; i++)
                {
                    if (line[i] == 'O')
                    {
                        sum += length - counter;
                        counter++;
                    }
                }
            }
            else if (!poundPart.Contains('O'))
            {
                Console.WriteLine("No O");
                continue;
            }
            else
            {
                var tempLength = length;
                for (var i = 0; i < poundPart.Length; i++)
                {
                    switch (poundPart[i])
                    {
                        case 'O':
                            Console.WriteLine("NEW " + tempLength);
                            sum += tempLength;
                            tempLength--;
                            break;
                        case '#':
                            tempLength = length - i - 1;
                            break;
                    }
                }
            }

            Console.WriteLine(line + " " + sum);
            result += sum;
        }

        return result;
    }

    public object PartTwo(string input)
    {
        // For part 2, I used Go/Golang, check out the go.txt file
        var grid = new Grid(input, true);
        long result = 0;

        for (var i = 0; i < 200; i++)
        {
            while (RollRocks(grid)) ;
            grid.Rotate(Orientation.East);
            while (RollRocks(grid)) ;
            grid.Rotate(Orientation.East);
            while (RollRocks(grid)) ;
            grid.Rotate(Orientation.East);
            while (RollRocks(grid)) ;
            grid.Rotate(Orientation.East);
        }
        var cycleVals = new List<long>();
        for (var i = 0; i < 200; i++)
        {
            while (RollRocks(grid)) ;
            grid.Rotate(Orientation.East);
            while (RollRocks(grid)) ;
            grid.Rotate(Orientation.East);
            while (RollRocks(grid)) ;
            grid.Rotate(Orientation.East);
            while (RollRocks(grid)) ;
            grid.Rotate(Orientation.East);

            var v = CalcLoad(grid);

            if (cycleVals.Contains(v))
            {
                var j = cycleVals.LastIndexOf(v);
                var cycleLen = (i - j);

                var q = (1000000000 - 200 - i -1) % cycleLen;

                var n = (q + j) % cycleLen;
                result = cycleVals[n];

                break;
            }

            cycleVals.Add(v);
        }

        return result;
    }

    private static bool RollRocks(Grid grid)
    {
        var didShift = false;
        for (var y = grid.MinY; y + 1 < grid.MaxY; y++)
        {
            for (var x = grid.MinX; x < grid.MaxX; x++)
            {
                if (grid[x, y] == '.' && grid[x, y + 1] == 'O')
                {
                    grid[x, y + 1] = '.';
                    grid[x, y] = 'O';
                    didShift = true;
                }
            }
        }

        return didShift;
    }

    private static long CalcLoad(Grid grid)
    {
        var result = 0L;
        grid.Rotate(Orientation.South);

        for (var y = grid.MinY; y < grid.MaxY; y++)
        {
            for (var x = grid.MinX; x < grid.MaxX; x++)
            {
                if (grid[x, y] == 'O')
                {
                    result += y + 1;
                }
            }
        }

        grid.Rotate(Orientation.South);
        return result;
    }
}