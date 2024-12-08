namespace AdventOfCode2024.Days;

public class Day08 : IDay
{
    public string SolvePart1(string input)
    {
        var lines = input.ParseLines();
        var antennas = Enumerable.Range(0, lines.Count)
            .SelectMany(i => Enumerable.Range(0, lines[i].Length)
                .Select(j => new Antenna(i, j, lines[i][j])))
            .Where(a => a.Frequency != '.')
            .ToList();

        var locations = new HashSet<Location>();
        foreach (var antenna in antennas)
        {
            foreach (var otherAntenna in antennas.Where(a => a.Frequency == antenna.Frequency && a != antenna))
            {
                var antennaAntiNode = antenna.Location.Subtract(otherAntenna.Location);
                var otherAntennaAntiNode = otherAntenna.Location.Subtract(antenna.Location);
                if (antennaAntiNode.IsInBounds(0, lines.Count - 1))
                {
                    locations.Add(antennaAntiNode);
                }

                if (otherAntennaAntiNode.IsInBounds(0, lines.Count - 1))
                {
                    locations.Add(otherAntennaAntiNode);
                }
            }
        }

        var set = locations.ToHashSet();
        var count = set.Count;

        return count.ToString();
    }

    public string SolvePart2(string input)
    {
        var lines = input.ParseLines();
        var antennas = Enumerable.Range(0, lines.Count)
            .SelectMany(i => Enumerable.Range(0, lines[i].Length)
                .Select(j => new Antenna(i, j, lines[i][j])))
            .Where(a => a.Frequency != '.')
            .ToList();

        var locations = new HashSet<Location>();
        foreach (var antenna in antennas)
        {
            foreach (var otherAntenna in antennas.Where(a => a.Frequency == antenna.Frequency && a != antenna))
            {
                locations.Add(antenna.Location);
                locations.Add(otherAntenna.Location);

                var antennaAntiNode = antenna.Location.Subtract(otherAntenna.Location);
                var oldAntennaAntiNode = antenna.Location;
                while (antennaAntiNode.IsInBounds(0, lines.Count - 1))
                {
                    locations.Add(antennaAntiNode);
                    (antennaAntiNode, oldAntennaAntiNode)
                        = (antennaAntiNode.Subtract(oldAntennaAntiNode), antennaAntiNode);
                }

                var otherAntennaAntiNode = otherAntenna.Location.Subtract(antenna.Location);
                oldAntennaAntiNode = otherAntennaAntiNode;
                while (antennaAntiNode.IsInBounds(0, lines.Count - 1))
                {
                    locations.Add(otherAntennaAntiNode);
                    (otherAntennaAntiNode, oldAntennaAntiNode)
                        = (otherAntennaAntiNode.Subtract(oldAntennaAntiNode), otherAntennaAntiNode);
                }
            }
        }

        var set = locations.ToHashSet();
        var count = set.Count;

        return count.ToString();
    }
}

record Location(int Vertical, int Horizontal)
{
    public bool IsInBounds(int min, int max)
    {
        return min <= Vertical && Vertical <= max
            && min <= Horizontal && Horizontal <= max;
    }

    public Location Subtract(Location other)
    {
        var verticalDistance = other.Vertical - Vertical;
        var horizontalDistance = other.Horizontal - Horizontal;

        return new Location(Vertical - verticalDistance, Horizontal - horizontalDistance);
    }
}

record Antenna(int Vertical, int Horizontal, char Frequency)
{
    public Location Location { get; } = new Location(Vertical, Horizontal);
}
