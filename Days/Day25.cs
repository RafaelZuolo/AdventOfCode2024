namespace AdventOfCode2024.Days;

public class Day25 : IDay
{
    public string SolvePart1(string input)
    {
        var schematics = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.ParseAsMatrix(c => c))
            .Select(s => new Schematic(s));
        var keys = schematics.Where(s => s.Type is Schemma.Key).ToList();
        var locks = schematics.Where(s => s.Type is Schemma.Lock).ToList();

        var count = 0;
        foreach (var key in keys)
        {
            foreach (var @lock in locks)
            {
                count += IsOverlap(key, @lock) ? 1 : 0;
            }
        }

        return count.ToString();
    }

    private static bool IsOverlap(Schematic key, Schematic @lock)
    {
        for (var i = 0; i < key.Heights.Count; i++)
        {
            if (key.Heights[i] + @lock.Heights[i] > 5)
            {
                return false;
            }
        }

        return true;
    }

    enum Schemma { Key, Lock, }

    class Schematic(List<List<char>> schematic)
    {
        public List<int> Heights { get; } = GetColumnsHeights(schematic);
        public Schemma Type { get; } = schematic[0][0] is '.' ? Schemma.Key : Schemma.Lock;

        private static List<int> GetColumnsHeights(List<List<char>> schematic)
        {
            var counts = new int[schematic[0].Count];
            for (int i = 0; i < schematic.Count; i++)
            {
                for (int j = 0; j < schematic[i].Count; j++)
                {
                    counts[j] += schematic[i][j] is '#' ? 1 : 0;
                }
            }

            for (int j = 0; j < schematic[0].Count; j++)
            {
                counts[j]--;
            }

            return [.. counts];
        }
    }

    public string SolvePart2(string input)
    {
        return "not solved";
    }
}
