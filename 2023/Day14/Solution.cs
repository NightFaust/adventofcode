using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using adventofcode.Utils;
using AngleSharp.Common;
using AngleSharp.Html.Dom;

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
        return 0;
    }
}
