namespace AdventOfCode2024.Days;

public class Day13 : IDay
{
    public string SolvePart1(string input)
    {
        var machines = input
            .Split(Environment.NewLine + Environment.NewLine,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(r =>
            {
                var machineLines = r.ParseLines();
                var buttonA = new Button(
                    XPositionParser(machineLines[0]),
                    YPositionParser(machineLines[0]));
                var buttonB = new Button(
                    XPositionParser(machineLines[1]),
                    YPositionParser(machineLines[1]));
                var buttonPrize = new Button(
                    XPositionParser(machineLines[2]),
                    YPositionParser(machineLines[2]));

                return new Machine(buttonA, buttonB, buttonPrize);
            })
            .ToList();

        return machines.Sum(GetMinimumCost).ToString();
    }

    public string SolvePart2(string input)
    {
        var machines = input
            .Split(Environment.NewLine + Environment.NewLine,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(r =>
            {
                var machineLines = r.ParseLines();
                var buttonA = new Button(
                    XPositionParser(machineLines[0]),
                    YPositionParser(machineLines[0]));
                var buttonB = new Button(
                    XPositionParser(machineLines[1]),
                    YPositionParser(machineLines[1]));
                var buttonPrize = new Button(
                    XPositionParser(machineLines[2]) + 10000000000000,
                    YPositionParser(machineLines[2]) + 10000000000000);

                return new Machine(buttonA, buttonB, buttonPrize);
            })
            .ToList();

        return machines.Sum(GetMinimumCost).ToString();
    }

    private long GetMinimumCost(Machine machine)
    {
        var det = ((long)machine.A.X * machine.B.Y) - ((long)machine.B.X * machine.A.Y);

        if (det == 0)
        {
            return 0;
        }

        var solveA = (((long)machine.Prize.X * machine.B.Y) - ((long)machine.Prize.Y * machine.B.X)) / (decimal)det;
        var solveB = (((long)machine.A.X * machine.Prize.Y) - ((long)machine.A.Y * machine.Prize.X)) / (decimal)det;

        if (solveA != (long)solveA || solveB != (long)solveB)
        {
            return 0;
        }

        return (3 * (long)solveA) + (long)solveB;
    }

    private static long XPositionParser(string buttonInfo)
    {
        var number = buttonInfo.Substring(
            buttonInfo.IndexOf('X') + 2,
            buttonInfo.IndexOf(',') - buttonInfo.IndexOf('X') - 2);
        return long.Parse(number);
    }

    private static long YPositionParser(string buttonInfo)
    {
        var number = buttonInfo.Substring(
            buttonInfo.IndexOf('Y') + 2,
            buttonInfo.Length - buttonInfo.IndexOf('Y') - 2);
        return long.Parse(number);
    }
}

record Button(long X, long Y);
record Machine(Button A, Button B, Button Prize);