using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var data = input.Split("\n");
        List<int> results = new();

        foreach (var d in data)
        {
            var numbers = string.Concat(d.Where(char.IsNumber));
            if (numbers.Length == 1)
            {
                results.Add(int.Parse($"{numbers}{numbers}"));
                continue;
            }

            results.Add(int.Parse($"{numbers[0]}{numbers[^1]}"));
        }

        return results.Sum();
    }

    public object PartTwo(string input)
    {
        var result = 0;
        var data = input.Split("\n");

        foreach (var d in data)
        {
            List<int> numbers = new();

            for (int i = 0; i < d.Length; i++)
            {
                if (char.IsNumber(d[i]))
                {
                    numbers.Add(int.Parse(d[i].ToString()));
                    continue;
                }

                foreach (var (text, number) in textNumbers)
                {
                    // Check if the text is in the string (and not out of bounds)
                    if (i + text.Length - 1 < d.Length && d[i..(i + text.Length)] == text)
                    {
                        numbers.Add(number);
                        break;
                    }
                }
            }

            result += int.Parse($"{numbers[0]}{numbers[^1]}");
        }

        return result;
    }

    private readonly Dictionary<string, int> textNumbers = new()
    {
        {"one",   1},
        {"two",   2},
        {"three", 3},
        {"four",  4},
        {"five",  5},
        {"six",   6},
        {"seven", 7},
        {"eight", 8},
        {"nine",  9}
    };
}
