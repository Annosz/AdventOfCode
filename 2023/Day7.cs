namespace _2023;

public static class Day7
{
    private static List<Hand> Hands = new();

    private static readonly Dictionary<char, int> CardValues = new Dictionary<char, int>()
    {
        { '2', 2 },
        { '3', 3 },
        { '4', 4 },
        { '5', 5 },
        { '6', 6 },
        { '7', 7 },
        { '8', 8 },
        { '9', 9 },
        { 'T', 10 },
        { 'J', 1 }, // Part 2 value
        { 'Q', 12 },
        { 'K', 13 },
        { 'A', 14 },
    };

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day7.txt"))
        {
            var lineSplit = line.Split(' ');
            Hands.Add(new Hand(lineSplit[0], int.Parse(lineSplit[1])));
        }

        Hands = Hands.OrderBy(h => GetHandValue(h.Cards)).ToList();
        return Hands.Select((h, i) => h.Bid * (i + 1)).Sum().ToString();
    }

    private static long GetHandValue(string cards)
    {
        long cardsValue = 0;

        // Value of individual cards
        for (int i = 0; i <= 4; i++)
        {
            cardsValue += (int)Math.Pow(15, (5 - i)) * CardValues[cards[i]];
        }

        // Value of hand type
        cards = ReplaceJokers(cards); // Part 2 preprocessing
        if (cards.Distinct().Count() == 1) // Five of a kind
        {
            cardsValue += 6 * (long)Math.Pow(15, 6);
        }
        else if (cards.Distinct().Count() == 2 && cards.Any(c => cards.Count(e => e == c) == 4)) // Four of a kind
        {
            cardsValue += 5 * (long)Math.Pow(15, 6);
        }
        else if (cards.Distinct().Count() == 2) // Full house
        {
            cardsValue += 4 * (long)Math.Pow(15, 6);
        }
        else if (cards.Distinct().Count() == 3 && cards.Any(c => cards.Count(e => e == c) == 3)) // Three of a kind
        {
            cardsValue += 3 * (long)Math.Pow(15, 6);
        }
        else if (cards.Distinct().Count() == 3) // Two pair
        {
            cardsValue += 2 * (long)Math.Pow(15, 6);
        }
        else if (cards.Distinct().Count() == 4 && cards.Any(c => cards.Count(e => e == c) == 2)) // One pair
        {
            cardsValue += 1 * (long)Math.Pow(15, 6);
        }

        return cardsValue;
    }

    private static string ReplaceJokers(string cards)
    {
        var nonJokerCardsOrdered = cards.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Where(grp => grp.Key != 'J').Select(grp => grp.Key);
        char mostCommonCard = nonJokerCardsOrdered.Any() ? nonJokerCardsOrdered.First() : 'A';
        return cards.Replace('J', mostCommonCard);
    }

    public record Hand(string Cards, int Bid);
}