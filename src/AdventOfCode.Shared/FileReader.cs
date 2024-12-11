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

    public static T[,] ReadGrid<T>(string path, Func<char, T> converter)
    {
        var lines = ReadAllLines(path);
        var grid = new T[lines.Length, lines[0].Length];
        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                grid[i, j] = converter(lines[i][j]);
            }
        }
        return grid;
    }
}
