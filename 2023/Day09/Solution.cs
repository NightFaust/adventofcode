using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day09;

[ProblemName("Mirage Maintenance")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var lines = input.Split("\n");
        var result = 0;

        foreach (var line in lines)
        {
            var histories = GenerateFullHistory(line);

            histories.Reverse();

            PredictNextSteps(histories);

            result += histories[^1][^1];
        }

        return result;
    }

    public object PartTwo(string input)
    {
        var lines = input.Split("\n");
        var result = 0;

        foreach (var line in lines)
        {
            var histories = GenerateFullHistory(line, true);

            histories.Reverse();

            PredictNextSteps(histories);

            result += histories[^1][^1];
        }

        return result;
    }

    private static void PredictNextSteps(List<List<int>> histories)
    {
        for (int i = 0; i < histories.Count; i++)
        {
            if (i == 0)
            {
                if (histories[i].Count > 0)
                {
                    histories[i].Add(histories[i][^1]);
                }

                continue;
            }

            if (i == 1)
            {
                if (histories[i].Count > 0)
                {
                    histories[i].Add(histories[i][^1]);
                }
            }

            if (histories[i].Count > 0 && histories[i - 1].Count > 0)
            {
                histories[i].Add(histories[i - 1][^1] + histories[i][^1]);
            }
        }
    }

    private static List<List<int>> GenerateFullHistory(string line, bool isPart2 = false)
    {
        var histories = new List<List<int>>();
        var numbers = line.Split(" ").Select(int.Parse).ToList();
        
        if (isPart2)
        {
            numbers.Reverse();
        }
        
        histories.Add(numbers);
        
        while (histories[^1].Any(h => h != 0))
        {
            var newHistory = new List<int>();
            for (int i = 0; i < histories[^1].Count - 1; i++)
            {
                var number1 = histories[^1][i];
                var number2 = histories[^1][i + 1];

                newHistory.Add(number2 - number1);
            }

            histories.Add(newHistory);
        }

        return histories;
    }
}