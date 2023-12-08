using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day07;

[ProblemName("Camel Cards")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var hands = ParseInput(input);
        foreach (var hand in hands)
        {
            hand.Type = GetHandType(hand.Cards);
        }

        return ComputeFinalScore(hands);
    }

    public object PartTwo(string input)
    {
        var hands = ParseInput(input, isPart2: true);

        foreach (var hand in hands)
        {
            hand.Type = GetHandType(hand.Cards, isPart2: true);
        }

        return ComputeFinalScore(hands);
    }

    private static int ComputeFinalScore(IEnumerable<Hand> hands)
    {
        var handsOrdered = hands.GroupBy(h => h.Type)
            .OrderBy(t => (int)t.Key)
            .ToList();

        var rank = 1;
        var result = 0;
        foreach (var orderHand in handsOrdered)
        {
            if (orderHand.Count() == 1)
            {
                result += orderHand.First().Bid * rank;
                rank++;
            }
            else
            {
                // sort all hands by first card of each hand
                var sortedHands = orderHand.OrderBy(h => h.Cards.First().Strength).ToList();
                sortedHands.Sort(new HandComparer());

                // Display sorted hands
                foreach (var hand in sortedHands)
                {
                    result += hand.Bid * rank;
                    rank++;
                }
            }
        }

        return result;
    }

    private static List<Hand> ParseInput(string input, bool isPart2 = false)
    {
        var hands = new List<Hand>();
        var lines = input.Split("\n");

        foreach (var line in lines)
        {
            var values = line.Split(" ");

            var cards = new List<Card>();
            for (var i = 0; i < values[0].Length; i++)
            {
                var card = values[0][i];
                cards.Add(new Card
                {
                    Name = card.ToString(),
                    Strength = isPart2 ? GetCardStrengthPart2(card) : GetCardStrength(card)
                });
            }

            hands.Add(new Hand
            {
                Cards = cards,
                Bid = int.Parse(values[1])
            });
        }

        return hands;
    }

    private static int GetCardStrength(char card)
    {
        return card switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 11,
            'T' => 10,
            _ => int.Parse(card.ToString())
        };
    }

    private static int GetCardStrengthPart2(char card)
    {
        return card switch
        {
            'A' => 13,
            'K' => 12,
            'Q' => 11,
            'T' => 10,
            'J' => 1,
            _ => int.Parse(card.ToString())
        };
    }

    private static Type GetHandType(IReadOnlyCollection<Card> cards, bool isPart2 = false)
    {
        var cardGroups = cards.GroupBy(card => card.Name).ToList();
        var cardGroupCount = cardGroups.Count;

        if (isPart2 && cards.Select(c => c.Name).Contains("J"))
        {
            switch (cardGroupCount)
            {
                case 1:
                case 2:
                    return Type.FiveOfAKind;
                case 3:
                    return cardGroups.Any(group => group.Count() == 3 || group.Count(g => g.Name == "J") == 2)
                        ? Type.FourOfAKind
                        : Type.FullHouse;
                case 4:
                    return Type.ThreeOfAKind;
                case 5:
                    return Type.OnePair;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected card group count {cardGroupCount}");
            }
        }

        return cardGroupCount switch
        {
            1 => Type.FiveOfAKind,
            2 when cardGroups.Any(group => group.Count() == 4) => Type.FourOfAKind,
            2 => Type.FullHouse,
            3 when cardGroups.Any(group => group.Count() == 3) => Type.ThreeOfAKind,
            3 => Type.TwoPair,
            4 => Type.OnePair,
            _ => Type.HighCard
        };
    }
}

public class Hand
{
    public List<Card> Cards { get; init; }
    public int Bid { get; init; }
    public Type Type { get; set; }
}

public enum Type
{
    FiveOfAKind = 7,
    FourOfAKind = 6,
    FullHouse = 5,
    ThreeOfAKind = 4,
    TwoPair = 3,
    OnePair = 2,
    HighCard = 1
}

public class Card
{
    public string Name { get; init; }
    public int Strength { get; init; }
}

public class HandComparer : IComparer<Hand>
{
    public int Compare(Hand x, Hand y)
    {
        for (var i = 0; i < 5; i++)
        {
            if (x!.Cards[i].Strength > y!.Cards[i].Strength)
            {
                return 1;
            }

            if (x.Cards[i].Strength < y.Cards[i].Strength)
            {
                return -1;
            }
        }

        return 0;
    }
}