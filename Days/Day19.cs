namespace AdventOfCode2024.Days;

public class Day19 : IDay
{
    public string SolvePart1(string input)
    {
        var towels = input.Split(Environment.NewLine + Environment.NewLine)[0]
            .Split(", ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var patterns = input.Split(Environment.NewLine + Environment.NewLine)[1].ParseLines();
        var start = ParseStatesFrom(towels);

        var count = (long)0;
        foreach (var pattern in patterns)
        {
            count += IsPerfectMatch(pattern, start) ? 1 : 0;
        }

        return count.ToString();
    }

    public string SolvePart2(string input)
    {
        var towels = input.Split(Environment.NewLine + Environment.NewLine)[0]
            .Split(", ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var patterns = input.Split(Environment.NewLine + Environment.NewLine)[1].ParseLines();

        var count = (long)0;
        foreach (var pattern in patterns)
        {
            count += Arrange(pattern);
        }

        return count.ToString();

        long Arrange(string pattern)
        {
            return Arrange(pattern, []);

            long Arrange(string pattern, Dictionary<string, long> memory)
            {
                if (string.IsNullOrEmpty(pattern)) return 1;
                var count = (long)0;

                if (memory.TryGetValue(pattern, out var value))
                {
                    return value;
                }

                foreach (string towel in towels)
                {
                    if (pattern.StartsWith(towel))
                    {
                        count += Arrange(pattern[towel.Length..], memory);
                    }
                }

                memory.Add(pattern, count);
                return count;
            }
        }
    }

    private static Node ParseStatesFrom(string[] towels)
    {
        var start = new Node();
        foreach (var towel in towels)
        {
            var currentNode = start;
            for (int i = 0; i < towel.Length; i++)
            {
                var nextNode = new Node(towel[i]);
                if (currentNode.Next.Any(n => n.Value == towel[i]))
                {
                    currentNode = currentNode.Next.Single(n => n.Value == towel[i]);
                }
                else
                {
                    currentNode.Next.Add(nextNode);
                    currentNode = nextNode;
                }
            }
            currentNode.IsTerminal = true;
        }

        return start;
    }

    private bool IsPerfectMatch(string pattern, Node start)
    {
        var currentStates = new HashSet<Node> { start };

        foreach (var c in pattern)
        {
            var nextState = new HashSet<Node>();
            foreach (var node in currentStates)
            {
                if (node.Next.Any(n => n.Value == c))
                {
                    nextState.Add(node.Next.Single(n => n.Value == c));
                }
                if (nextState.Any(n => n.IsTerminal))
                {
                    nextState.Add(start);
                }
            }
            currentStates = nextState;
        }

        return currentStates.Any(n => n.IsTerminal);
    }

    class Node(char value = '$')
    {
        public char Value { get; } = value;
        public bool IsTerminal { get; set; } = false;
        public ISet<Node> Next { get; set; } = new HashSet<Node>();
    }
}
