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

    public string SolvePart1(string input)
    {
        var directionalSequencesByNumericKeyPair = numericKeys
            .SelectMany(n => numericKeys.Select(m => (From: n, To: m)))
            .ToDictionary(key => key, value => new List<List<char>>());
        foreach (var pair in directionalSequencesByNumericKeyPair)
        {
            directionalSequencesByNumericKeyPair[pair.Key] = BfsNumericKeypad(pair.Key.From, pair.Key.To);
        }
        var directionalSequenceByDirectionalFromTo = directionalKeys
            .SelectMany(n => directionalKeys.Select(m => (From: n, To: m)))
            .ToDictionary(key => key, value => new List<List<char>>());
        foreach (var pair in directionalSequenceByDirectionalFromTo)
        {
            directionalSequenceByDirectionalFromTo[pair.Key] = BfsDirectionalKeypad(pair.Key.From, pair.Key.To);
        }

        var codesRaw1 = input.ParseLines().Select(c => ChangeCodeToRobotInstruction(c, directionalSequencesByNumericKeyPair)).ToList();

        var codesRaw2 = codesRaw1
            .Select(d => d.SelectMany(c => ChangeCodeToRobotInstruction(c, directionalSequenceByDirectionalFromTo)).ToList())
            .ToList();

        var codes = codesRaw2
            .Select(d => d.SelectMany(c => ChangeCodeToRobotInstruction(c, directionalSequenceByDirectionalFromTo)).ToList())
            .ToList();
        var trimmedCodes = codes.Select(TrimmCodes).ToList();
        var numericValuesOfCodes = input.ParseLines().Select(l => long.Parse(l[..3])).ToList();

        var sum = (long)0;
        for (var i = 0; i < trimmedCodes.Count; i++)
        {
            sum += numericValuesOfCodes[i] * trimmedCodes[i].First().Length;
        }

        return sum.ToString();
    }

    private static List<string> TrimmCodes(List<string> codes)
    {
        var minLengh = codes.Select(c => c.Length).Min();

        return codes.Where(c => c.Length == minLengh).ToList();
    }

    private List<string> ChangeCodeToRobotInstruction(
        string code,
        Dictionary<(char From, char To), List<List<char>>> directionalSequencesByKeyPair)
    {
        var result = new StringBuilder();
        code = "A" + code;
        var finalInstructions = new List<string>();
        BuildSequence(code, 0, "");

        return finalInstructions;

        void BuildSequence(
        string code,
        int index,
        string currentSequence)
        {
            if (index == code.Length - 1)
            {
                finalInstructions.Add(currentSequence);
                return;
            }

            foreach (var sequence in directionalSequencesByKeyPair[(code[index], code[index + 1])])
            {
                BuildSequence(code, index + 1, currentSequence + new string(sequence.ToArray()));
            }
        }
    }

    private List<List<char>> BfsNumericKeypad(char from, char to)
    {
        return Bfs(from, to, numericKeypad);
    }

    private List<List<char>> BfsDirectionalKeypad(char from, char to)
    {
        return Bfs(from, to, directionalKeypad);
    }

    private List<List<char>> Bfs(char from, char to, List<List<char>> keypad)
    {
        var fromI = Enumerable.Range(0, keypad.Count)
            .Where(index => keypad[index].Contains(from))
            .Single();
        var fromJ = Enumerable.Range(0, keypad[fromI].Count).Where(index => keypad[fromI][index] == from).Single();
        var toI = Enumerable.Range(0, keypad.Count)
            .Where(index => keypad[index].Contains(to))
            .Single();
        var toJ = Enumerable.Range(0, keypad[toI].Count).Where(index => keypad[toI][index] == to).Single();

        return GenerateGraphAndGetMinPaths(fromI, fromJ, toI, toJ, keypad);
    }

    private static List<List<char>> GenerateGraphAndGetMinPaths(int fromI, int fromJ, int toI, int toJ, List<List<char>> maze)
    {
        if (fromI == toI && fromJ == toJ) return [['A']];

        var verticalSteps = toI - fromI;
        var verticalMovement = Enumerable.Range(0, Math.Abs(verticalSteps)).Select(_ => verticalSteps > 0 ? 'v' : '^').ToList();

        var horizontalSteps = toJ - fromJ;
        var horizontalMovement = Enumerable.Range(0, Math.Abs(horizontalSteps)).Select(_ => horizontalSteps > 0 ? '>' : '<').ToList();

        if (verticalSteps == 0) return [[.. horizontalMovement, 'A']];
        if (horizontalSteps == 0) return [[.. verticalMovement, 'A']];

        if (maze[toI][fromJ] is '#')
        {
            return [[.. horizontalMovement, .. verticalMovement.Append('A')]];
        }
        if (maze[fromI][toJ] is '#')
        {
            return [[.. verticalMovement, .. horizontalMovement.Append('A')]];
        }

        return
        [
            [.. horizontalMovement, .. verticalMovement.Append('A')],
            [.. verticalMovement, .. horizontalMovement.Append('A')],
        ];
    }

    public string SolvePart2(string input)
    {
        return "not solved";
    }
}
