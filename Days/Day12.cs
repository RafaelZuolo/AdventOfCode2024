namespace AdventOfCode2024.Days;

public class Day12 : IDay
{
    public string SolvePart1(string input)
    {
        var garden = input.ParseAsMatrix(c => c);
        var visited = garden.ToFalseMatrix();

        long cost = 0;
        for (int i = 0; i < garden.Count; i++)
        {
            for (int j = 0; j < garden[i].Count; j++)
            {
                cost += GetPlotCost(i, j, garden, visited);
            }
        }

        return $"{cost}";
    }

    public string SolvePart2(string input)
    {
        var garden = input.ParseAsMatrix(c => c);
        var visited = garden.ToFalseMatrix();

        long cost = 0;
        for (int i = 0; i < garden.Count; i++)
        {
            for (int j = 0; j < garden[i].Count; j++)
            {
                cost += GetPlotDiscountedCost(i, j, garden, visited);
            }
        }

        return $"{cost}";
    }

    private static long GetPlotCost(int i, int j, List<List<char>> garden, List<List<bool>> visited)
    {
        if (garden.IsOutOfBounds(i, j) || visited[i][j])
            return 0;

        var plantType = garden[i][j];
        (long area, long perimeter) = ExplorePlantType(i, j, plantType, garden, visited);

        return area * perimeter;
    }

    private static (long Area, long Perimeter) ExplorePlantType(
        int i,
        int j,
        char plantType,
        List<List<char>> garden,
        List<List<bool>> visited)
    {
        if (garden.IsOutOfBounds(i, j) || visited[i][j])
            return (0, 0);

        long area = 1;
        long perimeter = 0;
        visited[i][j] = true;

        if (garden.IsOutOfBounds(i - 1, j) || garden[i - 1][j] != plantType)
            perimeter++;
        else
        {
            var (exploredArea, exploredPerimeter) = ExplorePlantType(i - 1, j, plantType, garden, visited);
            area += exploredArea;
            perimeter += exploredPerimeter;
        }

        if (garden.IsOutOfBounds(i + 1, j) || garden[i + 1][j] != plantType)
            perimeter++;
        else
        {
            var (exploredArea, exploredPerimeter) = ExplorePlantType(i + 1, j, plantType, garden, visited);
            area += exploredArea;
            perimeter += exploredPerimeter;
        }

        if (garden.IsOutOfBounds(i, j - 1) || garden[i][j - 1] != plantType)
            perimeter++;
        else
        {
            var (exploredArea, exploredPerimeter) = ExplorePlantType(i, j - 1, plantType, garden, visited);
            area += exploredArea;
            perimeter += exploredPerimeter;
        }

        if (garden.IsOutOfBounds(i, j + 1) || garden[i][j + 1] != plantType)
            perimeter++;
        else
        {
            var (exploredArea, exploredPerimeter) = ExplorePlantType(i, j + 1, plantType, garden, visited);
            area += exploredArea;
            perimeter += exploredPerimeter;
        }

        return (area, perimeter);
    }

    private static long GetPlotDiscountedCost(int i, int j, List<List<char>> garden, List<List<bool>> visited)
    {
        if (garden.IsOutOfBounds(i, j) || visited[i][j])
            return 0;

        var plantType = garden[i][j];
        (long area, long vertices) = ExploreDiscountedPlot(i, j, plantType, garden, visited);

        return area * vertices;
    }

    private static (long Area, long Vertices) ExploreDiscountedPlot(
        int i,
        int j,
        char plantType,
        List<List<char>> garden,
        List<List<bool>> visited)
    {
        if (garden.IsOutOfBounds(i, j) || visited[i][j])
            return (0, 0);

        long area = 1;
        long vertices = CountVertice(i, j, garden);
        visited[i][j] = true;

        if (!garden.IsOutOfBounds(i - 1, j) && garden[i - 1][j] == plantType)
        {
            var (exploredArea, exploredPerimeter) = ExploreDiscountedPlot(i - 1, j, plantType, garden, visited);
            area += exploredArea;
            vertices += exploredPerimeter;
        }
        if (!garden.IsOutOfBounds(i + 1, j) && garden[i + 1][j] == plantType)
        {
            var (exploredArea, exploredPerimeter) = ExploreDiscountedPlot(i + 1, j, plantType, garden, visited);
            area += exploredArea;
            vertices += exploredPerimeter;
        }
        if (!garden.IsOutOfBounds(i, j - 1) && garden[i][j - 1] == plantType)
        {
            var (exploredArea, exploredPerimeter) = ExploreDiscountedPlot(i, j - 1, plantType, garden, visited);
            area += exploredArea;
            vertices += exploredPerimeter;
        }
        if (!garden.IsOutOfBounds(i, j + 1) && garden[i][j + 1] == plantType)
        {
            var (exploredArea, exploredPerimeter) = ExploreDiscountedPlot(i, j + 1, plantType, garden, visited);
            area += exploredArea;
            vertices += exploredPerimeter;
        }

        return (area, vertices);
    }

    private static int CountVertice(int i, int j, List<List<char>> garden)
    {
        var count = 0;
        // is convex corner?
        if (IsDistinct(i, j, i, j + 1, garden) && IsDistinct(i, j, i - 1, j, garden)) count++;
        if (IsDistinct(i, j, i, j + 1, garden) && IsDistinct(i, j, i + 1, j, garden)) count++;
        if (IsDistinct(i, j, i, j - 1, garden) && IsDistinct(i, j, i - 1, j, garden)) count++;
        if (IsDistinct(i, j, i, j - 1, garden) && IsDistinct(i, j, i + 1, j, garden)) count++;

        //is top left concave corner?
        if (!garden.IsOutOfBounds(i - 1, j - 1) && IsDistinct(i, j, i - 1, j - 1, garden)
            && !IsDistinct(i, j, i - 1, j, garden) && !IsDistinct(i, j, i, j - 1, garden))
        {
            count++;
        }

        //is top right concave corner?
        if (!garden.IsOutOfBounds(i - 1, j + 1) && IsDistinct(i, j, i - 1, j + 1, garden)
            && !IsDistinct(i, j, i - 1, j, garden) && !IsDistinct(i, j, i, j + 1, garden))
        {
            count++;
        }

        //is botton left concave corner?
        if (!garden.IsOutOfBounds(i + 1, j - 1) && IsDistinct(i, j, i + 1, j - 1, garden)
            && !IsDistinct(i, j, i + 1, j, garden) && !IsDistinct(i, j, i, j - 1, garden))
        {
            count++;
        }

        //is botton right concave corner?
        if (!garden.IsOutOfBounds(i + 1, j + 1) && IsDistinct(i, j, i + 1, j + 1, garden)
            && !IsDistinct(i, j, i + 1, j, garden) && !IsDistinct(i, j, i, j + 1, garden))
        {
            count++;
        }

        return count;
    }

    private static bool IsDistinct(int i, int j, int u, int w, List<List<char>> garden) =>
        garden.IsOutOfBounds(u, w) || garden[i][j] != garden[u][w];
}
