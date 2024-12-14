namespace AdventOfCode2024.Days;

public class Day14 : IDay
{
    const int wide = 101;
    const int tall = 103;
    const int part1Time = 100;
    const int totalTime = 100000;

    public string SolvePart1(string input)
    {
        var lines = input.ParseLines();
        var robots = lines.Select(ParseRobot).ToList();

        for (int i = 0; i < part1Time; i++)
        {
            foreach (var robot in robots)
            {
                robot.Step();
            }
        }

        var safety = GetSafety(robots);

        return $"{safety}";
    }

    public string SolvePart2(string input)
    {
        var path = Path.Join(Directory.GetCurrentDirectory(), "dump.txt");
        var lines = input.ParseLines();
        var robots = lines.Select(ParseRobot).ToList();

        using StreamWriter outputFile = new StreamWriter(path);
        var count = 0;
        var minSafety = long.MaxValue;
        var minSafetyCount = 0;
        for (int i = 0; i < totalTime; i++)
        {
            count++;
            foreach (var robot in robots)
            {
                robot.Step();
            }
            var safety = GetSafety(robots);
            if (safety < minSafety)
            {
                minSafety = safety;
                minSafetyCount = count;
                PrintBots(robots, outputFile, count);
            }
        }

        return $"{minSafetyCount}";
    }

    private static long GetSafety(List<Robot> robots)
    {
        var q1 = (long)0;
        var q2 = (long)0;
        var q3 = (long)0;
        var q4 = (long)0;

        foreach (var robot in robots)
        {
            switch (robot.GetQuadrant())
            {
                case Quadrant.Q1:
                    q1++;
                    break;
                case Quadrant.Q2:
                    q2++;
                    break;
                case Quadrant.Q3:
                    q3++;
                    break;
                case Quadrant.Q4:
                    q4++;
                    break;
            }
        }

        var safety = q1 * q2 * q3 * q4;
        return safety;
    }

    private Robot ParseRobot(string line)
    {
        var split = line.Split(' ');
        var position = split[0][2..].Split(',').Select(int.Parse).ToArray();
        var velocity = split[1][2..].Split(',').Select(int.Parse).ToArray();

        return new Robot(position, velocity, wide, tall);
    }

    private static void PrintBots(List<Robot> robots, StreamWriter output, int count)
    {
        output.WriteLine("---------------------------------------------------------------------------------");
        output.WriteLine(count);
        output.WriteLine("---------------------------------------------------------------------------------");
        for (int i = 0; i < tall; i++)
        {
            output.WriteLine();
            for (int j = 0; j < wide; j++)
            {
                if (robots.Any(r => r.IsAt(j, i)))
                {
                    output.Write('#');
                }
                else
                {
                    output.Write(' ');
                }
            }
        }
        output.WriteLine();
        output.WriteLine("---------------------------------------------------------------------------------");
    }
}

internal class Robot(int[] position, int[] velocity, int wide, int tall)
{
    public int[] Position { get; } = position;
    public int[] Velocity { get; } = velocity;
    public int Wide { get; } = wide;
    public int Tall { get; } = tall;

    public bool IsAt(int x, int y)
    {
        return Position[0] == x && Position[1] == y;
    }

    public void Step()
    {
        Position[0] += Velocity[0];
        Position[1] += Velocity[1];

        Position[0] = Position[0] >= 0 ? Position[0] % Wide : Wide + (Position[0] % Wide);
        Position[1] = Position[1] >= 0 ? Position[1] % Tall : Tall + (Position[1] % Tall);
    }

    public Quadrant GetQuadrant()
    {
        if (Position[0] < Wide / 2)
        {
            if (Position[1] < Tall / 2)
                return Quadrant.Q1;
            if (Position[1] > Tall / 2)
                return Quadrant.Q2;
        }

        if (Position[0] > Wide / 2)
        {
            if (Position[1] < Tall / 2)
                return Quadrant.Q3;
            if (Position[1] > Tall / 2)
                return Quadrant.Q4;
        }

        return Quadrant.Middle;
    }
}

internal enum Quadrant { Q1, Q2, Q3, Q4, Middle }