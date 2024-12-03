using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day03 : IDay
{
    public string SolvePart1(string input)
    {
        var numberPattern = "[0-9]{1,3}";
        var pattern = $"mul\\({numberPattern},{numberPattern}\\)";

        var result = (long)0;
        foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
        {
            var values = Regex.Matches(match.Value, numberPattern);
            var firstMatch = long.Parse(values[0].Value);
            var secondMatch = long.Parse(values[1].Value);

            result += firstMatch * secondMatch;
        }

        return result.ToString();
    }

    public string SolvePart2(string input)
    {
        var numberPattern = "[0-9]{1,3}";
        var doPattern = "do\\(\\)";
        var dontPattern = "don't\\(\\)";
        var pattern = $"mul\\({numberPattern},{numberPattern}\\)|{doPattern}|{dontPattern}";

        var result = (long)0;
        var isEnabled = true;
        foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
        {
            var value = match.Value;
            if (Regex.IsMatch(value, doPattern))
            {
                isEnabled = true;
                continue;
            }
            if (Regex.IsMatch(value, dontPattern))
            {
                isEnabled = false;
                continue;
            }
            if (!isEnabled)
            {
                continue;
            }

            var numbers = Regex.Matches(value, numberPattern);
            var firstMatch = long.Parse(numbers[0].Value);
            var secondMatch = long.Parse(numbers[1].Value);

            result += firstMatch * secondMatch;
        }

        return result.ToString();
    }
}
