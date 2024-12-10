namespace AdventOfCode2024.Days;

public class Day09 : IDay
{
    public string SolvePart1(string input)
    {
        input = input.Trim();
        var blocks = new List<MemoryBlock>();
        var freeBlocks = new List<MemoryBlock>();
        for (int i = 0; i < input.Length; i++)
        {
            var currentChar = input[i];
            if (i % 2 == 0 && int.Parse(currentChar.ToString()) != 0)
            {
                blocks.Add(new MemoryBlock(i / 2, int.Parse(currentChar.ToString())));
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
        input = input.Trim();
        var blocks = new List<Part2MemoryBlock>();
        for (int i = 0; i < input.Length; i++)
        {
            var currentChar = input[i];
            if (i % 2 == 0)
            {
                blocks.Add(new Part2MemoryBlock(i / 2, int.Parse(currentChar.ToString())));
            }
            else
            {
                blocks.Add(new Part2MemoryBlock(-1, int.Parse(currentChar.ToString())));
            }
        }

        var currentBlockIndex = blocks.Count - 1;

        if (currentBlockIndex % 2 != 0) throw new Exception("Wrong last block index");

        while (currentBlockIndex > 0)
        {
            for (int i = 0; i < currentBlockIndex; i++)
            {
                var currentBlock = blocks[currentBlockIndex];
                var freeSpaceBlock = blocks[i];
                if (freeSpaceBlock.CanAddRange(currentBlock.Memory))
                {
                    freeSpaceBlock.AddRange(currentBlock.Memory);
                    currentBlock.Clear();
                    break;
                }
            }
            currentBlockIndex -= 2;
        }
        blocks.ForEach(b => b.FillWithZeros());

        var compressedBlocksRepresentation = blocks.SelectMany(b => b.Memory).ToArray();

        var sum = (long)0;
        for (long i = 0; i < compressedBlocksRepresentation.Length; i++)
        {
            sum += i * compressedBlocksRepresentation[(int)i];
        }

        return sum.ToString();
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

internal class Part2MemoryBlock(int id, int capacity)
{
    public int Id { get; } = id;
    public int Capacity { get; } = capacity;
    public List<int> Memory { get; } = id >= 0 ? Enumerable.Repeat(id, capacity).ToList() : [];
    public bool IsFreeSpace => Id < 0;
    public bool HasFreeSpace => IsFreeSpace && Memory.Count < Capacity;

    public bool CanAddRange(IEnumerable<int> range)
    {
        return IsFreeSpace && Capacity - Memory.Count >= range.Count();
    }

    public void AddRange(IEnumerable<int> range)
    {
        if (!CanAddRange(range)) throw new Exception($"Cannot add range to memory with id [{Id}]");

        Memory.AddRange(range);
    }

    public void Clear()
    {
        Memory.Clear();
        Memory.AddRange(Enumerable.Repeat(0, Capacity));
    }

    public void FillWithZeros()
    {
        if (!HasFreeSpace) return;

        Memory.AddRange(Enumerable.Repeat(0, Capacity - Memory.Count));
    }
}
