using AdventOfCode.Shared;
using DaySix;

const char Empty = '.';
const char Furniture = '#';
const char DiscoveredPath = 'X';
const char Obstacle = 'O';

var rows = FileReader.ReadAllLines("text_input.txt");

// Create the terrain
var rowsCount = rows.Length;
var columnsCount = rows[0].Length;
var terrain = new char[rowsCount, columnsCount];
var guardPosition = new Location();
var guardStartPosition = Guard.Up;




// Creating terrain and find guard position
for (var i = 0; i < rowsCount; i++)
{
    for (var j = 0; j < columnsCount; j++)
    {
        terrain[i, j] = rows[i][j];
        if (terrain[i, j] == (char)Guard.Up || terrain[i, j] == (char)Guard.Down || terrain[i, j] == (char)Guard.Left || terrain[i, j] == (char)Guard.Right)
        {
            guardPosition.X = j;
            guardPosition.Y = i;
            guardStartPosition = (Guard)terrain[i, j];
        }
    }
}

Console.WriteLine($"The guard's position is at location X:{guardPosition.X} Y:{guardPosition.Y}");

// Part 1 
var partOneTerrain = (char[,])terrain.Clone();
int xCount = SimulateGuardPath(partOneTerrain, guardPosition);
Console.WriteLine($"The Guard does {xCount} moves");

// Part 2

int loopCount = 0;
var partTwoTerrain = (char[,])terrain.Clone(); // Preserve original terrain for Part Two

// Iterate over each possible position to place the obstruction
for (var i = 0; i < rowsCount; i++)
{
    for (var j = 0; j < columnsCount; j++)
    {
        // Skip the guard's starting position and existing obstructions
        if (partTwoTerrain[i, j] != Empty || (i == guardPosition.Y && j == guardPosition.X))
        {
            continue;
        }

        // Create a fresh copy of the terrain for this simulation
        var terrainCopy = (char[,])partTwoTerrain.Clone();

        // Place the obstruction temporarily
        terrainCopy[i, j] = Obstacle;

        if (await StuckInLoopAsync(terrainCopy, guardPosition, guardStartPosition))
        {

            loopCount++;
        }
    }
}

Console.WriteLine($"Number of positions where the guard gets stuck in a loop: {loopCount}");

bool HasCollision(char cell)
{
    return cell == Furniture || cell == Obstacle;
}

bool IsInBounds(int x, int y)
{
    return x >= 0 && x < columnsCount && y >= 0 && y < rowsCount;
}

async Task<bool> StuckInLoopAsync(char[,] terrain, Location startPosition, Guard startDirection)
{
    var guardPosition = new Location { X = startPosition.X, Y = startPosition.Y };
    var facing = startDirection;
    var visitedStates = new HashSet<string>();

    while (true)
    {

        var state = $"{guardPosition.X},{guardPosition.Y},{facing}";
        if (visitedStates.Contains(state))
        {
            // Guard is stuck in a loop
            return true;
        }
        visitedStates.Add(state);

        int nextX = guardPosition.X;
        int nextY = guardPosition.Y;
        GetForwardPosition(facing, ref nextX, ref nextY);

        if (!IsInBounds(nextX, nextY))
        {
            // Guard leaves the mapped area
            return false;
        }

        char nextCell = terrain[nextY, nextX];

        if (HasCollision(nextCell))
        {
            // Turn right 90 degrees
            facing = TurnRight(facing);
            continue;
        }
        else
        {
            // Move forward
            guardPosition.X = nextX;
            guardPosition.Y = nextY;
        }
        printTerrain(terrain);
        await Task.Delay(20);
    }
}

void printTerrain(char[,] terrainMap)
{
    Console.Clear();

    for (var i = 0; i < rowsCount; i++)
    {
        for (var j = 0; j < columnsCount; j++)
        {
            Console.Write(terrainMap[i, j]);
        }
        Console.WriteLine();
    }
}
void GetForwardPosition(Guard facing, ref int x, ref int y)
{
    switch (facing)
    {
        case Guard.Up:
            y -= 1;
            break;
        case Guard.Down:
            y += 1;
            break;
        case Guard.Left:
            x -= 1;
            break;
        case Guard.Right:
            x += 1;
            break;
    }
}

Guard TurnRight(Guard facing)
{
    return facing switch
    {
        Guard.Up => Guard.Right,
        Guard.Right => Guard.Down,
        Guard.Down => Guard.Left,
        Guard.Left => Guard.Up,
        _ => throw new InvalidOperationException("Invalid facing direction."),
    };
}

int SimulateGuardPath(char[,] partOneTerrain, Location guardStartPosition)
{

    var guardPosition = new Location { X = guardStartPosition.X, Y = guardStartPosition.Y };

    bool isExit = false;
    while (!isExit)
    {
        var currentGuardPosition = (Guard)partOneTerrain[guardPosition.Y, guardPosition.X];
        char tempPosition;

        switch (currentGuardPosition)
        {
            case Guard.Up:
                // Must move forward
                if (IsInBounds(guardPosition.X, guardPosition.Y - 1))
                {
                    tempPosition = partOneTerrain[guardPosition.Y - 1, guardPosition.X]; //Move up
                }
                else
                {
                    partOneTerrain[guardPosition.Y, guardPosition.X] = DiscoveredPath;
                    isExit = true;
                    break;
                }

                if (HasCollision(tempPosition))
                {
                    partOneTerrain[guardPosition.Y, guardPosition.X] = '>';
                    continue;
                }

                partOneTerrain[guardPosition.Y, guardPosition.X] = DiscoveredPath;
                guardPosition.Y -= 1;
                guardPosition.X += 0;
                partOneTerrain[guardPosition.Y, guardPosition.X] = (char)Guard.Up;
                break;
            case Guard.Down:
                // Must move down
                if (IsInBounds(guardPosition.X, guardPosition.Y + 1))
                {
                    tempPosition = partOneTerrain[guardPosition.Y + 1, guardPosition.X]; //Move down
                }
                else
                {
                    partOneTerrain[guardPosition.Y, guardPosition.X] = DiscoveredPath;
                    isExit = true;
                    break;
                }

                if (HasCollision(tempPosition))
                {
                    partOneTerrain[guardPosition.Y, guardPosition.X] = '<';
                    continue;
                }

                partOneTerrain[guardPosition.Y, guardPosition.X] = DiscoveredPath;
                guardPosition.Y += 1;
                guardPosition.X += 0;
                partOneTerrain[guardPosition.Y, guardPosition.X] = (char)Guard.Down;
                break;
            case Guard.Left:
                // Must move left
                if (IsInBounds(guardPosition.X - 1, guardPosition.Y))
                {
                    tempPosition = partOneTerrain[guardPosition.Y, guardPosition.X - 1]; //Move left
                }
                else
                {
                    partOneTerrain[guardPosition.Y, guardPosition.X] = DiscoveredPath;
                    isExit = true;
                    break;
                }

                if (HasCollision(tempPosition))
                {
                    partOneTerrain[guardPosition.Y, guardPosition.X] = '^';
                    continue;
                }

                partOneTerrain[guardPosition.Y, guardPosition.X] = DiscoveredPath;
                guardPosition.Y += 0;
                guardPosition.X -= 1;
                partOneTerrain[guardPosition.Y, guardPosition.X] = (char)Guard.Left;
                break;
            case Guard.Right:
                // Must move righ
                if (IsInBounds(guardPosition.X + 1, guardPosition.Y))
                {
                    tempPosition = partOneTerrain[guardPosition.Y, guardPosition.X + 1]; //Move Right
                }
                else
                {
                    partOneTerrain[guardPosition.Y, guardPosition.X] = DiscoveredPath;
                    isExit = true;
                    break;
                }

                if (HasCollision(tempPosition))
                {
                    partOneTerrain[guardPosition.Y, guardPosition.X] = 'v';
                    continue;
                }

                partOneTerrain[guardPosition.Y, guardPosition.X] = DiscoveredPath;
                guardPosition.Y += 0;
                guardPosition.X += 1;
                partOneTerrain[guardPosition.Y, guardPosition.X] = (char)Guard.Right;
                break;
            default:
                isExit = true;
                break;
        }

        //printTerrain(partOneTerrain);
    }

    var xCount = 0;

    for (var i = 0; i < rowsCount; i++)
    {
        for (var j = 0; j < columnsCount; j++)
        {
            if (partOneTerrain[i, j] == 'X')
            {
                xCount++;
            }
        }
    }

    return xCount;
}

public enum Guard
{
    Up = '^',
    Down = 'v',
    Left = '<',
    Right = '>'
}