namespace AdventOfCode2024.Days;

public class Day02 : IDay
{
    public string SolvePart1(string input)
    {
        var parsedInput = input
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var reports = parsedInput
            .Select(l => l.Split(" ").Select(long.Parse).ToArray())
            .ToArray();

        var safeReports = 0;

        foreach (var report in reports)
        {
            if (IsReportSafe(report))
            {
                safeReports++;
            }
        }

        return safeReports.ToString();
    }

    private static bool IsReportSafe(long[] report)
    {
        var isIncreasing = true;
        var isDecreasing = true;
        var isSafeStep = true;

        for (var i = 0; i < report.Length - 1; i++)
        {
            var val = report[i] - report[i + 1];
            if (val < 0) isIncreasing = false;
            if (val > 0) isDecreasing = false;
            if (Math.Abs(val) < 1 || Math.Abs(val) > 3) isSafeStep = false;
        }

        return isSafeStep && (isIncreasing || isDecreasing);
    }

    public string SolvePart2(string input)
    {
        var parsedInput = input
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var reports = parsedInput
            .Select(l => l.Split(" ").Select(long.Parse).ToArray())
            .ToArray();

        var safeReports = 0;

        foreach (var report in reports)
        {
            if (IsReportDampenerSafe(report))
            {
                safeReports++;
            }
        }

        return safeReports.ToString();
    }

    private static bool IsReportDampenerSafe(long[] report)
    {
        if (IsReportSafe(report)) return true;

        for (var i = 0; i < report.Length; i++)
        {
            var dampenedReport = report[..i].Concat(report[(i + 1)..]).ToArray();

            if (IsReportSafe(dampenedReport)) return true;
        }

        return false;
    }
}
