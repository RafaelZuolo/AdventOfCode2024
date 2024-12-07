namespace AdventOfCode2024.Days;

public class Day07 : IDay
{
    public string SolvePart1(string input)
    {
        var lines = input.ParseLines();
        var equations = lines.Select(l =>
        {
            var members = l.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var target = long.Parse(members[0]);
            var factors = members[1].Split(" ").Select(long.Parse).ToArray();

            return new Equation(target, factors);
        })
            .ToList();

        return equations
            .Aggregate(
                (long)0,
                (accumulated, equation) => accumulated + equation.Solve())
            .ToString();
    }

    public string SolvePart2(string input)
    {
        var lines = input.ParseLines();
        var equations = lines.Select(l =>
        {
            var members = l.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var target = long.Parse(members[0]);
            var factors = members[1].Split(" ").Select(long.Parse).ToArray();

            return new EquationWithConcat(target, factors);
        })
            .ToList();

        return equations
            .Aggregate(
                (long)0,
                (accumulated, equation) => accumulated + equation.Solve())
            .ToString();
    }
}

enum Operations { sum, prod, concat }

class Equation
{
    private readonly Operations[] operations;

    public long Target { get; }
    public long[] Factors { get; }

    public Equation(long target, long[] factors)
    {
        Target = target;
        Factors = factors;
        operations = new Operations[factors.Length - 1];
    }

    public long Evaluate()
    {
        var accumulator = Factors[0];
        for (int i = 0; i < operations.Length; i++)
        {
            if (operations[i] is Operations.sum)
                accumulator += Factors[i + 1];
            else
                accumulator *= Factors[i + 1];
        }

        return accumulator;
    }

    public bool IsAllProd() => operations.All(o => o is Operations.prod);

    public void NextEnumeration()
    {
        for (int i = 0; i < operations.Length; i++)
        {
            if (operations[i] is Operations.sum)
            {
                operations[i] = Operations.prod;
                return;
            }

            operations[i] = Operations.sum;
        }
    }

    public long Solve()
    {
        SetToProd();
        do
        {
            if (Target == Evaluate())
            {
                return Target;
            }

            NextEnumeration();
        } while (!IsAllProd());

        return 0;
    }

    private void SetToProd()
    {
        for (int i = 0; i < operations.Length; i++)
        {
            operations[i] = Operations.prod;
        }
    }
}

class EquationWithConcat
{
    private readonly Operations[] operations;

    public long Target { get; }
    public long[] Factors { get; }

    public EquationWithConcat(long target, long[] factors)
    {
        Target = target;
        Factors = factors;
        operations = new Operations[factors.Length - 1];
    }

    public long Evaluate()
    {
        var accumulator = Factors[0];
        for (int i = 0; i < operations.Length; i++)
        {
            if (operations[i] is Operations.sum)
                accumulator += Factors[i + 1];
            else if (operations[i] is Operations.concat)
                accumulator = long.Parse(accumulator.ToString() + Factors[i + 1].ToString());
            else
                accumulator *= Factors[i + 1];
        }

        return accumulator;
    }

    public bool IsAllProd() => operations.All(o => o is Operations.prod);

    public void NextEnumeration()
    {
        for (int i = 0; i < operations.Length; i++)
        {
            if (operations[i] is Operations.sum)
            {
                operations[i] = Operations.concat;
                return;
            }
            if (operations[i] is Operations.concat)
            {
                operations[i] = Operations.prod;
                return;
            }

            operations[i] = Operations.sum;
        }
    }

    public long Solve()
    {
        SetToProd();
        do
        {
            if (Target == Evaluate())
            {
                return Target;
            }

            NextEnumeration();
        } while (!IsAllProd());

        return 0;
    }

    private void SetToProd()
    {
        for (int i = 0; i < operations.Length; i++)
        {
            operations[i] = Operations.prod;
        }
    }
}
