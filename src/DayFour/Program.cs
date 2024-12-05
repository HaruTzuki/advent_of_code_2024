using AdventOfCode.Shared; // Import the namespace containing the FileReader class

var input = FileReader.ReadAllLines("input.txt"); // Read all lines from the input file into the input array
int rows = input.Length;
int cols = input[0].Length;
var terrain = new char[rows, cols]; // Initialize a 2D char array to store the grid of characters

// Populate the terrain array with characters from the input array
for (var i = 0; i < input.Length; i++)
{
    for (var j = 0; j < input[i].Length; j++)
    {
        terrain[i, j] = input[i][j]; // Assign each character to the corresponding position in the terrain array
    }
}

string word = "XMAS";
int wordLength = word.Length;
int count = 0;

// Directions: Right, Left, Down, Up, Down-Right, Down-Left, Up-Right, Up-Left
int[] dx = { 0, 0, 1, -1, 1, 1, -1, -1 };
int[] dy = { 1, -1, 0, 0, 1, -1, 1, -1 };

for (int r = 0; r < rows; r++)
{
    for (int c = 0; c < cols; c++)
    {
        for (int d = 0; d < 8; d++)
        {
            int k;
            for (k = 0; k < wordLength; k++)
            {
                int nr = r + k * dx[d];
                int nc = c + k * dy[d];

                if (nr < 0 || nc < 0 || nr >= rows || nc >= cols || terrain[nr, nc] != word[k])
                {
                    break;
                }
            }
            if (k == wordLength)
            {
                count++;
            }
        }
    }
}

Console.WriteLine($"The word 'XMAS' appears {count} times.");


// Part Two

int newCount = 0; // Initialize a counter to keep track of the number of occurrences of the pattern "X-MAS"

// Define the possible sequences for "MAS" and "SAM"
char[][] masSequences = new char[][]
{
    new char[] { 'M', 'A', 'S' },
    new char[] { 'S', 'A', 'M' }
};

// Iterate through each cell in the terrain array, excluding the borders
for (var i = 1; i < terrain.GetLength(0) - 1; i++)
{
    for (var j = 1; j < terrain.GetLength(1) - 1; j++)
    {
        if (terrain[i, j] == 'A') // Check if the current cell contains the character 'A'
        {
            // Check all combinations of diagonals
            foreach (var diag1 in masSequences)
            {
                foreach (var diag2 in masSequences)
                {
                    // Positions for the diagonals
                    int topLeftX = i - 1;
                    int topLeftY = j - 1;
                    int bottomRightX = i + 1;
                    int bottomRightY = j + 1;

                    int topRightX = i - 1;
                    int topRightY = j + 1;
                    int bottomLeftX = i + 1;
                    int bottomLeftY = j - 1;

                    // Check if positions are within bounds
                    if (topLeftX >= 0 && topLeftY >= 0 && bottomRightX < terrain.GetLength(0) && bottomRightY < terrain.GetLength(1) &&
                        topRightX >= 0 && topRightY < terrain.GetLength(1) && bottomLeftX < terrain.GetLength(0) && bottomLeftY >= 0)
                    {
                        // Check first diagonal (top-left to bottom-right)
                        bool diag1Matches = (terrain[topLeftX, topLeftY] == diag1[0] &&
                                             terrain[i, j] == diag1[1] &&
                                             terrain[bottomRightX, bottomRightY] == diag1[2]);

                        // Check second diagonal (top-right to bottom-left)
                        bool diag2Matches = (terrain[topRightX, topRightY] == diag2[0] &&
                                             terrain[i, j] == diag2[1] &&
                                             terrain[bottomLeftX, bottomLeftY] == diag2[2]);

                        if (diag1Matches && diag2Matches)
                        {
                            newCount++;
                        }
                    }
                }
            }
        }
    }
}

Console.WriteLine($"The pattern X-MAS appears {newCount} times."); // Print the total count of occurrences of the pattern "X-MAS"