namespace _2024;

public static class Day17
{
    private static int RegisterA = 61156655;
    private static int RegisterB = 0;
    private static int RegisterC = 0;

    private static List<int> Program = [];
    private static int Pointer = 0;

    private static List<string> Output = [];

    public static string Solve()
    {
        // Only keep the problem in the input
        Program = File.ReadLines(@".\Input\Day17.txt").First().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        while (Pointer < Program.Count)
        {
            var instructionCode = Program[Pointer++];
            Instruction instruction = instructionCode switch
            {
                0 => new Adv(),
                1 => new Bxl(),
                2 => new Bst(),
                3 => new Jnz(),
                4 => new Bxc(),
                5 => new Out(),
                6 => new Bdv(),
                7 => new Cdv(),
                _ => throw new InvalidDataException("Not a valid instruction code")
            };

            var operandCode = Program[Pointer++];
            Operand operand = instructionCode switch
            {
                0 => new Combo(operandCode),
                1 => new Literal(operandCode),
                2 => new Combo(operandCode),
                3 => new Literal(operandCode),
                4 => new Literal(operandCode),
                5 => new Combo(operandCode),
                6 => new Combo(operandCode),
                7 => new Combo(operandCode),
                _ => throw new InvalidDataException("Not a valid instruction code")
            };

            instruction.Execute(operand);
        }

        return $"Part 1: {string.Join(",", Output)}";
    }

    public abstract class Instruction
    {
        public abstract void Execute(Operand operand);
    }

    public class Adv : Instruction
    {
        public override void Execute(Operand operand)
        {
            RegisterA = (int)Math.Floor(RegisterA / Math.Pow(2, operand.Value));
        }
    }

    public class Bxl : Instruction
    {
        public override void Execute(Operand operand)
        {
            RegisterB ^= operand.Value;
        }
    }

    public class Bst : Instruction
    {
        public override void Execute(Operand operand)
        {
            RegisterB = operand.Value % 8;
        }
    }

    public class Jnz : Instruction
    {
        public override void Execute(Operand operand)
        {
            if (RegisterA != 0)
            {
                Pointer = operand.Value;
            }
        }
    }

    public class Bxc : Instruction
    {
        public override void Execute(Operand operand)
        {
            RegisterB ^= RegisterC;
        }
    }

    public class Out : Instruction
    {
        public override void Execute(Operand operand)
        {
            Output.Add((operand.Value % 8).ToString());
        }
    }

    public class Bdv : Instruction
    {
        public override void Execute(Operand operand)
        {
            RegisterB = (int)Math.Floor(RegisterA / Math.Pow(2, operand.Value));
        }
    }

    public class Cdv : Instruction
    {
        public override void Execute(Operand operand)
        {
            RegisterC = (int)Math.Floor(RegisterA / Math.Pow(2, operand.Value));
        }
    }

    public abstract class Operand
    {
        public int Value { get; set; }
    }
    public class Literal : Operand
    {
        public Literal(int operandCode)
        {
            Value = operandCode;
        }
    }

    public class Combo : Operand
    {
        public Combo(int operandCode)
        {
            Value = operandCode switch
            {
                >= 0 and < 4 => operandCode,
                4 => RegisterA,
                5 => RegisterB,
                6 => RegisterC,
                _ => throw new InvalidDataException("Not a valid operand code")
            };
        }
    }
}