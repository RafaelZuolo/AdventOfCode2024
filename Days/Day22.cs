namespace AdventOfCode2024.Days;

public class Day22 : IDay
{
    public string SolvePart1(string input)
    {
        const int numberOfGenerations = 2000;
        var seeds = input.ParseLines().Select(long.Parse);
        var transformed = seeds.Select(seed =>
        {
            for (int i = 0; i < numberOfGenerations; i++)
            {
                seed = Evolve(seed);
            }

            return seed;
        });

        return transformed.Sum().ToString();
    }

    public string SolvePart2(string input)
    {
        const int numberOfGenerations = 2000;
        var seeds = input.ParseLines().Select(long.Parse);
        var vendorsPrices = seeds
            .Select(seed =>
            {
                var prices = new List<long>();
                for (int i = 0; i < numberOfGenerations; i++)
                {
                    seed = Evolve(seed);
                    prices.Add(seed % 10);
                }

                return prices;
            })
            .ToList();
        var priceChanges = seeds
            .Select(seed =>
            {
                var changes = new List<long>();
                var previousSeed = seed;
                for (int i = 0; i < numberOfGenerations; i++)
                {
                    seed = Evolve(seed);
                    changes.Add(seed % 10 - previousSeed % 10);
                    previousSeed = seed;
                }

                return changes;
            })
            .ToList();

        var finalBuyValueByChange = GetTotalBananasByInputSequence(vendorsPrices, priceChanges);
        var maxBananas = finalBuyValueByChange.MaxBy(entry => entry.Value);

        return maxBananas.Value.ToString();
    }

    private static Dictionary<(long, long, long, long), long> GetTotalBananasByInputSequence(
        List<List<long>> vendorsPrices,
        List<List<long>> priceChanges)
    {
        var finalBuyValueByChange = new Dictionary<(long, long, long, long), long>();
        for (int i = 0; i < priceChanges.Count; i++)
        {
            var priceChange = priceChanges[i];
            var vendorPrices = vendorsPrices[i];

            var buyValueByChange = new Dictionary<(long, long, long, long), long>();
            for (int j = 3; j < priceChange.Count; j++)
            {
                var changeSequence = (priceChange[j - 3], priceChange[j - 2], priceChange[j - 1], priceChange[j]);
                _ = buyValueByChange.TryAdd(changeSequence, vendorPrices[j]);
            }

            foreach (var entry in buyValueByChange)
            {
                if (finalBuyValueByChange.ContainsKey(entry.Key))
                {
                    finalBuyValueByChange[entry.Key] += entry.Value;
                }
                else
                {
                    finalBuyValueByChange.Add(entry.Key, entry.Value);
                }
            }
        }

        return finalBuyValueByChange;
    }

    private static long Evolve(long seed)
    {
        var step1 = Prune(Mix(seed << 6, seed));
        var step2 = Prune(Mix(step1 >> 5, step1));
        var step3 = Prune(Mix(step2 << 11, step2));

        return step3;
    }

    private static long Prune(long seed) => seed & 0b111111111111111111111111;
    private static long Mix(long a, long b) => a ^ b;
}
