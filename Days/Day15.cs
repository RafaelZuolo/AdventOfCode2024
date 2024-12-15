namespace AdventOfCode2024.Days;

public class Day15 : IDay
{
    public string SolvePart1(string input)
    {
        var lines = input
            .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();
        var map = lines[0].ParseAsMatrix(c => c);
        var movements = lines[1]
            .ParseLines()
            .SelectMany(l => l.ToList())
            .ToList();

        Position robotPosition = new Position(0, 0);
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == '@') robotPosition = new Position(i, j);
            }
        }

        foreach (var movement in movements)
        {
            Move(map, movement, robotPosition);
        }

        var sum = (long)0;
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map.Count; j++)
            {
                if (map[i][j] == 'O') sum += 100 * i + j;
            }
        }

        return sum.ToString();
    }

    private void Move(List<List<char>> map, char direction, Position robotPosition)
    {
        switch (direction)
        {
            case '^':
                if (MoveUp(map, robotPosition.X, robotPosition.Y, '@'))
                {
                    robotPosition.X -= 1;
                };
                break;
            case '>':
                if (MoveRight(map, robotPosition.X, robotPosition.Y, '@'))
                {
                    robotPosition.Y += 1;
                }
                break;
            case 'v':
                if (MoveDown(map, robotPosition.X, robotPosition.Y, '@'))
                {
                    robotPosition.X += 1;
                }
                break;
            case '<':
                if (MoveLeft(map, robotPosition.X, robotPosition.Y, '@'))
                {
                    robotPosition.Y -= 1;
                }
                break;
        }
    }

    private bool MoveUp(List<List<char>> map, int x, int y, char objectToMove)
    {
        if (map[x][y] == '#') return false;
        if (map[x][y] == '.')
        {
            map[x][y] = objectToMove;
            return true;
        }
        if (map[x][y] == 'O')
        {
            if (MoveUp(map, x - 1, y, 'O'))
            {
                map[x][y] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[x][y] == '@')
        {
            if (MoveUp(map, x - 1, y, '@'))
            {
                map[x][y] = '.';
                return true;
            }
        }

        return false;
    }

    private bool MoveLeft(List<List<char>> map, int x, int y, char objectToMove)
    {
        if (map[x][y] == '#') return false;
        if (map[x][y] == '.')
        {
            map[x][y] = objectToMove;
            return true;
        }
        if (map[x][y] == 'O')
        {
            if (MoveLeft(map, x, y - 1, 'O'))
            {
                map[x][y] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[x][y] == '@')
        {
            if (MoveLeft(map, x, y - 1, '@'))
            {
                map[x][y] = '.';
                return true;
            }
        }

        return false;
    }

    private bool MoveDown(List<List<char>> map, int x, int y, char objectToMove)
    {
        if (map[x][y] == '#') return false;
        if (map[x][y] == '.')
        {
            map[x][y] = objectToMove;
            return true;
        }
        if (map[x][y] == 'O')
        {
            if (MoveDown(map, x + 1, y, 'O'))
            {
                map[x][y] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[x][y] == '@')
        {
            if (MoveDown(map, x + 1, y, '@'))
            {
                map[x][y] = '.';
                return true;
            }
        }

        return false;
    }

    private bool MoveRight(List<List<char>> map, int x, int y, char objectToMove)
    {
        if (map[x][y] == '#') return false;
        if (map[x][y] == '.')
        {
            map[x][y] = objectToMove;
            return true;
        }
        if (map[x][y] == 'O')
        {
            if (MoveRight(map, x, y + 1, 'O'))
            {
                map[x][y] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[x][y] == '@')
        {
            if (MoveRight(map, x, y + 1, '@'))
            {
                map[x][y] = '.';
                return true;
            }
        }

        return false;
    }

    public string SolvePart2(string input)
    {
        var lines = input
            .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();
        var map = lines[0]
            .ParseAsMatrix(c => c)
            .Select(l => l.SelectMany(c =>
            {
                if (c == '@') return [c, '.'];
                if (c == 'O') return ['[', ']'];
                return new[] { c, c };
            }).ToList())
            .ToList();
        map.PrintMatrix();
        var movements = lines[1]
            .ParseLines()
            .SelectMany(l => l.ToList())
            .ToList();

        Position robotPosition = new Position(0, 0);
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map.Count; j++)
            {
                if (map[i][j] == '@') robotPosition = new Position(i, j);
            }
        }

        foreach (var movement in movements)
        {
            MovePart2(map, movement, robotPosition);
        }

        var sum = (long)0;
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map.Count; j++)
            {
                if (map[i][j] == '[') sum += 100 * i + j;
            }
        }

        return sum.ToString();
    }

    private void MovePart2(List<List<char>> map, char direction, Position robotPosition)
    {
        switch (direction)
        {
            case '^':
                if (MoveUpPart2(map, robotPosition.X, robotPosition.Y, '@'))
                {
                    robotPosition.X -= 1;
                };
                break;
            case '>':
                if (MoveRightPart2(map, robotPosition.X, robotPosition.Y, '@'))
                {
                    robotPosition.Y += 1;
                }
                break;
            case 'v':
                if (MoveDownPart2(map, robotPosition.X, robotPosition.Y, '@'))
                {
                    robotPosition.X += 1;
                }
                break;
            case '<':
                if (MoveLeftPart2(map, robotPosition.X, robotPosition.Y, '@'))
                {
                    robotPosition.Y -= 1;
                }
                break;
        }
    }

    private bool MoveUpPart2(List<List<char>> map, int x, int y, char objectToMove)
    {
        if (map[x][y] == '#') return false;
        if (map[x][y] == '.')
        {
            map[x][y] = objectToMove;
            return true;
        }
        if (map[x][y] is '[' or ']')
        {
            if (map[x][y] is '[')
            {
                if (MoveUpPart2(map, x - 1, y, '[') && MoveUpPart2(map, x - 1, y + 1, ']'))
                {
                    map[x][y] = objectToMove;
                    return true;
                }
            }
            if (map[x][y] is ']')
            {
                if (MoveUpPart2(map, x - 1, y, ']') && MoveUpPart2(map, x - 1, y - 1, '['))
                {
                    map[x][y] = objectToMove;
                    return true;
                }
            }

            return false;
        }
        if (map[x][y] == '@')
        {
            if (MoveUpPart2(map, x - 1, y, '@'))
            {
                map[x][y] = '.';
                return true;
            }
        }

        return false;
    }

    private bool MoveLeftPart2(List<List<char>> map, int x, int y, char objectToMove)
    {
        if (map[x][y] == '#') return false;
        if (map[x][y] == '.')
        {
            map[x][y] = objectToMove;
            return true;
        }
        if (map[x][y] is '[' or ']')
        {
            if (MoveLeftPart2(map, x, y - 1, map[x][y]))
            {
                map[x][y] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[x][y] == '@')
        {
            if (MoveLeftPart2(map, x, y - 1, '@'))
            {
                map[x][y] = '.';
                return true;
            }
        }

        return false;
    }

    private bool MoveDownPart2(List<List<char>> map, int x, int y, char objectToMove)
    {
        if (map[x][y] == '#') return false;
        if (map[x][y] == '.')
        {
            map[x][y] = objectToMove;
            return true;
        }
        if (map[x][y] is '[' or ']')
        {
            if (map[x][y] is '[')
            {
                if (MoveDownPart2(map, x + 1, y, '[') && MoveDownPart2(map, x + 1, y + 1, ']'))
                {
                    map[x][y] = objectToMove;
                    return true;
                }
            }
            if (map[x][y] is ']')
            {
                if (MoveDownPart2(map, x + 1, y, ']') && MoveDownPart2(map, x + 1, y - 1, '['))
                {
                    map[x][y] = objectToMove;
                    return true;
                }
            }

            return false;
        }
        if (map[x][y] == '@')
        {
            if (MoveDownPart2(map, x + 1, y, '@'))
            {
                map[x][y] = '.';
                return true;
            }
        }

        return false;
    }

    private bool MoveRightPart2(List<List<char>> map, int x, int y, char objectToMove)
    {
        if (map[x][y] is '#') return false;
        if (map[x][y] is '.')
        {
            map[x][y] = objectToMove;
            return true;
        }
        if (map[x][y] is '[' or ']')
        {
            if (MoveRightPart2(map, x, y + 1, map[x][y]))
            {
                map[x][y] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[x][y] == '@')
        {
            if (MoveRightPart2(map, x, y + 1, '@'))
            {
                map[x][y] = '.';
                return true;
            }
        }

        return false;
    }
}

internal class Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
}
