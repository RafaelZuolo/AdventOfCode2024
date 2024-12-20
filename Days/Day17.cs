namespace AdventOfCode2024.Days;

public class Day17 : IDay
{
    public string SolvePart1(string input)
    {
        var computer = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var registers = computer[0].Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(l => long.Parse(l.Split(": ")[1]))
            .ToArray();

        var regA = registers[0];
        var regB = registers[1];
        var regC = registers[2];

        var program = computer[1]
            .Split(": ")[1]
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
        var comp = new Computer(program, regA, regB, regC)!;
        while (comp.CanCompute)
        {
            comp.Compute();
        }

        return comp.ToString();
    }

    class Computer(int[] program, long regA, long regB, long regC)
    {
        public long RegA { get; private set; } = regA;
        public long RegB { get; private set; } = regB;
        public long RegC { get; private set; } = regC;
        public int InstructionPointer { get; private set; } = 0;
        public int[] Program { get; } = program;
        public List<int> Output { get; } = [];

        public bool CanCompute => InstructionPointer < Program.Length;
        public override string ToString() => string.Join(',', Output);

        public void Compute()
        {
            switch (Program[InstructionPointer])
            {
                case 0:
                    Adv();
                    break;
                case 1:
                    Bxl();
                    break;
                case 2:
                    Bst();
                    break;
                case 3:
                    Jnz();
                    InstructionPointer -= 2;
                    break;
                case 4:
                    Bxc();
                    break;
                case 5:
                    Out();
                    break;
                case 6:
                    Bdv();
                    break;
                case 7:
                    Cdv();
                    break;
            }

            InstructionPointer += 2;
        }

        private void Adv() => RegA = RegA >> (int)Combo;
        private void Bdv() => RegB = RegA >> (int)Combo;
        private void Cdv() => RegC = RegA >> (int)Combo;
        private void Out() => Output.Add((int)(Combo % 8));
        private void Bxc() => RegB = RegB ^ RegC;
        private void Bst() => RegB = Combo % 8;
        private void Bxl() => RegB = RegB ^ Literal;
        private void Jnz()
        {
            if (RegA == 0)
            {
                InstructionPointer += 2;
                return;
            };

            InstructionPointer = (int)Literal;
        }

        private long Literal => Program[InstructionPointer + 1];
        private long Combo => Program[InstructionPointer + 1] switch
        {
            < 4 => Literal,
            4 => RegA,
            5 => RegB,
            6 => RegC,
            _ => throw new NotImplementedException(),
        };
    }

    public string SolvePart2(string input)
    {
        var computer = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var registers = computer[0].Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(l => long.Parse(l.Split(": ")[1]))
            .ToArray();

        var regA = (long)0;
        var regB = registers[1];
        var regC = registers[2];
        var currentMatch = 0;

        var program = computer[1]
            .Split(": ")[1]
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        do
        {
            var comp = new Computer(program, regA, regB, regC)!;
            while (comp.CanCompute)
            {
                comp.Compute();
            }
            if (computer[1].Split(": ")[1].EndsWith(comp.ToString()))
            {
                currentMatch++;
                regA *= 8;
            }
            else
            {
                regA++;
            }
        } while (currentMatch < program.Length);

        return (regA / 8).ToString();
    }
}
