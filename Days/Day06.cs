namespace AdventOfCode2024.Days;

public class Day06 : IDay
{
    public string SolvePart1(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToArray();
        var lab = new char[lines.Length, lines[0].Length];
        int vertical = -1;
        int horizontal = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains('^'))
            {
                vertical = i;
                horizontal = lines[i].IndexOf('^');
            }
            for (int j = 0; j < lines[i].Length; j++)
            {
                lab[i, j] = lines[i][j];
            }
        }
        var guard = new Guard(vertical, horizontal, lab);

        while (!guard.IsInBorder())
        {
            guard.Step();
        }

        var visited = 0;
        for (int i = 0; i < lab.GetLength(0); i++)
        {
            for (int j = 0; j < lab.GetLength(1); j++)
            {
                if (guard.Lab[i, j] == 'X') visited++;
            }
        }

        return visited.ToString();
    }

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToArray();
        var originalLab = new char[lines.Length, lines[0].Length];
        int verticalStartIndex = -1;
        int horizontalStartIndex = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains('^'))
            {
                verticalStartIndex = i;
                horizontalStartIndex = lines[i].IndexOf('^');
            }
            for (int j = 0; j < lines[i].Length; j++)
            {
                originalLab[i, j] = lines[i][j];
            }
        }
        var originalGuard = new Guard(verticalStartIndex, horizontalStartIndex, originalLab);

        while (!originalGuard.IsInBorder())
        {
            originalGuard.Step();
        }

        var visitedLab = new char[lines.Length, lines[0].Length];
        var visited = 0;
        for (int i = 0; i < originalLab.GetLength(0); i++)
        {
            for (int j = 0; j < originalLab.GetLength(1); j++)
            {
                if (originalLab[i, j] == 'X' && !(i == verticalStartIndex && j == horizontalStartIndex))
                {
                    visited += CheckForCycleWith(i, j);
                }
            }
        }

        return visited.ToString();

        int CheckForCycleWith(int verticalBlockIndex, int horizontalBlockIndex)
        {
            var augmentedLab = new HashSet<char>[lines.Length, lines[0].Length];
            for (int i = 0; i < originalLab.GetLength(0); i++)
            {
                for (int j = 0; j < originalLab.GetLength(1); j++)
                {
                    augmentedLab[i, j] = i == verticalBlockIndex && j == horizontalBlockIndex
                        ? ['#']
                        : [originalLab[i, j]];
                }
            }

            var guard = new GuardWithCycleDetection(verticalStartIndex, horizontalStartIndex, augmentedLab);
            while (!guard.IsInBorder() && !guard.HasCycle)
            {
                guard.Step();
            }

            return guard.HasCycle ? 1 : 0;
        }
    }
}
class GuardWithCycleDetection(int verticalPosition, int horizontalPosition, ISet<char>[,] lab)
{
    private Direction direction = Direction.Up;

    public int VerticalPosition { get; private set; } = verticalPosition;
    public int HorizontalPosition { get; private set; } = horizontalPosition;
    public ISet<char>[,] Lab { get; } = lab;
    public bool HasCycle { get; private set; } = false;

    public bool IsInBorder()
    {
        return VerticalPosition == 0
            || HorizontalPosition == 0
            || VerticalPosition == Lab.GetLength(0) - 1
            || HorizontalPosition == Lab.GetLength(1) - 1;
    }

    internal void Step()
    {
        switch (direction)
        {
            case Direction.Up:
            {
                if (!Lab[VerticalPosition - 1, HorizontalPosition].Contains('#'))
                {
                    if (Lab[VerticalPosition - 1, HorizontalPosition].Contains('^'))
                    {
                        HasCycle = true;
                    }
                    VerticalPosition--;
                    Lab[VerticalPosition, HorizontalPosition].Add('^');
                }
                else
                {
                    direction = Direction.Right;
                }
                break;
            }
            case Direction.Right:
            {
                if (!Lab[VerticalPosition, HorizontalPosition + 1].Contains('#'))
                {
                    if (Lab[VerticalPosition, HorizontalPosition + 1].Contains('>'))
                    {
                        HasCycle = true;
                    }
                    HorizontalPosition++;
                    Lab[VerticalPosition, HorizontalPosition].Add('>');
                }
                else
                {
                    direction = Direction.Down;
                }
                break;
            }
            case Direction.Down:
            {
                if (!Lab[VerticalPosition + 1, HorizontalPosition].Contains('#'))
                {
                    if (Lab[VerticalPosition + 1, HorizontalPosition].Contains('v'))
                    {
                        HasCycle = true;
                    }
                    VerticalPosition++;
                }
                else
                {
                    direction = Direction.Left;
                }
                break;
            }
            case Direction.Left:
            {
                if (!Lab[VerticalPosition, HorizontalPosition - 1].Contains('#'))
                {
                    if (Lab[VerticalPosition, HorizontalPosition - 1].Contains('<'))
                    {
                        HasCycle = true;
                    }
                    HorizontalPosition--;
                }
                else
                {
                    direction = Direction.Up;
                }
                break;
            }
        }
    }
    enum Direction
    {
        Up, Down, Left, Right
    }
}
internal class Guard(int verticalPosition, int horizontalPosition, char[,] lab)
{
    private Direction direction = Direction.Up;

    public int VerticalPosition { get; private set; } = verticalPosition;
    public int HorizontalPosition { get; private set; } = horizontalPosition;
    public char[,] Lab { get; } = lab;

    public bool IsInBorder()
    {
        return VerticalPosition == 0
            || HorizontalPosition == 0
            || VerticalPosition == Lab.GetLength(0) - 1
            || HorizontalPosition == Lab.GetLength(1) - 1;
    }

    internal void Step()
    {
        Lab[VerticalPosition, HorizontalPosition] = 'X';
        switch (direction)
        {
            case Direction.Up:
            {
                if (Lab[VerticalPosition - 1, HorizontalPosition] != '#')
                {
                    VerticalPosition--;
                }
                else
                {
                    direction = Direction.Right;
                }
                break;
            }
            case Direction.Right:
            {
                if (Lab[VerticalPosition, HorizontalPosition + 1] != '#')
                {
                    HorizontalPosition++;
                }
                else
                {
                    direction = Direction.Down;
                }
                break;
            }
            case Direction.Down:
            {
                if (Lab[VerticalPosition + 1, HorizontalPosition] != '#')
                {
                    VerticalPosition++;
                }
                else
                {
                    direction = Direction.Left;
                }
                break;
            }
            case Direction.Left:
            {
                if (Lab[VerticalPosition, HorizontalPosition - 1] != '#')
                {
                    HorizontalPosition--;
                }
                else
                {
                    direction = Direction.Up;
                }
                break;
            }
        }
        Lab[VerticalPosition, HorizontalPosition] = 'X';
    }

    enum Direction
    {
        Up, Down, Left, Right
    }

}
