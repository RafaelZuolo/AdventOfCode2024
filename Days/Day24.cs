namespace AdventOfCode2024.Days;

public class Day24 : IDay
{
    public string SolvePart1(string input)
    {
        var splittedInput = input.Split(
            Environment.NewLine + Environment.NewLine,
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var wires = new Dictionary<string, Wire>();

        var gates = new HashSet<Gate>();
        foreach (var line in splittedInput[1].ParseLines())
        {
            var firstSplit = line.Split(" -> ");
            if (!wires.TryGetValue(firstSplit[1], out var output))
            {
                output = new Wire(firstSplit[1]);
                wires.Add(output.Name, output);
            }

            var secondSplit = firstSplit[0].Split(' ');
            if (!wires.TryGetValue(secondSplit[0], out var input1))
            {
                input1 = new Wire(secondSplit[0]);
                wires.Add(input1.Name, input1);
            }
            if (!wires.TryGetValue(secondSplit[2], out var input2))
            {
                input2 = new Wire(secondSplit[2]);
                wires.Add(input2.Name, input2);
            }

            var gate = new Gate(line)
            {
                Operation = Gate.ParseOperator(secondSplit[1]),
                Next = output,
            };
            gates.Add(gate);

            input1.Next.Add(gate);
            input2.Next.Add(gate);
        }

        foreach (var l in splittedInput[0].ParseLines())
        {
            var parts = l.Split(": ");
            if (wires.TryGetValue(parts[0], out var start))
            {
                start.State = parts[1] == "1";
            }
        }

        while (wires.Values.Where(w => w.Name.StartsWith('z')).Any(w => w.State is null))
        {
            foreach (var w in wires.Values.Where(w => w.State is not null && !w.WasOperated))
            {
                w.Operate();
                w.UpdateGates(w.State);
            }
        }

        var zWires = wires.Values.Where(w => w.Name.StartsWith('z')).ToList();
        zWires.Sort((w, z) => -int.Parse(w.Name[1..]) + int.Parse(z.Name[1..]));

        return Convert.ToInt64(string.Join("", zWires.Select(z => z.State!.Value ? "1" : "0").ToArray()), 2).ToString();
    }

    public string SolvePart2(string input)
    {
        var splittedInput = input.Split(
            Environment.NewLine + Environment.NewLine,
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var wires = new Dictionary<string, Wire>();

        var gates = new HashSet<Gate>();
        foreach (var line in splittedInput[1].ParseLines())
        {
            var firstSplit = line.Split(" -> ");
            if (!wires.TryGetValue(firstSplit[1], out var output))
            {
                output = new Wire(firstSplit[1]);
                wires.Add(output.Name, output);
            }

            var secondSplit = firstSplit[0].Split(' ');
            if (!wires.TryGetValue(secondSplit[0], out var input1))
            {
                input1 = new Wire(secondSplit[0]);
                wires.Add(input1.Name, input1);
            }
            if (!wires.TryGetValue(secondSplit[2], out var input2))
            {
                input2 = new Wire(secondSplit[2]);
                wires.Add(input2.Name, input2);
            }

            var gate = new Gate(line)
            {
                Operation = Gate.ParseOperator(secondSplit[1]),
                Next = output,
            };
            gates.Add(gate);

            input1.Next.Add(gate);
            input2.Next.Add(gate);
        }

        foreach (var l in splittedInput[0].ParseLines())
        {
            var parts = l.Split(": ");
            if (wires.TryGetValue(parts[0], out var start))
            {
                start.State = parts[0].StartsWith('x');
            }
        }

        while (wires.Values.Where(w => w.Name.StartsWith('z')).Any(w => w.State is null))
        {
            foreach (var w in wires.Values.Where(w => w.State is not null && !w.WasOperated))
            {
                w.Operate();
                w.UpdateGates(w.State);
            }
        }

        var zWires = wires.Values.Where(w => w.Name.StartsWith('z')).ToList();
        zWires.Sort((w, z) => -int.Parse(w.Name[1..]) + int.Parse(z.Name[1..]));

        return string.Join("", zWires.Select(z => z.State!.Value ? "1" : "0"))
            + Environment.NewLine
            + " Use this to help with mannualy investigating the input. The correct output should be:"
            + "0111111111111111111111111111111111111111111111";
    }

    class Node(string name) : IEquatable<Node>
    {
        public string Name { get; set; } = name;

        public bool Equals(Node? other)
        {
            return other is Node node && Name == node.Name;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Node);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

    class Wire(string name) : Node(name)
    {
        private bool? state;
        public bool? State
        {
            get => state;
            set
            {
                state = value;
                UpdateGates(value);
            }
        }

        public void UpdateGates(bool? value)
        {
            foreach (var gate in Next)
            {
                gate.SetInput(value);
                gate.TryOperate();
            }
        }

        public ISet<Gate> Next { get; set; } = new HashSet<Gate>();

        public bool WasOperated { get; private set; } = false;
        public void Operate() { WasOperated = true; }
    }

    class Gate(string name) : Node(name)
    {
        public bool? Input1 { get; set; }
        public bool? Input2 { get; set; }
        public required Operator Operation { get; init; }
        public Wire? Next { get; set; }

        public void SetInput(bool? input)
        {
            if (input is null) return;
            if (Input1 is null) Input1 = input;
            else Input2 = input;
        }

        public bool TryOperate()
        {
            if (Next is null) return false;
            if (Input1 is null || Input2 is null) return false;
            switch (Operation)
            {
                case Operator.AND:
                    Next.State = Input1.Value && Input2.Value;
                    break;
                case Operator.OR:
                    Next.State = Input1.Value || Input2.Value;
                    break;
                case Operator.XOR:
                    Next.State = Input1.Value ^ Input2.Value;
                    break;
            }

            return true;
        }

        public enum Operator
        {
            AND,
            OR,
            XOR,
        }

        public static Operator ParseOperator(string op)
        {
            if (op == "AND") return Operator.AND;
            if (op == "OR") return Operator.OR;
            if (op == "XOR") return Operator.XOR;
            throw new InvalidCastException($"Invalid operator [{op}]");
        }
    }
}
