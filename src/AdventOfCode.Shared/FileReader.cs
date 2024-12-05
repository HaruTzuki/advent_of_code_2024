namespace AdventOfCode.Shared;

public static class FileReader
{
    public static string[] ReadAllLines(string path)
    {
        return File.ReadAllLines(path);
    }

    public static string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }
}