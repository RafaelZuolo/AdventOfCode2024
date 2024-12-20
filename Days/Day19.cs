using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day19 : IDay
{
    public string SolvePart1(string input)
    {
        var towels = input.Split(Environment.NewLine + Environment.NewLine)[0]
            .Split(", ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var patterns = input.Split(Environment.NewLine + Environment.NewLine)[1].ParseLines();
        var regex = new Regex("^(" + string.Join("|", towels) + ")*$", RegexOptions.Compiled);

        var count = (long)0;

        foreach (var pattern in patterns)
        {
            if (regex.IsMatch(pattern)) count++;
            Console.WriteLine(pattern);
        }

        return count.ToString();
    }

    public string SolvePart2(string input)
    {
        return "not solved";
    }
}
