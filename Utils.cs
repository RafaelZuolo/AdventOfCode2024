namespace AdventOfCode2024;

public static class Utils
{
    public static IList<string> ParseLines(this string input)
    {
        return input
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }

    public static List<List<T>> ParseAsMatrix<T>(this string input, Func<char, T> selector)
    {
        return input.ParseLines()
            .Select(l => l.Select(selector).ToList())
            .ToList();
    }

    public static void PrintMatrix<T>(this List<List<T>> matrix)
    {
        Console.WriteLine("------------------------------");
        for (int i = 0; i < matrix.Count; i++)
        {
            Console.WriteLine();
            for (int j = 0; j < matrix[i].Count; j++)
            {
                Console.Write(matrix[i][j]);
            }
        }
    }

    public static bool IsOutOfBounds<T>(this List<List<T>> matrix, int i, int j)
    {
        return i < 0 || i >= matrix.Count || j < 0 || j >= matrix[i].Count;
    }

    public static List<List<bool>> ToFalseMatrix<T>(this List<List<T>> matrix)
    {
        return matrix.Select(l => l.Select(_ => false).ToList()).ToList();
    }
}