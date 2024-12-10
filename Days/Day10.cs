namespace AdventOfCode2024.Days;

public class Day10 : IDay
{
    public string SolvePart1(string input)
    {
        var lines = input.ParseLines();
        var topographic = lines
            .Select(l => l.Select(c => int.Parse(c.ToString())).ToList())
            .ToList();

        long sumOfScores = 0;
        for (int i = 0; i < topographic.Count; i++)
        {
            for (int j = 0; j < topographic[i].Count; j++)
            {
                sumOfScores += GetScore(i, j, topographic);
            }
        }

        return sumOfScores.ToString();
    }

    private static long GetScore(int i, int j, List<List<int>> topographic)
    {
        if (topographic[i][j] != 0) return 0;

        var isVisited = topographic.Select(l => l.Select(v => false).ToList()).ToList();
        isVisited[i][j] = true;

        return GetScore(i - 1, j, 0, topographic, isVisited)
            + GetScore(i + 1, j, 0, topographic, isVisited)
            + GetScore(i, j - 1, 0, topographic, isVisited)
            + GetScore(i, j + 1, 0, topographic, isVisited);
    }

    private static long GetScore(int i, int j, int previousHeight, List<List<int>> topographic, List<List<bool>> isVisited)
    {
        if (IsOutOfBounds(i, j, topographic) || isVisited[i][j])
            return 0;

        var currentHeight = topographic[i][j];
        if (currentHeight != previousHeight + 1)
            return 0;

        isVisited[i][j] = true;
        if (topographic[i][j] == 9) return 1;

        return GetScore(i - 1, j, currentHeight, topographic, isVisited)
            + GetScore(i + 1, j, currentHeight, topographic, isVisited)
            + GetScore(i, j - 1, currentHeight, topographic, isVisited)
            + GetScore(i, j + 1, currentHeight, topographic, isVisited);

        static bool IsOutOfBounds(int i, int j, List<List<int>> topographic)
        {
            return i < 0 || i >= topographic.Count || j < 0 || j >= topographic[i].Count;
        }
    }

    public string SolvePart2(string input)
    {
        var lines = input.ParseLines();
        var topographic = lines
            .Select(l => l.Select(c => int.Parse(c.ToString())).ToList())
            .ToList();

        long sumOfScores = 0;
        for (int i = 0; i < topographic.Count; i++)
        {
            for (int j = 0; j < topographic[i].Count; j++)
            {
                sumOfScores += GetAugmentedScore(i, j, topographic);
            }
        }

        return sumOfScores.ToString();
    }



    private static long GetAugmentedScore(int i, int j, List<List<int>> topographic)
    {
        if (topographic[i][j] != 0)
            return 0;

        return GetAugmentedScore(i - 1, j, 0, topographic)
            + GetAugmentedScore(i + 1, j, 0, topographic)
            + GetAugmentedScore(i, j - 1, 0, topographic)
            + GetAugmentedScore(i, j + 1, 0, topographic);
    }

    private static long GetAugmentedScore(int i, int j, int previousHeight, List<List<int>> topographic)
    {
        if (IsOutOfBounds(i, j, topographic))
            return 0;

        var currentHeight = topographic[i][j];
        if (currentHeight != previousHeight + 1)
            return 0;

        if (topographic[i][j] == 9)
            return 1;

        return GetAugmentedScore(i - 1, j, currentHeight, topographic)
            + GetAugmentedScore(i + 1, j, currentHeight, topographic)
            + GetAugmentedScore(i, j - 1, currentHeight, topographic)
            + GetAugmentedScore(i, j + 1, currentHeight, topographic);

        static bool IsOutOfBounds(int i, int j, List<List<int>> topographic)
        {
            return i < 0 || i >= topographic.Count || j < 0 || j >= topographic[i].Count;
        }
    }
}
