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
                if (map[i][j] is '@') robotPosition = new Position(i, j);
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
                if (map[i][j] is 'O') sum += 100 * i + j;
            }
        }

        return sum.ToString();
    }

    private void Move(List<List<char>> map, char direction, Position robotPosition)
    {
        switch (direction)
        {
            case '^':
                if (MoveUp(map, robotPosition.Y, robotPosition.X, '@'))
                {
                    robotPosition.Y -= 1;
                };
                break;
            case '>':
                if (MoveRight(map, robotPosition.Y, robotPosition.X, '@'))
                {
                    robotPosition.X += 1;
                }
                break;
            case 'v':
                if (MoveDown(map, robotPosition.Y, robotPosition.X, '@'))
                {
                    robotPosition.Y += 1;
                }
                break;
            case '<':
                if (MoveLeft(map, robotPosition.Y, robotPosition.X, '@'))
                {
                    robotPosition.X -= 1;
                }
                break;
        }
    }

    private bool MoveUp(List<List<char>> map, int i, int j, char objectToMove)
    {
        if (map[i][j] is '#') return false;
        if (map[i][j] is '.')
        {
            map[i][j] = objectToMove;
            return true;
        }
        if (map[i][j] is 'O')
        {
            if (MoveUp(map, i - 1, j, 'O'))
            {
                map[i][j] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[i][j] is '@')
        {
            if (MoveUp(map, i - 1, j, '@'))
            {
                map[i][j] = '.';
                return true;
            }
        }

        return false;
    }

    private bool MoveLeft(List<List<char>> map, int i, int j, char objectToMove)
    {
        if (map[i][j] is '#') return false;
        if (map[i][j] is '.')
        {
            map[i][j] = objectToMove;
            return true;
        }
        if (map[i][j] is 'O')
        {
            if (MoveLeft(map, i, j - 1, 'O'))
            {
                map[i][j] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[i][j] is '@')
        {
            if (MoveLeft(map, i, j - 1, '@'))
            {
                map[i][j] = '.';
                return true;
            }
        }

        return false;
    }

    private bool MoveDown(List<List<char>> map, int i, int j, char objectToMove)
    {
        if (map[i][j] is '#') return false;
        if (map[i][j] is '.')
        {
            map[i][j] = objectToMove;
            return true;
        }
        if (map[i][j] is 'O')
        {
            if (MoveDown(map, i + 1, j, 'O'))
            {
                map[i][j] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[i][j] is '@')
        {
            if (MoveDown(map, i + 1, j, '@'))
            {
                map[i][j] = '.';
                return true;
            }
        }

        return false;
    }

    private bool MoveRight(List<List<char>> map, int i, int j, char objectToMove)
    {
        if (map[i][j] is '#') return false;
        if (map[i][j] is '.')
        {
            map[i][j] = objectToMove;
            return true;
        }
        if (map[i][j] is 'O')
        {
            if (MoveRight(map, i, j + 1, 'O'))
            {
                map[i][j] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[i][j] is '@')
        {
            if (MoveRight(map, i, j + 1, '@'))
            {
                map[i][j] = '.';
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
                if (c is '@') return [c, '.'];
                if (c is 'O') return ['[', ']'];
                return new[] { c, c };
            }).ToList())
            .ToList();
        var movements = lines[1]
            .ParseLines()
            .SelectMany(l => l.ToList())
            .ToList();

        Position robotPosition = new Position(0, 0);
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] is '@') robotPosition = new Position(i, j);
            }
        }

        foreach (var movement in movements)
        {
            MovePart2(map, movement, robotPosition);
        }

        var sum = (long)0;
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] is '[') sum += 100 * i + j;
            }
        }

        return sum.ToString();
    }

    private void MovePart2(List<List<char>> map, char direction, Position robotPosition)
    {
        switch (direction)
        {
            case '^':
                if (CanMoveUpPart2(map, robotPosition.Y, robotPosition.X, '@'))
                {
                    DoMoveUpPart2(map, map.ToFalseMatrix(), robotPosition.Y, robotPosition.X, '@');
                    robotPosition.Y -= 1;
                };
                break;
            case '>':
                if (MoveRightPart2(map, robotPosition.Y, robotPosition.X, '@'))
                {
                    robotPosition.X += 1;
                }
                break;
            case 'v':
                if (CanMoveDownPart2(map, robotPosition.Y, robotPosition.X, '@'))
                {
                    DoMoveDownPart2(map, map.ToFalseMatrix(), robotPosition.Y, robotPosition.X, '@');
                    robotPosition.Y += 1;
                }
                break;
            case '<':
                if (MoveLeftPart2(map, robotPosition.Y, robotPosition.X, '@'))
                {
                    robotPosition.X -= 1;
                }
                break;
        }
    }

    private bool CanMoveUpPart2(List<List<char>> map, int i, int j, char objectToMove)
    {
        if (map[i][j] is '#') return false;
        if (map[i][j] is '.')
        {
            return true;
        }
        if (map[i][j] is '[' or ']')
        {
            if (map[i][j] is '[')
            {
                if (CanMoveUpPart2(map, i - 1, j, '[') && CanMoveUpPart2(map, i - 1, j + 1, ']'))
                {
                    return true;
                }
            }
            if (map[i][j] is ']')
            {
                if (CanMoveUpPart2(map, i - 1, j, ']') && CanMoveUpPart2(map, i - 1, j - 1, '['))
                {
                    return true;
                }
            }

            return false;
        }
        if (map[i][j] is '@')
        {
            if (CanMoveUpPart2(map, i - 1, j, '@'))
            {
                return true;
            }
        }

        return false;
    }

    private void DoMoveUpPart2(List<List<char>> map, List<List<bool>> moved, int i, int j, char objectToMove)
    {
        if (map[i][j] is '#') throw new Exception("Should not be moving");
        if (moved[i][j]) return;

        moved[i][j] = true;
        if (map[i][j] is '.')
        {
            map[i][j] = objectToMove;
            return;
        }
        if (map[i][j] is '[' or ']')
        {
            if (map[i][j] is '[')
            {
                map[i][j] = objectToMove;
                if (objectToMove is '@' or ']')
                {
                    map[i][j + 1] = '.';
                }
                DoMoveUpPart2(map, moved, i - 1, j, '[');
                DoMoveUpPart2(map, moved, i - 1, j + 1, ']');
                return;
            }
            if (map[i][j] is ']')
            {
                map[i][j] = objectToMove;
                if (objectToMove is '@' or '[')
                {
                    map[i][j - 1] = '.';
                }
                DoMoveUpPart2(map, moved, i - 1, j, ']');
                DoMoveUpPart2(map, moved, i - 1, j - 1, '[');
                return;
            }
        }
        if (map[i][j] is '@')
        {
            DoMoveUpPart2(map, moved, i - 1, j, '@');
            map[i][j] = '.';
        }
    }

    private bool MoveLeftPart2(List<List<char>> map, int i, int j, char objectToMove)
    {
        if (map[i][j] is '#') return false;
        if (map[i][j] is '.')
        {
            map[i][j] = objectToMove;
            return true;
        }
        if (map[i][j] is '[' or ']')
        {
            if (MoveLeftPart2(map, i, j - 1, map[i][j]))
            {
                map[i][j] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[i][j] is '@')
        {
            if (MoveLeftPart2(map, i, j - 1, '@'))
            {
                map[i][j] = '.';
                return true;
            }
        }

        return false;
    }

    private bool CanMoveDownPart2(List<List<char>> map, int i, int j, char _)
    {
        if (map[i][j] is '#') return false;
        if (map[i][j] is '.')
        {
            return true;
        }
        if (map[i][j] is '[' or ']')
        {
            if (map[i][j] is '[')
            {
                if (CanMoveDownPart2(map, i + 1, j, '[') && CanMoveDownPart2(map, i + 1, j + 1, ']'))
                {
                    return true;
                }
            }
            if (map[i][j] is ']')
            {
                if (CanMoveDownPart2(map, i + 1, j, ']') && CanMoveDownPart2(map, i + 1, j - 1, '['))
                {
                    return true;
                }
            }

            return false;
        }
        if (map[i][j] is '@')
        {
            if (CanMoveDownPart2(map, i + 1, j, '@'))
            {
                return true;
            }
        }

        return false;
    }

    private void DoMoveDownPart2(List<List<char>> map, List<List<bool>> moved, int i, int j, char objectToMove)
    {
        if (map[i][j] is '#') throw new Exception("Cannot invoke this if reach [#]");

        if (moved[i][j])
        {
            return;
        }

        moved[i][j] = true;
        if (map[i][j] is '.')
        {
            map[i][j] = objectToMove;
            return;
        }
        if (map[i][j] is '[' or ']')
        {
            if (map[i][j] is '[')
            {
                map[i][j] = objectToMove;
                if (objectToMove is '@' or ']')
                {
                    map[i][j + 1] = '.';
                }
                DoMoveDownPart2(map, moved, i + 1, j, '[');
                DoMoveDownPart2(map, moved, i + 1, j + 1, ']');
                return;
            }
            if (map[i][j] is ']')
            {
                map[i][j] = objectToMove;
                if (objectToMove is '@' or '[')
                {
                    map[i][j - 1] = '.';
                }
                DoMoveDownPart2(map, moved, i + 1, j, ']');
                DoMoveDownPart2(map, moved, i + 1, j - 1, '[');
                return;
            }
        }
        if (map[i][j] is '@')
        {
            map[i][j] = '.';
            DoMoveDownPart2(map, moved, i + 1, j, '@');
        }
    }

    private bool MoveRightPart2(List<List<char>> map, int i, int j, char objectToMove)
    {
        if (map[i][j] is '#') return false;
        if (map[i][j] is '.')
        {
            map[i][j] = objectToMove;
            return true;
        }
        if (map[i][j] is '[' or ']')
        {
            if (MoveRightPart2(map, i, j + 1, map[i][j]))
            {
                map[i][j] = objectToMove;
                return true;
            }

            return false;
        }
        if (map[i][j] is '@')
        {
            if (MoveRightPart2(map, i, j + 1, '@'))
            {
                map[i][j] = '.';
                return true;
            }
        }

        return false;
    }
}

internal class Position
{
    public Position(int i, int j)
    {
        Y = i;
        X = j;
    }

    public int Y { get; set; }
    public int X { get; set; }
}
