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
}