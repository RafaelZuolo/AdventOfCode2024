
namespace AdventOfCode2024.Days;

public class Day11 : IDay
{
    public string SolvePart1(string input)
    {
        return ComputeInput(input, 25);
    }

    public string SolvePart2(string input)
    {
        return ComputeInput(input, 75);
    }

    private string ComputeInput(string input, int blinks)
    {
        var stones = input.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .ToDictionary(long.Parse, _ => (long)1);

        var nextStones = new Dictionary<long, long>();

        for (int i = 0; i < blinks; i++)
        {
            foreach (var stone in stones)
            {
                Change(stone, nextStones);
            }
            (stones, nextStones) = (nextStones, new Dictionary<long, long>());
        }

        return stones.Values.Sum().ToString();
    }

    private static void Change(KeyValuePair<long, long> stone, Dictionary<long, long> nextStones)
    {
        var value = stone.Key;
        var quantity = stone.Value;
        long nextValue;
        long? nextSplitedValue = null;

        if (value == 0)
        {
            nextValue = 1;
        }
        else if (HasEvenDigits(value))
        {
            nextValue = SplitMostSignificantDigits(value);
            nextSplitedValue = SplitLessSignificantDigits(value);
        }
        else
        {
            nextValue = value * 2024;
        }

        if (!nextStones.TryAdd(nextValue, quantity))
        {
            nextStones[nextValue] += quantity;
        }

        if (nextSplitedValue is not null)
        {
            if (!nextStones.TryAdd(nextSplitedValue.Value, quantity))
            {
                nextStones[nextSplitedValue.Value] += quantity;
            }
        }
    }

    private static long SplitLessSignificantDigits(long value)
    {
        var writtenValue = value.ToString();

        return long.Parse(writtenValue.Substring(writtenValue.Length / 2, writtenValue.Length / 2));
    }

    private static long SplitMostSignificantDigits(long value)
    {
        var writtenValue = value.ToString();

        return long.Parse(writtenValue.Substring(0, writtenValue.Length / 2));
    }

    private static bool HasEvenDigits(long value) => value.ToString().Length % 2 == 0;
}
