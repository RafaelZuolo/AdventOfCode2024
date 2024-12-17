namespace AdventOfCode2024.Days;

public class Day16 : IDay
{
    public string SolvePart1(string input)
    {
        var maze = input.ParseAsMatrix(c => c);
        var vertices = ExtractGraph(maze);
        Vertex start = new Vertex(-1, -1, Day16Direction.Irrelevant);
        Vertex end = new Vertex(-1, -1, Day16Direction.Irrelevant);
        for (var i = 0; i < maze.Count; i++)
        {
            for (var j = 0; j < maze[i].Count; j++)
            {
                if (maze[i][j] is 'S')
                {
                    start = new Vertex(i, j, Day16Direction.Irrelevant);
                }
                if (maze[i][j] is 'E')
                {
                    end = new Vertex(i, j, Day16Direction.Irrelevant);
                }
            }
        }
        start.Adjacency.UnionWith(vertices
            .Where(v => v.I == start.I && v.J == start.J)
            .Select(v => new Edge(start, v, v.Direction is Day16Direction.Horizontal ? 0 : 1000)));
        foreach (var vertex in vertices.Where(v => v.I == end.I && v.J == end.J))
        {
            vertex.Adjacency.Add(new Edge(vertex, end, 0));
        }

        vertices.Add(start);
        vertices.Add(end);
        long cost = Dijkstra(vertices, start, end)[end];

        return cost.ToString();
    }

    private static IDictionary<Vertex, long> Dijkstra(HashSet<Vertex> vertices, Vertex start, Vertex end)
    {
        var finalDistances = vertices.ToDictionary(v => v, v => (long)-1); // -1 represent unreachable vertex
        var visited = new HashSet<Vertex> { start };
        var frontierVertices = start.Adjacency.ToHashSet();
        finalDistances[start] = 0;

        while (!visited.Contains(end))
        {
            Explore(finalDistances, frontierVertices, visited);
        }

        return finalDistances;

        static void Explore(Dictionary<Vertex, long> finalDistances, HashSet<Edge> frontierEdges, HashSet<Vertex> visited)
        {
            var minDist = long.MaxValue;
            Edge? minEdge = null;
            foreach (var edge in frontierEdges)
            {
                if (edge.Weight + finalDistances[edge.From] < minDist)
                {
                    minDist = edge.Weight + finalDistances[edge.From];
                    minEdge = edge;
                }
            }

            finalDistances[minEdge!.To] = minDist;

            visited.Add(minEdge.To);
            frontierEdges.UnionWith(minEdge.To.Adjacency);
            frontierEdges.RemoveWhere(x => visited.Contains(x.To));
        }
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
                    vertices.Add(new Vertex(i, j, Day16Direction.Vertical));
                    vertices.Add(new Vertex(i, j, Day16Direction.Horizontal));
                }
            }
        }

        foreach (var v in vertices.Where(u => u.Direction is Day16Direction.Vertical))
        {
            if (vertices.TryGetValue(new Vertex(v.I + 1, v.J, Day16Direction.Vertical), out var adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if (vertices.TryGetValue(new Vertex(v.I - 1, v.J, Day16Direction.Vertical), out adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if ((vertices.Contains(new Vertex(v.I, v.J - 1, Day16Direction.Horizontal))
                || vertices.Contains(new Vertex(v.I, v.J + 1, Day16Direction.Horizontal)))
                && vertices.TryGetValue(new Vertex(v.I, v.J, Day16Direction.Horizontal), out adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1000));
            }
        }
        foreach (var v in vertices.Where((Func<Vertex, bool>)(u => u.Direction is Day16Direction.Horizontal)))
        {
            if (vertices.TryGetValue(new Vertex(v.I, v.J + 1, Day16Direction.Horizontal), out var adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if (vertices.TryGetValue(new Vertex(v.I, v.J - 1, Day16Direction.Horizontal), out adj))
            {
                v.Adjacency.Add(new Edge(v, adj, 1));
            }
            if ((vertices.Contains(new Vertex(v.I - 1, v.J, Day16Direction.Horizontal))
                || vertices.Contains(new Vertex(v.I + 1, v.J, Day16Direction.Horizontal)))
                && vertices.TryGetValue(new Vertex(v.I, v.J, Day16Direction.Vertical), out adj))
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
        Vertex start = new Vertex(-1, -1, Day16Direction.Irrelevant);
        Vertex end = new Vertex(-1, -1, Day16Direction.Irrelevant);
        for (var i = 0; i < maze.Count; i++)
        {
            for (var j = 0; j < maze[i].Count; j++)
            {
                if (maze[i][j] is 'S')
                {
                    start = new Vertex(i, j, Day16Direction.Irrelevant);
                }
                if (maze[i][j] is 'E')
                {
                    end = new Vertex(i, j, Day16Direction.Irrelevant);
                }
            }
        }
        start.Adjacency.UnionWith(vertices
            .Where(v => v.I == start.I && v.J == start.J)
            .Select(v => new Edge(start, v, v.Direction is Day16Direction.Horizontal ? 0 : 1000)));
        foreach (var vertex in vertices.Where(v => v.I == end.I && v.J == end.J))
        {
            vertex.Adjacency.Add(new Edge(vertex, end, 0));
        }

        vertices.Add(start);
        vertices.Add(end);
        var distances = Dijkstra(vertices, start, end);
        var pathVertices = GetVeticesFromMinPaths(distances, start, end);
        var validVertex = pathVertices.Select(v => (v.I, v.J)).ToHashSet();

        return validVertex.Count.ToString();
    }

    private ISet<Vertex> GetVeticesFromMinPaths(IDictionary<Vertex, long> distances, Vertex start, Vertex end)
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

class Vertex(int I, int J, Day16Direction Direction) : IEquatable<Vertex>
{
    public ISet<Edge> Adjacency { get; } = new HashSet<Edge>();
    public int I { get; } = I;
    public int J { get; } = J;
    public Day16Direction Direction { get; } = Direction;

    public bool IsAdjacentTo(Vertex other)
    {
        return Adjacency.Any(e => e.To == other);
    }

    public bool Equals(Vertex? other)
    {
        return other is Vertex v && v.I == I && v.J == J && v.Direction == Direction;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Vertex);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(I, J, Direction);
    }

    public static bool operator ==(Vertex current, Vertex other) => current.Equals(other);
    public static bool operator !=(Vertex current, Vertex other) => !(current == other);
}

record Edge(Vertex From, Vertex To, long Weight);

enum Day16Direction { Vertical, Horizontal, Irrelevant }
