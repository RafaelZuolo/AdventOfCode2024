using AdventOfCode2024;
using AdventOfCode2024.Days;

IReadOnlyDictionary<int, IDay> DayByNumber = new Dictionary<int, IDay>
{
    { 01, new Day01() },
    { 02, new Day02() },
    { 03, new Day03() },
    { 04, new Day04() },
    { 05, new Day05() },
    { 06, new Day06() },
    { 07, new Day07() },
    { 08, new Day08() },
    { 09, new Day09() },
    { 10, new Day10() },
    { 11, new Day11() },
    { 12, new Day12() },
    { 13, new Day13() },
    { 14, new Day14() },
    { 15, new Day15() },
    { 16, new Day16() },
    { 17, new Day17() },
    { 18, new Day18() },
    { 19, new Day19() },
    { 20, new Day20() },
    { 21, new Day21() },
    { 22, new Day22() },
    { 23, new Day23() },
    { 24, new Day24() },
    { 25, new Day25() },
};
const int DebbugDay = 9;
var isTest = args.Contains("-t");
//isTest = true;
var projectPath = Directory.GetCurrentDirectory();
var currentDay = args.Contains("-d") ? int.Parse(args[Array.IndexOf(args, "-d") + 1]) : DebbugDay;
var onlyPart1 = args.Contains("-p1");
var onlyPart2 = !onlyPart1 && args.Contains("-p2");
Console.WriteLine();

var message = isTest ? $"Testing day {currentDay:D2}" : $"Solving day {currentDay:D2}";
Console.WriteLine(message + Environment.NewLine);

var folder = isTest ? "Tests" : "Inputs";
var path = Path.Combine(projectPath, folder, currentDay.ToString("D2") + ".txt");
var input = File.ReadAllText(path);

var problemDay = DayByNumber[currentDay];
if (!onlyPart2) Console.WriteLine($"Part 1: {problemDay.SolvePart1(input)}");
if (!onlyPart1) Console.WriteLine($"Part 2: {problemDay.SolvePart2(input)}");
