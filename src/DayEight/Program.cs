using AdventOfCode.Shared;
using System.Collections.Generic;

// Read the grid from the input file
var grid = FileReader.ReadGrid<char>("test_input.txt", c => c);

var antennas = new Dictionary<char, List<(int row, int col)>>();

int numRows = grid.GetLength(0);
int numCols = grid.GetLength(1);

// Identify antennas and their positions
for (var i = 0; i < numRows; i++) // rows
{
    for (var j = 0; j < numCols; j++) // columns
    {
        char c = grid[i, j];
        if (c != '.')
        {
            if (!antennas.ContainsKey(c))
            {
                antennas[c] = new List<(int row, int col)>();
            }
            antennas[c].Add((i, j));
        }
    }
}

var antinodes = new HashSet<(int row, int col)>();

// For each frequency, process the antenna pairs
foreach (var freq in antennas.Keys)
{
    var positions = antennas[freq];
    for (int i = 0; i < positions.Count; i++)
    {
        var (row1, col1) = positions[i];

        for (int j = i + 1; j < positions.Count; j++)
        {
            var (row2, col2) = positions[j];

            // Calculate antinode positions
            int antinodeRow1 = 2 * row1 - row2;
            int antinodeCol1 = 2 * col1 - col2;

            int antinodeRow2 = 2 * row2 - row1;
            int antinodeCol2 = 2 * col2 - col1;

            var antinode1 = (antinodeRow1, antinodeCol1);
            var antinode2 = (antinodeRow2, antinodeCol2);

            // Check bounds and add to set
            if (IsWithinBounds(antinode1, numRows, numCols))
            {
                antinodes.Add(antinode1);
            }
            if (IsWithinBounds(antinode2, numRows, numCols))
            {
                antinodes.Add(antinode2);
            }
        }
    }
}

// Include antennas themselves if they overlap with antinode positions
// (Antennas can be at antinode positions)
foreach (var freq in antennas.Keys)
{
    foreach (var pos in antennas[freq])
    {
        antinodes.Add(pos);
    }
}

Console.WriteLine($"Total unique antinodes: {antinodes.Count}");

// Helper method to check if the position is within the grid bounds
static bool IsWithinBounds((int row, int col) position, int numRows, int numCols)
{
    return position.row >= 0 && position.row < numRows && position.col >= 0 && position.col < numCols;
}
