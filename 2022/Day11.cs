namespace _2022;

public static class Day11
{
    private const int MaxDivider = 25;

    public static string Solve()
    {
        List<Monkey> monkeys = new List<Monkey>();

        var lines = File.ReadLines(@".\Input\Day11.txt").ToArray();
        for (int i = 0; i < lines.Count(); i++)
        {
            var monkey = new Monkey();

            // Items line
            i++;
            var itemsString = lines[i].Remove(0, "Starting items: ".Length);
            monkey.ItemRemainders = itemsString.Split(',').Select(s => s.TrimStart()).Select(decimal.Parse).Select(i => Enumerable.Repeat(i, MaxDivider + 1).ToArray()).ToList();

            // Operation line
            i++;
            var operationString = lines[i].Remove(0, "Operation: new = old ".Length);
            monkey.Operation = operationString[0];
            if (operationString.Contains("old"))
            {
                monkey.IsOperationWithOldValue = true;
            }
            else
            {
                monkey.Operand = int.Parse(operationString[2..]);
            }

            // Test line
            i++;
            monkey.TestNumber = int.Parse(lines[i].Remove(0, "Test: divisible by ".Length));

            // True line
            i++;
            monkey.TrueMonkey = int.Parse(lines[i].Remove(0, "If true: throw to monkey ".Length));

            // False line
            i++;
            monkey.FalseMonkey = int.Parse(lines[i].Remove(0, "If false: throw to monkey ".Length));

            i++;

            monkeys.Add(monkey);
        }

        for (int i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.Inspect();
                monkey.Throw(monkeys);
            }
        }

        monkeys = monkeys.OrderByDescending(m => m.MonkeyBusiness).ToList();
        return (monkeys[0].MonkeyBusiness * monkeys[1].MonkeyBusiness).ToString();
    }

    public class Monkey
    {
        public List<decimal[]> ItemRemainders = new();
        public char Operation;
        public bool IsOperationWithOldValue;
        public int Operand;
        public int TestNumber;
        public int TrueMonkey;
        public int FalseMonkey;

        public decimal MonkeyBusiness;

        public void Inspect(bool isPartOne = false)
        {
            foreach (var itemRemainder in ItemRemainders)
            {
                if (Operation == '+')
                {
                    if (IsOperationWithOldValue)
                    {
                        for (int i = 1; i <= MaxDivider; i++)
                            itemRemainder[i] += itemRemainder[i];
                    }
                    else
                    {
                        for (int i = 1; i <= MaxDivider; i++)
                            itemRemainder[i] += Operand;
                    }
                }

                if (Operation == '*')
                {
                    if (IsOperationWithOldValue)
                    {
                        for (int i = 1; i <= MaxDivider; i++)
                            itemRemainder[i] *= itemRemainder[i];
                    }
                    else
                    {
                        for (int i = 1; i <= MaxDivider; i++)
                            itemRemainder[i] *= Operand;
                    }
                }

                if (isPartOne)
                {
                    for (int i = 1; i <= MaxDivider; i++)
                        itemRemainder[i] = Math.Floor(itemRemainder[i] / 3);
                }

                for (int i = 1; i <= MaxDivider; i++)
                    itemRemainder[i] %= i;
            }

            MonkeyBusiness += ItemRemainders.Count;
        }

        public void Throw(List<Monkey> monkeys)
        {
            foreach (var itemRemainder in ItemRemainders)
            {
                if (itemRemainder[TestNumber] % TestNumber == 0)
                {
                    monkeys[TrueMonkey].ItemRemainders.Add(itemRemainder);
                }
                else
                {
                    monkeys[FalseMonkey].ItemRemainders.Add(itemRemainder);
                }
            }

            ItemRemainders = new();
        }
    }
}