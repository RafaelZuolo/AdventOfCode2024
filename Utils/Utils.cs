namespace AdventOfCode2024;

public static class Utils
{
    public static IList<string> ParseLines(this string input)
    {
        return input
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }

    public static List<List<T>> ParseAsMatrix<T>(this string input, Func<char, T> selector)
    {
        return input.ParseLines()
            .Select(l => l.Select(selector).ToList())
            .ToList();
    }

    public static void PrintMatrix<T>(this List<List<T>> matrix)
    {
        Console.WriteLine("------------------------------");
        for (int i = 0; i < matrix.Count; i++)
        {
            Console.WriteLine();
            for (int j = 0; j < matrix[i].Count; j++)
            {
                Console.Write(matrix[i][j]);
            }
        }
    }

    public static bool IsOutOfBounds<T>(this List<List<T>> matrix, int i, int j)
    {
        return i < 0 || i >= matrix.Count || j < 0 || j >= matrix[i].Count;
    }

    public static List<List<bool>> ToFalseMatrix<T>(this List<List<T>> matrix)
    {
        return matrix.Select(l => l.Select(_ => false).ToList()).ToList();
    }

    public static IDictionary<Vertex, long> Dijkstra(this ISet<Vertex> vertices, Vertex start, Vertex end)
    {
        var finalDistances = vertices.ToDictionary(v => v, v => (long)-1); // -1 represent unreachable vertex
        var visited = new HashSet<Vertex> { start };
        var frontierVertices = start.Adjacency.ToHashSet();
        finalDistances[start] = 0;

        while (!visited.Contains(end) && frontierVertices.Count > 0)
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

    public static ISet<Vertex> GetVeticesFromMinPaths(IDictionary<Vertex, long> distances, Vertex start, Vertex end)
    {
        var vertices = new HashSet<Vertex> { end };

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

    public static IDictionary<Vertex, long> DijkstraWithPrevious(
        this ISet<Vertex> vertices,
        Vertex start,
        Vertex end,
        out IDictionary<Vertex, Vertex> previousVertex)
    {
        var finalDistances = vertices.ToDictionary(v => v, v => (long)-1); // -1 represent unreachable vertex
        var visited = new HashSet<Vertex> { start };
        var frontierVertices = start.Adjacency.ToHashSet();
        finalDistances[start] = 0;

        previousVertex = new Dictionary<Vertex, Vertex>();

        while (!visited.Contains(end) && frontierVertices.Count > 0)
        {
            Explore(finalDistances, frontierVertices, visited, previousVertex);
        }

        return finalDistances;

        static void Explore(
            Dictionary<Vertex, long> finalDistances,
            HashSet<Edge> frontierEdges,
            HashSet<Vertex> visited,
            IDictionary<Vertex, Vertex> previousVertex)
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
            previousVertex.Add(minEdge.To, minEdge.From);
            frontierEdges.UnionWith(minEdge.To.Adjacency);
            frontierEdges.RemoveWhere(x => visited.Contains(x.To));
        }
    }
}

public class Vertex(int I, int J, Direction Direction = Direction.Irrelevant) : IEquatable<Vertex>
{
    public ISet<Edge> Adjacency { get; } = new HashSet<Edge>();
    public int I { get; } = I;
    public int J { get; } = J;
    public Direction Direction { get; } = Direction;

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

public record Edge(Vertex From, Vertex To, long Weight);

public enum Direction { Vertical, Horizontal, Irrelevant }