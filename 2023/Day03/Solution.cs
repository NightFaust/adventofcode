using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day03;

[ProblemName("Gear Ratios")]
class Solution : Solver
{
    private readonly List<Number> partNumbers = new();
    private List<Gear> gears = new();

    public object PartOne(string input)
    {
        var lines = GetLines(input);
        ExtractPartNumbers(lines);
        return partNumbers.Select(n => n.Value).Sum();
    }

    public object PartTwo(string input)
    {
        var gearRatios = gears
            .Select(gear => gear.PartNumbers.Aggregate(1, (current, partNumber) => current * partNumber.Value))
            .ToList();
        return gearRatios.Sum();
    }

    private static string[] GetLines(string input)
    {
        return input.Split("\n");
    }

    private void ExtractPartNumbers(IReadOnlyList<string> input)
    {
        for (int lineIndex = 0; lineIndex < input.Count - 1; lineIndex++)
        {
            for (int slotIndex = 0; slotIndex < input[lineIndex].Length; slotIndex++)
            {
                var slot = input[lineIndex][slotIndex];
                var slotType = GetSlotType(slot);

                if (slotType == SlotType.Symbol)
                {
                    Gear gear = null;
                    if (slot == '*')
                    {
                        gear = new Gear
                        {
                            SlotIndex = slotIndex,
                            LineIndex = lineIndex,
                            PartNumbers = new List<Number>()
                        };
                    }

                    CheckPreviousLine(input, lineIndex, slotIndex, gear);
                    CheckCurrentLine(input, lineIndex, slotIndex, gear);
                    CheckNextLine(input, lineIndex, slotIndex, gear);

                    if (gear?.PartNumbers.Count >= 2)
                    {
                        gears.Add(gear);
                    }
                }
            }
        }
    }

    private void CheckNextLine(IReadOnlyList<string> input, int lineIndex, int slotIndex, Gear gear)
    {
        if (lineIndex < input.Count - 1)
        {
            var nextLine = input[lineIndex + 1];
            CheckLeft(slotIndex - 1, nextLine, lineIndex + 1, gear);
            CheckBottomOrTop(slotIndex, nextLine, lineIndex + 1, gear);
            CheckRight(slotIndex + 1, nextLine, lineIndex + 1, gear);
        }
    }

    private void CheckCurrentLine(IReadOnlyList<string> input, int lineIndex, int slotIndex, Gear gear)
    {
        var line = input[lineIndex];
        CheckLeft(slotIndex - 1, line, lineIndex, gear);
        CheckRight(slotIndex + 1, line, lineIndex, gear);
    }

    private void CheckPreviousLine(IReadOnlyList<string> input, int lineIndex, int slotIndex, Gear gear)
    {
        if (lineIndex > 0)
        {
            var previousLine = input[lineIndex - 1];
            CheckLeft(slotIndex - 1, previousLine, lineIndex - 1, gear);
            CheckBottomOrTop(slotIndex, previousLine, lineIndex - 1, gear);
            CheckRight(slotIndex + 1, previousLine, lineIndex - 1, gear);
        }
    }

    private void CheckBottomOrTop(int slotIndex, string previousLine, int lineIndex, Gear gear = null)
    {
        var topSlotType = GetSlotType(previousLine[slotIndex]);
        if (topSlotType == SlotType.Number)
        {
            GetNumber(slotIndex, lineIndex, previousLine, gear);
        }
    }

    private void CheckRight(int slotIndex, string line, int lineIndex, Gear gear = null)
    {
        if (slotIndex < line.Length - 1)
        {
            var rightSlotType = GetSlotType(line[slotIndex]);
            if (rightSlotType == SlotType.Number)
            {
                GetNumber(slotIndex, lineIndex, line, gear);
            }
        }
    }

    private void CheckLeft(int slotIndex, string line, int lineIndex, Gear gear = null)
    {
        if (slotIndex > 0)
        {
            var leftSlotType = GetSlotType(line[slotIndex]);
            if (leftSlotType == SlotType.Number)
            {
                GetNumber(slotIndex, lineIndex, line, gear);
            }
        }
    }

    private void GetNumber(int slotIndex, int lineIndex, string line, Gear gear = null)
    {
        Number number = new()
        {
            Y = lineIndex,
            Coordinates = new List<Coordinate>
            {
                new()
                {
                    X = slotIndex,
                    Value = int.Parse(line[slotIndex].ToString()),
                    Order = 0
                }
            }
        };

        IterateLeft(slotIndex, line, number);
        IterateRight(slotIndex, line, number);

        // On calcul la value final
        number.Value = ComputeValue(number);

        if (!CheckIfNumberAlreadyListed(slotIndex, lineIndex))
        {
            number.Id = Guid.NewGuid();
            // On liste le numéro
            partNumbers.Add(number);
        }
        
        if (gear is not null)
        {
            if (number.Id.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                gear.PartNumbers?.Add(number);
            }
        }
    }

    private static int ComputeValue(Number number)
    {
        var coordinatesValues = number.Coordinates.OrderBy(c => c.Order).Select(c => c.Value).ToList();
        var value = coordinatesValues.Aggregate("", (current, coordinateValue) => current + coordinateValue);
        return int.Parse(value);
    }

    private static void IterateRight(int slotIndex, string line, Number number)
    {
        var rightIndex = slotIndex;

        while (rightIndex < line.Length - 1)
        {
            var rightSlotType = GetSlotType(line[rightIndex + 1]);
            if (rightSlotType == SlotType.Number)
            {
                number.Coordinates.Add(new Coordinate
                {
                    X = rightIndex + 1,
                    Value = int.Parse(line[rightIndex + 1].ToString()),
                    Order = number.Coordinates.Min(c => c.Order) + 1
                });
                rightIndex++;
            }
            else
            {
                break;
            }
        }
    }

    private static void IterateLeft(int slotIndex, string line, Number number)
    {
        var leftIndex = slotIndex;

        while (leftIndex > 0)
        {
            var leftSlotType = GetSlotType(line[leftIndex - 1]);
            if (leftSlotType == SlotType.Number)
            {
                number.Coordinates.Add(new Coordinate
                {
                    X = leftIndex - 1,
                    Value = int.Parse(line[leftIndex - 1].ToString()),
                    Order = number.Coordinates.Min(c => c.Order) - 1
                });
                leftIndex--;
            }
            else
            {
                break;
            }
        }
    }

    private bool CheckIfNumberAlreadyListed(int slotIndex, int lineIndex)
    {
        var coordinates = partNumbers.Where(n => n.Y == lineIndex).Select(n => n.Coordinates);
        var result = coordinates.Any(coords => coords.Any(coord => coord.X == slotIndex));
        return result;
    }

    private static SlotType GetSlotType(char slot)
    {
        if (char.IsNumber(slot))
        {
            return SlotType.Number;
        }

        if (slot == '.')
        {
            return SlotType.Dot;
        }

        return SlotType.Symbol;
    }
}

public class Number
{
    public Guid Id { get; set; }
    public List<Coordinate> Coordinates { get; set; }

    // Un nombre ne peut être que sur une seule ligne
    public int Y { get; set; }

    public int Value { get; set; }
}

public class Gear
{
    public int SlotIndex { get; set; }
    public int LineIndex { get; set; }
    public List<Number> PartNumbers { get; set; }
}

public class Coordinate
{
    public int X { get; set; }

    public int Value { get; set; }

    public int Order { get; set; }
}

public enum SlotType
{
    Dot,
    Number,
    Symbol
}