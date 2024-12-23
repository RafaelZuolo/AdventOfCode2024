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

        var maxDegree = vertices.Select(v => v.Degree).Max();
        var password = "";
        //for (int i = maxDegree + 1; i >= 3; i--)
        //{
        //    if (TryFindClique(vertices, i, out var clique))
        //    {
        //        var sortedClique = clique.Select(v => v.Name).ToList();
        //        sortedClique.Sort();
        //        password = string.Join(',', sortedClique);

        //        break;
        //    }
        //}

        var triangles = new HashSet<HashSet<Vertex>>();
        foreach (var u in vertices)
        {
            foreach (var v in u.AdjacencySet)
            {
                foreach (var w in v.AdjacencySet)
                {
                    if (w.IsAdjacentTo(u))
                    {
                        triangles.Add([u, v, w]);
                    }
                }
            }
        }
        var maximalCliques = triangles;
        while (maximalCliques.Count > 1)
        {
            maximalCliques = GrowCliques(vertices, maximalCliques);
        }

        var names = maximalCliques.Single().Select(v => v.Name).ToList();
        names.Sort();
        password = string.Join(',', names);

        return password;
    }

    private HashSet<HashSet<Vertex>> GrowCliques(ISet<Vertex> vertices, HashSet<HashSet<Vertex>> maximalCliques)
    {
        var augmentedCliques = new HashSet<HashSet<Vertex>>();
        foreach (var vertex in vertices)
        {
            foreach (var clique in maximalCliques)
            {
                if (clique.All(cliqueMember => cliqueMember.IsAdjacentTo(vertex)))
                {
                    augmentedCliques.Add(new HashSet<Vertex>(clique) { vertex });
                }
            }
        }

        return augmentedCliques;
    }

    private bool TryFindClique(ISet<Vertex> vertices, int cliqueSize, out ISet<Vertex> clique)
    {
        clique = new HashSet<Vertex>();
        var candidates = vertices.Where(v => v.Degree >= cliqueSize - 1).ToHashSet();
        if (candidates.Count < cliqueSize)
        {
            return false;
        }

        if (cliqueSize is 3)
        {
            foreach (var u in candidates)
            {
                foreach (var v in u.GetInducedAdjacency(candidates))
                {
                    foreach (var w in v.GetInducedAdjacency(candidates))
                    {
                        if (w.IsAdjacentTo(u))
                        {
                            clique = new HashSet<Vertex>() { u, v, w };
                            return true;
                        }
                    }
                }
            }
        }

        foreach (var vertex in candidates)
        {
            if (TryFindClique(vertex.GetInducedAdjacency(candidates), cliqueSize - 1, out var subClique))
            {
                clique.Add(vertex);
                clique.UnionWith(subClique);
                return true;
            }
        }

        return false;
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
            return Vertices.Sum(v => v.GetHashCode());
        }
    }
}
