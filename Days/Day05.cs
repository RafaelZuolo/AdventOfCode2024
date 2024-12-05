namespace AdventOfCode2024.Days;

public class Day05 : IDay
{
    public string SolvePart1(string input)
    {
        var dividedInput = input
            .Split(
                Environment.NewLine + Environment.NewLine,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        var rules = dividedInput[0].Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(l =>
            {
                var values = l.Split('|');
                return new Rule(int.Parse(values[0]), int.Parse(values[1]));
            })
            .ToArray();
        var manuals = dividedInput[1].Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split(',').Select(int.Parse).ToArray())
            .ToArray();

        return manuals
            .Aggregate(
                0,
                (count, manual) => IsValid(manual, rules) ? count + GetMiddle(manual) : count)
            .ToString();
    }

    private static int GetMiddle(int[] manual)
    {
        return manual[manual.Length / 2];
    }

    private static bool IsValid(int[] manual, Rule[] rules)
    {
        foreach (var rule in rules)
        {
            if (!manual.Contains(rule.Before) || !manual.Contains(rule.After))
            {
                continue;
            }

            if (Array.IndexOf(manual, rule.Before) > Array.IndexOf(manual, rule.After))
            {
                return false;
            }
        }

        return true;
    }

    public string SolvePart2(string input)
    {
        var dividedInput = input
            .Split(
                Environment.NewLine + Environment.NewLine,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        var rules = dividedInput[0].Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(l =>
            {
                var values = l.Split('|');
                return new Rule(int.Parse(values[0]), int.Parse(values[1]));
            })
            .ToArray();
        var invalidManuals = dividedInput[1].Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split(',').Select(int.Parse).ToArray())
            .Where(m => !IsValid(m, rules))
            .ToArray();

        return invalidManuals
            .Aggregate(
                0,
                (count, manual) => count + GetMiddle(manual.InsertionSort(rules)))
            .ToString();
    }
}

internal record Rule(int Before, int After);

internal static class IntArrayExtensions
{
    public static int[] InsertionSort(this int[] manual, ICollection<Rule> rules)
    {
        for (int i = 0; i < manual.Length; i++)
        {
            SwapWithLowestValueAfterIndex(i, manual, rules);
        }

        return manual;
    }

    private static void SwapWithLowestValueAfterIndex(int index, int[] manual, ICollection<Rule> rules)
    {
        var currentLowerIdex = index;
        for (int i = index + 1; i < manual.Length; i++)
        {
            if (IsLowerByTheRules(currentLower: manual[currentLowerIdex], candidate: manual[i], rules))
            {
                currentLowerIdex = i;
            }
        }

        (manual[index], manual[currentLowerIdex]) = (manual[currentLowerIdex], manual[index]);
    }

    private static bool IsLowerByTheRules(int currentLower, int candidate, ICollection<Rule> rules)
    {
        foreach (var rule in rules)
        {
            if (currentLower == rule.After && candidate == rule.Before)
            {
                return true;
            }
        }

        return false;
    }
}
