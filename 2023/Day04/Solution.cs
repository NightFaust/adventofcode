using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day04;

[ProblemName("Scratchcards")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        return GetLines(input)
            .Select(GetWinningDictionary)
            .Select(ComputeCardScore)
            .Sum();
    }

    public object PartTwo(string input)
    {
        var lines = GetLines(input);
        var cardInstance = new Dictionary<int, int>();

        // On prefill le dictionnaire avec pour chaque ligne la valeur 1
        for (int i = 0; i < lines.Length; i++)
        {
            cardInstance.Add(i, 1);
        }

        for (int i = 0; i < lines.Length - 1; i++)
        {
            var winningDictionary = GetWinningDictionary(lines[i]);

            var totalMatches = winningDictionary.Count(pair => pair.Value > 0);
            for (int j = 0; j < cardInstance[i]; j++)
            {
                for (int k = 0; k < totalMatches; k++)
                {
                    cardInstance[i + k + 1] += 1;
                }
            }
        }

        return cardInstance.Values.Sum();
    }

    private static Dictionary<int, int> GetWinningDictionary(string line)
    {
        var numberParts = line.Split('|');
        var winningNumbers = GetWinningNumbers(numberParts);
        var numbers = GetNumbers(numberParts);
        var winningDictionary = FillWinningDictionary(winningNumbers, numbers);
        return winningDictionary;
    }

    private static Dictionary<int, int> FillWinningDictionary(IEnumerable<int> winningNumbers, IEnumerable<int> numbers)
    {
        var winningDictionary = winningNumbers.ToDictionary(winningNumber => winningNumber, _ => 0);
        foreach (var number in numbers)
        {
            if (winningDictionary.ContainsKey(number))
            {
                winningDictionary[number] += 1;
            }
        }

        return winningDictionary;
    }

    private static IEnumerable<int> GetNumbers(string[] numberParts)
    {
        return numberParts[1]
            .Replace("  ", " ")
            .Trim()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse);
    }

    private static IEnumerable<int> GetWinningNumbers(string[] numberParts)
    {
        return numberParts[0]
            .Replace("  ", " ")
            .Trim()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Skip(2)
            .Select(int.Parse);
    }

    private static int ComputeCardScore(Dictionary<int, int> winningDictionary)
    {
        var totalMatch = winningDictionary.Sum(pair => pair.Value);
        var cardTotal = 0;

        if (totalMatch > 0)
        {
            cardTotal = 1;
            for (var i = 0; i < totalMatch - 1; i++)
            {
                cardTotal *= 2;
            }
        }

        return cardTotal;
    }

    private static string[] GetLines(string input)
    {
        return input.Split("\n");
    }
}