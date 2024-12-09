namespace AdventOfCode2024.Days;

public class Day09 : IDay
{
    public string SolvePart1(string input)
    {
        input = input.Trim();
        var blocks = new List<MemoryBlock>();
        var freeBlocks = new List<MemoryBlock>();
        var test = (long)0;
        for (int i = 0; i < input.Length; i++)
        {
            var currentChar = input[i];
            if (i % 2 == 0 && int.Parse(currentChar.ToString()) != 0)
            {
                blocks.Add(new MemoryBlock(i / 2, int.Parse(currentChar.ToString())));
                test += int.Parse(currentChar.ToString());
            }
            else if (int.Parse(currentChar.ToString()) != 0)
            {
                freeBlocks.Add(new MemoryBlock(-1, int.Parse(currentChar.ToString())));
            }
        }

        var currentBlockIndex = 0;
        var currentFreeBlockIndex = 0;
        var lastBlockIndex = blocks.Count - 1;
        var compressedBlocksRepresentation = new List<int>();
        while (currentBlockIndex < lastBlockIndex)
        {
            compressedBlocksRepresentation.AddRange(blocks[currentBlockIndex].AsRange());
            currentBlockIndex++;
            while (currentFreeBlockIndex < freeBlocks.Count && freeBlocks[currentFreeBlockIndex].Size > 0)
            {
                var lastBlock = blocks[lastBlockIndex];
                var subtracted = freeBlocks[currentFreeBlockIndex].TrySubtract(lastBlock.Size);

                lastBlock.TrySubtract(subtracted);
                if (lastBlock.Size == 0) lastBlockIndex--;

                compressedBlocksRepresentation.AddRange(lastBlock.AsRange(subtracted));
            }
            currentFreeBlockIndex++;
        }
        compressedBlocksRepresentation.AddRange(blocks[currentBlockIndex].AsRange());

        var sum = (long)0;
        for (int i = 0; i < compressedBlocksRepresentation.Count; i++)
        {
            sum += i * compressedBlocksRepresentation[i];
        }
        //Console.WriteLine(string.Join(", ", compressedBlocksRepresentation));
        return sum.ToString();
    }

    public string SolvePart2(string input)
    {
        return "not solved";
    }
}

internal class MemoryBlock(int id, int size)
{
    public bool IsFree => Id >= 0;
    public int Id { get; } = id;
    public int Size { get; set; } = size;

    public IEnumerable<int> AsRange()
    {
        return Enumerable.Repeat(Id, Size);
    }

    public IEnumerable<int> AsRange(int subtracted)
    {
        return Enumerable.Repeat(Id, subtracted);
    }

    public int TrySubtract(int quantity)
    {
        var subtracted = Math.Min(Size, quantity);
        Size -= subtracted;

        return subtracted;
    }
}
