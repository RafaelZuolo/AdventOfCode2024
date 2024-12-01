using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days;
internal class Day01 : IDay
{
    public string SolvePart1(string input)
    {
        var parsedInput = input
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var firstIds = new List<long>();
        var secondIds = new List<long>();

        foreach (var item in parsedInput)
        {
            var values = item.Split("   ").Select(long.Parse).ToArray();
            firstIds.Add(values[0]);
            secondIds.Add(values[1]);
        }

        firstIds.Sort();
        secondIds.Sort();

        var result = (long)0;
        for (int i = 0; i < firstIds.Count; i++)
        {
            result += Math.Abs(firstIds[i] - secondIds[i]);
        }

        return result.ToString();
    }

    public string SolvePart2(string input)
    {
        var parsedInput = input
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var firstIds = new List<long>();
        var secondIds = new List<long>();

        foreach (var item in parsedInput)
        {
            var values = item.Split("   ").Select(long.Parse).ToArray();
            firstIds.Add(values[0]);
            secondIds.Add(values[1]);
        }

        var frequencyById = new Dictionary<long, long>();

        foreach (var id in secondIds)
        {
            frequencyById[id] = frequencyById.TryGetValue(id, out var value)
                ? ++value
                : 1;
        }

        return firstIds
            .Aggregate((long)0, (current, next) => current + CalculateFrequency(next, frequencyById))
            .ToString();

        static long CalculateFrequency(long id, Dictionary<long, long> frequency)
        {
            return frequency.TryGetValue(id, out var value) ? value * id : 0;
        }
    }
}
