using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day04 : IDay
{
    public string SolvePart1(string input)
    {
        const string pattern = "XMAS";
        const string inverterPattern = "SAMX";
        var lines = input
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        var columns = new string[lines.Length];
        var upperDiagonals = new string[lines[0].Length];
        var lowerDiagonals = new string[lines.Length - 1];
        var upperInvertedDiagonals = new string[lines[0].Length];
        var lowerInvertedDiagonals = new string[lines.Length - 1];

        InitializeSearchSpace(
            lines,
            columns,
            upperDiagonals,
            lowerDiagonals,
            upperInvertedDiagonals,
            lowerInvertedDiagonals);

        return MatchPatterns(
            pattern,
            inverterPattern,
            lines,
            columns,
            upperDiagonals,
            lowerDiagonals,
            upperInvertedDiagonals,
            lowerInvertedDiagonals);

        static string MatchPatterns(
            string pattern,
            string inverterPattern,
            string[] lines,
            string[] columns,
            string[] upperDiagonals,
            string[] lowerDiagonals,
            string[] upperInvertedDiagonals,
            string[] lowerInvertedDiagonals)
        {
            var occurrences = 0;
            foreach (var column in columns)
            {
                occurrences += Regex.Count(column, pattern);
                occurrences += Regex.Count(column, inverterPattern);
            }
            foreach (var line in lines)
            {
                occurrences += Regex.Count(line, pattern);
                occurrences += Regex.Count(line, inverterPattern);
            }
            foreach (var diagonal in upperDiagonals)
            {
                occurrences += Regex.Count(diagonal, pattern);
                occurrences += Regex.Count(diagonal, inverterPattern);
            }
            foreach (var diagonal in lowerDiagonals)
            {
                occurrences += Regex.Count(diagonal, pattern);
                occurrences += Regex.Count(diagonal, inverterPattern);
            }
            foreach (var diagonal in upperInvertedDiagonals)
            {
                occurrences += Regex.Count(diagonal, pattern);
                occurrences += Regex.Count(diagonal, inverterPattern);
            }
            foreach (var diagonal in lowerInvertedDiagonals)
            {
                occurrences += Regex.Count(diagonal, pattern);
                occurrences += Regex.Count(diagonal, inverterPattern);
            }

            return occurrences.ToString();
        }

        static void InitializeSearchSpace(
            string[] lines,
            string[] columns,
            string[] upperDiagonals,
            string[] lowerDiagonals,
            string[] upperInvertedDiagonals,
            string[] lowerInvertedDiagonals)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                var columnBuilder = new StringBuilder();
                for (int j = 0; j < lines.Length; j++)
                {
                    columnBuilder.Append(lines[j][i]);
                }
                columns[i] = columnBuilder.ToString();
            }

            for (int i = 0; i < upperDiagonals.Length; i++)
            {
                var diagonalBuilder = new StringBuilder();
                var invertedDiagonalBuilder = new StringBuilder();
                for (int j = 0; j < lines[0].Length && i + j < lines.Length; j++)
                {
                    diagonalBuilder.Append(lines[j][i + j]);
                    invertedDiagonalBuilder.Append(lines[j][lines.Length - 1 - (i + j)]);
                }
                upperDiagonals[i] = diagonalBuilder.ToString();
                upperInvertedDiagonals[i] = invertedDiagonalBuilder.ToString();
            }

            for (int i = 0; i < lowerDiagonals.Length; i++)
            {
                var diagonalBuilder = new StringBuilder();
                var invertedDiagonalBuilder = new StringBuilder();
                for (int j = 0; j < lines.Length && i + j + 1 < lines.Length; j++)
                {
                    diagonalBuilder.Append(lines[i + j + 1][j]);
                    invertedDiagonalBuilder.Append(lines[i + j + 1][lines.Length - 1 - j]);
                }
                lowerDiagonals[i] = diagonalBuilder.ToString();
                lowerInvertedDiagonals[i] = invertedDiagonalBuilder.ToString();
            }
        }
    }

    public string SolvePart2(string input)
    {
        var lines = input
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        var occurrences = 0;
        for (int i = 1; i < lines.Length - 1; i++)
        {
            for (int j = 1; j < lines[0].Length - 1; j++)
            {
                occurrences += CheckOccurrences(i, j);
            }
        }

        return occurrences.ToString();

        int CheckOccurrences(int i, int j)
        {
            if (lines[i][j] != 'A')
            {
                return 0;
            }

            var isFirstDiagonal = false;
            if ((lines[i - 1][j - 1] == 'M' && lines[i + 1][j + 1] == 'S')
                || (lines[i - 1][j - 1] == 'S' && lines[i + 1][j + 1] == 'M'))
            {
                isFirstDiagonal = true;
            }

            var isSecondDiagonal = false;
            if ((lines[i - 1][j + 1] == 'M' && lines[i + 1][j - 1] == 'S')
                || (lines[i - 1][j + 1] == 'S' && lines[i + 1][j - 1] == 'M'))
            {
                isSecondDiagonal = true;
            }

            return isFirstDiagonal && isSecondDiagonal ? 1 : 0;
        }
    }
}
