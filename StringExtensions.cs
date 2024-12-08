namespace AdventOfCode2024;

public static class StringExtensions
{
    public static IList<string> ParseLines(this string input)
    {
        return input
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }
}