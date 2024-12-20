namespace AdventOfCode2024.Days;

public class Day16 : IDay
{
    public string SolvePart1(string input)
    {
        var maze = input.ParseAsMatrix(c => c);
        var vertices = ExtractGraph(maze);
        Vertex start = new Vertex(-1, -1, Direction.Irrelevant);
        Vertex end = new Vertex(-1, -1, Direction.Irrelevant);
        for (var i = 0; i < maze.Count; i++)
        {
            for (var j = 0; j < maze[i].Count; j++)
            {
                if (maze[i][j] is 'S')
                {
                    start = new Vertex(i, j, Direction.Irrelevant);
                }
                if (maze[i][j] is 'E')
                {
                    end = new Vertex(i, j, Direction.Irrelevant);
                }
            }
        }
        start.Adjacency.UnionWith(vertices
            .Where(v => v.I == start.I && v.J == start.J)
            .Select(v => new Edge(start, v, v.Direction is Direction.Horizontal ? 0 : 1000)));
        foreach (var vertex in vertices.Where(v => v.I == end.I && v.J == end.J))
        {
            vertex.Adjacency.Add(new Edge(vertex, end, 0));
        }

        vertices.Add(start);
        vertices.Add(end);
        long cost = vertices.Dijkstra(start, end)[end];

        return cost.ToString();
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
                    vertices.Add(new Vertex(i, j, Direction.Vertical));
                    vertices.Add(new Vertex(i, j, Direction.Horizontal));
                }
            }
        }

        foreach (var v in vertices.Where(u => u.Direction is Direction.Vertical))
        {
            if (vertices.TryGetValue(new Vertex(v.I + 1, v.J, Direction.Vertical), out var adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if (vertices.TryGetValue(new Vertex(v.I - 1, v.J, Direction.Vertical), out adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if ((vertices.Contains(new Vertex(v.I, v.J - 1, Direction.Horizontal))
                || vertices.Contains(new Vertex(v.I, v.J + 1, Direction.Horizontal)))
                && vertices.TryGetValue(new Vertex(v.I, v.J, Direction.Horizontal), out adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1000));
            }
        }
        foreach (var v in vertices.Where((Func<Vertex, bool>)(u => u.Direction is Direction.Horizontal)))
        {
            if (vertices.TryGetValue(new Vertex(v.I, v.J + 1, Direction.Horizontal), out var adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if (vertices.TryGetValue(new Vertex(v.I, v.J - 1, Direction.Horizontal), out adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if ((vertices.Contains(new Vertex(v.I - 1, v.J, Direction.Horizontal))
                || vertices.Contains(new Vertex(v.I + 1, v.J, Direction.Horizontal)))
                && vertices.TryGetValue(new Vertex(v.I, v.J, Direction.Vertical), out adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1000));
            }
        }

        return vertices;
    }

    public string SolvePart2(string input)
    {
        var maze = input.ParseAsMatrix(c => c);
        var vertices = ExtractGraph(maze);
        Vertex start = new Vertex(-1, -1, Direction.Irrelevant);
        Vertex end = new Vertex(-1, -1, Direction.Irrelevant);
        for (var i = 0; i < maze.Count; i++)
        {
            for (var j = 0; j < maze[i].Count; j++)
            {
                if (maze[i][j] is 'S')
                {
                    start = new Vertex(i, j, Direction.Irrelevant);
                }
                if (maze[i][j] is 'E')
                {
                    end = new Vertex(i, j, Direction.Irrelevant);
                }
            }
        }
        start.Adjacency.UnionWith(vertices
            .Where(v => v.I == start.I && v.J == start.J)
            .Select(v => new Edge(start, v, v.Direction is Direction.Horizontal ? 0 : 1000)));
        foreach (var vertex in vertices.Where(v => v.I == end.I && v.J == end.J))
        {
            vertex.Adjacency.Add(new Edge(vertex, end, 0));
        }

        vertices.Add(start);
        vertices.Add(end);
        var distances = vertices.Dijkstra(start, end);
        var pathVertices = GetVeticesFromMinPaths(distances, start, end);
        var validVertex = pathVertices.Select(v => (v.I, v.J)).ToHashSet();

        return validVertex.Count.ToString();
    }

    private static ISet<Vertex> GetVeticesFromMinPaths(IDictionary<Vertex, long> distances, Vertex start, Vertex end)
    {
        var vertices = new HashSet<Vertex> { end };
        var candidates = distances.Keys.Where(v => v.IsAdjacentTo(end)).ToHashSet();

        GetMinPath(end);

        return vertices;

        void GetMinPath(Vertex current)
        {
            if (current == start) return;

            var candidates = distances.Keys.Where(v => v.IsAdjacentTo(current)).ToHashSet();

            foreach (var candidate in candidates)
            {
                if (distances[candidate] + candidate.Adjacency.Single(e => e.To == current).Weight == distances[current])
                {
                    vertices.Add(candidate);
                    GetMinPath(candidate);
                }
            }
        }
    }
}
