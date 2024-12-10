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
            else
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
            compressedBlocksRepresentation.AddRange(blocks[currentBlockIndex].SubtractAllAsRange());
            currentBlockIndex++;
            while (currentFreeBlockIndex < freeBlocks.Count
                && freeBlocks[currentFreeBlockIndex].Size > 0
                && currentBlockIndex < lastBlockIndex)
            {
                var lastBlock = blocks[lastBlockIndex];
                var subtracted = freeBlocks[currentFreeBlockIndex].TrySubtract(lastBlock.Size);
                compressedBlocksRepresentation.AddRange(lastBlock.SubtractAsRange(subtracted));
                if (lastBlock.Size == 0) lastBlockIndex--;
            }
            currentFreeBlockIndex++;
        }
        compressedBlocksRepresentation.AddRange(blocks[currentBlockIndex].SubtractAllAsRange());
        var sum = (long)0;
        for (long i = 0; i < compressedBlocksRepresentation.Count; i++)
        {
            var value = compressedBlocksRepresentation[(int)i];
            var partial = i * value;
            sum += partial;
        }

        return sum.ToString();
    }

    public string SolvePart2(string input)
    {
        return "not solved";
    }
}

internal class MemoryBlock(int id, int size)
{
    public int Id { get; } = id;
    public int Size { get; set; } = size;

    public IEnumerable<int> SubtractAllAsRange()
    {
        var subtracted = Size;
        Size = 0;
        return Enumerable.Repeat(Id, subtracted);
    }

    public IEnumerable<int> SubtractAsRange(int subtracted)
    {
        Size -= subtracted;
        return Enumerable.Repeat(Id, subtracted);
    }

    public int TrySubtract(int quantity)
    {
        var subtracted = Math.Min(Size, quantity);
        Size -= subtracted;

        return subtracted;
    }
}
