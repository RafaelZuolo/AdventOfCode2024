namespace AdventOfCode2024.Days;

public class Day20 : IDay
{
    public string SolvePart1(string input)
    {
        const long minShortcutCost = 100;

        var maze = input.ParseAsMatrix(c => c);
        var vertices = ExtractGraph(maze);
        Vertex start = new Vertex(-1, -1);
        Vertex end = new Vertex(-1, -1);
        for (var i = 0; i < maze.Count; i++)
        {
            for (var j = 0; j < maze[i].Count; j++)
            {
                if (maze[i][j] is 'S')
                {
                    _ = vertices.TryGetValue(new Vertex(i, j), out start!);
                }
                if (maze[i][j] is 'E')
                {
                    _ = vertices.TryGetValue(new Vertex(i, j), out end!);
                }
            }
        }

        var distances = vertices.DijkstraWithPrevious(end, start, out var previousVertex);
        var shortestPathVertices = GetMinPathVetices(previousVertex, end, start);
        var count = 0;
        foreach (var v in shortestPathVertices)
        {
            var possiblejumpVertices = GetAtDistance(v, 2);
            foreach (var u in possiblejumpVertices)
            {
                if (vertices.TryGetValue(u, out var cheatVertex))
                {
                    count += distances[v] - distances[u] - 2 >= minShortcutCost
                        ? 1
                        : 0;
                }
            }
        }

        return count.ToString();
    }

    public string SolvePart2(string input)
    {
        const long minShortcutCost = 100;
        const long cheatDistance = 20;

        var maze = input.ParseAsMatrix(c => c);
        var vertices = ExtractGraph(maze);
        Vertex start = new Vertex(-1, -1);
        Vertex end = new Vertex(-1, -1);
        for (var i = 0; i < maze.Count; i++)
        {
            for (var j = 0; j < maze[i].Count; j++)
            {
                if (maze[i][j] is 'S')
                {
                    _ = vertices.TryGetValue(new Vertex(i, j), out start!);
                }
                if (maze[i][j] is 'E')
                {
                    _ = vertices.TryGetValue(new Vertex(i, j), out end!);
                }
            }
        }

        var distances = vertices.DijkstraWithPrevious(end, start, out var previousVertex);
        var shortestPathVertices = GetMinPathVetices(previousVertex, end, start);
        var count = 0;
        foreach (var v in shortestPathVertices)
        {
            var possiblejumpVertices = GetAtDistance(v, (int)cheatDistance);
            foreach (var u in possiblejumpVertices)
            {
                if (vertices.TryGetValue(u, out var cheatVertex))
                {
                    count += distances[v] - distances[u] - GetNormOneDistance(v, u) >= minShortcutCost
                        ? 1
                        : 0;
                }
            }
        }

        return count.ToString();
    }

    private static ISet<Vertex> GetAtDistance(Vertex v, int distance)
    {
        var ball = new HashSet<Vertex>();
        for (var i = -distance; i <= distance; i++)
        {
            for (var j = -distance; j <= distance; j++)
            {
                if (Math.Abs(i) + Math.Abs(j) > distance || Math.Abs(i) + Math.Abs(j) <= 1) continue;
                ball.Add(new Vertex(v.I + i, v.J + j));
            }
        }

        return ball;
    }

    private static long GetNormOneDistance(Vertex v, Vertex u)
    {
        return Math.Abs(v.I - u.I) + Math.Abs(v.J - u.J);
    }

    private static HashSet<Vertex> GetMinPathVetices(IDictionary<Vertex, Vertex> previous, Vertex start, Vertex end)
    {
        var path = new HashSet<Vertex> { end };
        var current = end;
        while (current != start)
        {
            var next = previous[current];
            path.Add(next);
            current = next;
        }

        return path;
    }

    private static HashSet<Vertex> ExtractGraph(List<List<char>> maze)
    {
        var vertices = new HashSet<Vertex>();
        for (var i = 0; i < maze.Count; i++)
        {
            for (var j = 0; j < maze[i].Count; j++)
            {
                if (maze[i][j] is not '#')
                {
                    vertices.Add(new Vertex(i, j));
                }
            }
        }

        foreach (var v in vertices)
        {
            if (vertices.TryGetValue(new Vertex(v.I + 1, v.J), out var adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if (vertices.TryGetValue(new Vertex(v.I - 1, v.J), out adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if (vertices.TryGetValue(new Vertex(v.I, v.J - 1), out adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if (vertices.TryGetValue(new Vertex(v.I, v.J + 1), out adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
        }

        return vertices;
    }
}
