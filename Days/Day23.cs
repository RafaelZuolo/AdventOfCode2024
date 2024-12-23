namespace AdventOfCode2024.Days;

public class Day23 : IDay
{
    public string SolvePart1(string input)
    {
        var undirectedEdges = input.ParseLines().Select(l => (UName: l.Split('-')[0], VName: l.Split('-')[1]));
        var vertices = ParseGraph(undirectedEdges);

        var triangles = new HashSet<Triangle>();
        foreach (var u in vertices)
        {
            foreach (var v in u.AdjacencySet)
            {
                foreach (var w in v.AdjacencySet)
                {
                    if (w.IsAdjacentTo(u))
                    {
                        triangles.Add(new Triangle(u, v, w));
                    }
                }
            }
        }

        var tTriangles = triangles
            .Where(t => t.V1.Name.StartsWith('t') || t.V2.Name.StartsWith('t') || t.V3.Name.StartsWith('t'))
            .ToHashSet();

        return tTriangles.Count.ToString();
    }

    public string SolvePart2(string input)
    {
        var undirectedEdges = input.ParseLines().Select(l => (UName: l.Split('-')[0], VName: l.Split('-')[1]));
        var vertices = ParseGraph(undirectedEdges);

        var triangles = new HashSet<Clique>();
        foreach (var u in vertices)
        {
            foreach (var v in u.AdjacencySet)
            {
                foreach (var w in v.AdjacencySet)
                {
                    if (w.IsAdjacentTo(u))
                    {
                        triangles.Add(new Clique(new HashSet<Vertex>() { u, v, w }));
                    }
                }
            }
        }

        var maximalCliques = triangles;
        while (maximalCliques.Count > 1)
        {
            maximalCliques = GrowCliques(vertices, maximalCliques);
        }

        var names = maximalCliques.Single().Vertices.Select(v => v.Name).ToList();
        names.Sort();
        var password = string.Join(',', names);

        return password;
    }

    private static HashSet<Clique> GrowCliques(ISet<Vertex> vertices, HashSet<Clique> maximalCliques)
    {
        var augmentedCliques = new HashSet<Clique>();
        var currentCliqueSize = maximalCliques.First().Vertices.Count;
        foreach (var vertex in vertices.Where(v => v.Degree >= currentCliqueSize))
        {
            foreach (var clique in maximalCliques)
            {
                if (clique.Vertices.All(cliqueMember => cliqueMember.IsAdjacentTo(vertex)))
                {
                    augmentedCliques.Add(new Clique(new HashSet<Vertex>(clique.Vertices) { vertex }));
                }
            }
        }

        return augmentedCliques;
    }

    private static ISet<Vertex> ParseGraph(IEnumerable<(string UName, string VName)> undirectedEdges)
    {
        var vertices = new Dictionary<string, Vertex>();
        foreach (var (UName, VName) in undirectedEdges)
        {
            if (!vertices.TryGetValue(UName, out var u))
            {
                u = new Vertex(name: UName);
                vertices.Add(UName, u);
            }
            if (!vertices.TryGetValue(VName, out var v))
            {
                v = new Vertex(name: VName);
                vertices.Add(VName, v);
            }

            u.Adjacency.Add(new Edge(u, v));
            v.Adjacency.Add(new Edge(v, u));
        }

        return vertices.Values.ToHashSet();
    }

    class Triangle(Vertex v1, Vertex v2, Vertex v3) : IEquatable<Triangle>
    {
        public Vertex V1 { get; } = v1;
        public Vertex V2 { get; } = v2;
        public Vertex V3 { get; } = v3;

        public bool Equals(Triangle? other)
        {
            return other is Triangle t && t.GetHashCode() == GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Triangle);
        }

        public override int GetHashCode()
        {
            return V1.GetHashCode() + V2.GetHashCode() + V3.GetHashCode();
        }
    }

    class Clique : IEquatable<Clique>
    {
        public ISet<Vertex> Vertices { get; }

        public Clique(ISet<Vertex> vertices)
        {
            Vertices = vertices;
        }

        public bool Equals(Clique? other)
        {
            return other is Clique t && t.GetHashCode() == GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Clique);
        }

        public override int GetHashCode()
        {
            return (int)Vertices.Sum(v => (long)v.GetHashCode());
        }
    }
}
