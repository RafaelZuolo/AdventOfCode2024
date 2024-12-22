




using System.Text;

namespace AdventOfCode2024.Days;

public class Day21 : IDay
{
    private static readonly List<List<char>> numericKeypad =
    [
        ['7', '8', '9'],
        ['4', '5', '6'],
        ['1', '2', '3'],
        ['#', '0', 'A'],
    ];
    private static readonly List<List<char>> directionalKeypad =
    [
        ['#', '^', 'A'],
        ['<', 'v', '>'],
    ];
    private static readonly List<char> numericKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A'];
    private static readonly List<char> directionalKeys = ['<', '>', '^', 'v', 'A'];

    private const long DownCost = 1;
    private const long LeftCost = 1;

    public string SolvePart1(string input)
    {
        var directionalSequencesByNumericKeyPair = numericKeys
            .SelectMany(n => numericKeys.Select(m => (From: n, To: m)))
            .ToDictionary(key => key, value => new List<char>());
        foreach (var pair in directionalSequencesByNumericKeyPair)
        {
            directionalSequencesByNumericKeyPair[pair.Key] = BfsNumericKeypad(pair.Key.From, pair.Key.To);
        }

        var directionalSequenceByDirectionalFromTo = directionalKeys
            .SelectMany(n => directionalKeys.Select(m => (From: n, To: m)))
            .ToDictionary(key => key, value => new List<char>());
        foreach (var pair in directionalSequenceByDirectionalFromTo)
        {
            directionalSequenceByDirectionalFromTo[pair.Key] = BfsDirectionalKeypad(pair.Key.From, pair.Key.To);
        }

        foreach (var pair in directionalSequencesByNumericKeyPair)
        {
            var newSequences = IterateRobot(pair.Value, directionalSequenceByDirectionalFromTo);
            directionalSequencesByNumericKeyPair[pair.Key] = newSequences;
        }
        foreach (var pair in directionalSequencesByNumericKeyPair)
        {
            var newSequences = IterateRobot(pair.Value, directionalSequenceByDirectionalFromTo);
            directionalSequencesByNumericKeyPair[pair.Key] = newSequences;
        }

        var codes = input.ParseLines().Select(c => ChangeCodeToRobotInstruction(c, directionalSequencesByNumericKeyPair)).ToList();
        var numericValuesOfCodes = input.ParseLines().Select(l => long.Parse(l[..3])).ToList();

        var sum = (long)0;
        for (var i = 0; i < codes.Count; i++)
        {
            sum += numericValuesOfCodes[i] * codes[i].Length;
        }

        return sum.ToString();
    }

    private string ChangeCodeToRobotInstruction(string code, Dictionary<(char From, char To), List<char>> directionalSequencesByNumericKeyPair)
    {
        var result = new StringBuilder();
        for (var i = 0; i < code.Length - 1; i++)
        {
            result.Append(string.Concat(directionalSequencesByNumericKeyPair[(code[i], code[i + 1])]));
        }

        return result.ToString();
    }

    private List<char> IterateRobot(List<char> sequenceList, Dictionary<(char From, char To), List<char>> directionalSequenceByDirectionalFromTo)
    {
        if (sequenceList.Count == 1) return ['A'];
        var result = new List<char>();
        for (var i = 0; i < sequenceList.Count - 1; i++)
        {
            result.AddRange(directionalSequenceByDirectionalFromTo[(sequenceList[i], sequenceList[i + 1])]);
        }

        return result;
    }

    private List<char> BfsNumericKeypad(char from, char to)
    {
        return Bfs(from, to, numericKeypad);
    }

    private List<char> BfsDirectionalKeypad(char from, char to)
    {
        return Bfs(from, to, directionalKeypad);
    }

    private List<char> Bfs(char from, char to, List<List<char>> keypad)
    {
        var fromI = Enumerable.Range(0, keypad.Count)
            .Where(index => keypad[index].Contains(from))
            .Single();
        var fromJ = Enumerable.Range(0, keypad[fromI].Count).Where(index => keypad[fromI][index] == from).Single();
        var toI = Enumerable.Range(0, keypad.Count)
            .Where(index => keypad[index].Contains(to))
            .Single();
        var toJ = Enumerable.Range(0, keypad[toI].Count).Where(index => keypad[toI][index] == to).Single();

        return GenerateGraphAndGetMinPath(fromI, fromJ, toI, toJ, keypad);
    }

    private static List<char> GenerateGraphAndGetMinPath(int fromI, int fromJ, int toI, int toJ, List<List<char>> maze)
    {
        var vertices = new HashSet<Vertex>();
        for (var i = 0; i < maze.Count; i++)
        {
            for (var j = 0; j < maze[i].Count; j++)
            {
                if (maze[i][j] is not '#')
                    vertices.Add(new Vertex(i, j));
            }
        }
        for (var i = 0; i < maze.Count; i++)
        {
            for (var j = 0; j < maze[i].Count; j++)
            {
                if (vertices.TryGetValue(new Vertex(i, j), out var v))
                {
                    if (vertices.TryGetValue(new Vertex(i + 1, j), out var adj))
                    {
                        v.Adjacency.Add(new Edge(v, adj, 100 - j));
                    }
                    if (vertices.TryGetValue(new Vertex(i, j + 1), out adj))
                    {
                        v.Adjacency.Add(new Edge(v, adj, 100 - j));
                    }
                    if (vertices.TryGetValue(new Vertex(i - 1, j), out adj))
                    {
                        v.Adjacency.Add(new Edge(v, adj, 100 - j));
                    }
                    if (vertices.TryGetValue(new Vertex(i, j - 1), out adj))
                    {
                        v.Adjacency.Add(new Edge(v, adj, 100 - j));
                    }
                }
            }
        }
        _ = vertices.TryGetValue(new Vertex(fromI, fromJ), out var start);
        _ = vertices.TryGetValue(new Vertex(toI, toJ), out var end);
        _ = vertices.DijkstraWithPrevious(start!, end!, out var previousVertex);

        var minPath = GetMinPathVetices(previousVertex, start!, end!);

        return minPath;

        static List<char> GetMinPathVetices(IDictionary<Vertex, Vertex> previous, Vertex start, Vertex end)
        {
            var path = new List<char>();
            var current = end;
            while (current != start)
            {
                var previousVertex = previous[current];
                if (current.I > previousVertex.I)
                    path.Insert(0, 'v');
                if (current.I < previousVertex.I)
                    path.Insert(0, '^');
                if (current.J > previousVertex.J)
                    path.Insert(0, '>');
                if (current.J < previousVertex.J)
                    path.Insert(0, '<');
                current = previousVertex;
            }
            path.Add('A');

            return path;
        }
    }

    public string SolvePart2(string input)
    {
        return "not solved";
    }
}
