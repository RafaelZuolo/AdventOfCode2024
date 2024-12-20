namespace AdventOfCode2024.Days;

public class Day18 : IDay
{
    public string SolvePart1(string input)
    {
        const int sideLength = 71;
        const int corruptedLocations = 1024;

        var blocks = input.ParseLines()
            .Select(l => (int.Parse(l.Split(',')[0]), int.Parse(l.Split(',')[1])))
            .ToList();

        var maze = Enumerable.Range(0, sideLength)
            .Select(i => Enumerable.Range(0, sideLength)
                .Select(j => '.')
                .ToList())
            .ToList();

        for (var i = 0; i < corruptedLocations; i++)
        {
            var block = blocks[i];
            maze[block.Item1][block.Item2] = '#';
        }

        var cost = GenerateGraphAndGetCost(sideLength, maze);
        return $"{cost}";
    }

    private static long GenerateGraphAndGetCost(int sideLength, List<List<char>> maze)
    {
        var vertices = new HashSet<Vertex>();
        for (var i = 0; i < sideLength; i++)
        {
            for (var j = 0; j < sideLength; j++)
            {
                if (maze[i][j] is not '#')
                    vertices.Add(new Vertex(i, j));
            }
        }
        for (var i = 0; i < sideLength; i++)
        {
            for (var j = 0; j < sideLength; j++)
            {
                if (vertices.TryGetValue(new Vertex(i, j), out var v))
                {
                    if (vertices.TryGetValue(new Vertex(i + 1, j), out var adj))
                    {
                        v.Adjacency.Add(new Edge(v, adj, 1));
                    }
                    if (vertices.TryGetValue(new Vertex(i, j + 1), out adj))
                    {
                        v.Adjacency.Add(new Edge(v, adj, 1));
                    }
                    if (vertices.TryGetValue(new Vertex(i - 1, j), out adj))
                    {
                        v.Adjacency.Add(new Edge(v, adj, 1));
                    }
                    if (vertices.TryGetValue(new Vertex(i, j - 1), out adj))
                    {
                        v.Adjacency.Add(new Edge(v, adj, 1));
                    }
                }
            }
        }
        _ = vertices.TryGetValue(new Vertex(0, 0), out var start);
        _ = vertices.TryGetValue(new Vertex(sideLength - 1, sideLength - 1), out var end);
        var cost = vertices.Dijkstra(start!, end!)[end!];
        return cost;
    }

    public string SolvePart2(string input)
    {
        const int sideLength = 71;

        var blocks = input.ParseLines()
            .Select(l => (int.Parse(l.Split(',')[0]), int.Parse(l.Split(',')[1])))
            .ToList();
        var blockingBlock = BinarySearch(blocks);

        return $"{blockingBlock.Item1},{blockingBlock.Item2}";

        (int, int) BinarySearch(List<(int, int)> block)
        {
            return DoBinarySearch(0, blocks.Count - 1);


            (int, int) DoBinarySearch(int left, int right)
            {
                var midleSize = (left + right) / 2;
                var maze = Enumerable.Range(0, sideLength)
                    .Select(i => Enumerable.Range(0, sideLength)
                        .Select(j => '.')
                        .ToList())
                    .ToList();

                for (var i = 0; i < midleSize; i++)
                {
                    var block = blocks[i];
                    maze[block.Item1][block.Item2] = '#';
                }

                var cost = GenerateGraphAndGetCost(sideLength, maze);

                if (cost >= 0)
                {
                    var block = blocks[midleSize];
                    maze[block.Item1][block.Item2] = '#';
                    var nextCost = GenerateGraphAndGetCost(sideLength, maze);

                    return nextCost < 0 ? block : DoBinarySearch(midleSize + 1, right);
                }
                else
                {
                    return DoBinarySearch(left, midleSize - 1);
                }
            }
        }
    }
}
