namespace AdventOfCode2024.Days;

public class Day23 : IDay
{
    public string SolvePart1(string input)
    {
        var undirectedEdges = input.ParseLines().Select(l => (UName: l.Split('-')[0], VName: l.Split('-')[1]));
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

        var triangles = new HashSet<Triangle>();
        foreach (var u in vertices.Values)
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
        return "not solved";
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
}
