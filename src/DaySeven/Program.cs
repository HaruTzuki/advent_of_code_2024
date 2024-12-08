using AdventOfCode.Shared;
using System.Runtime.CompilerServices;

var rows = FileReader.ReadAllLines("input.txt");
var trueEquations = new List<long>();


// Loop through each row
for(var i = 0; i < rows.Length; i++)
{
    // Split the row by the colon
    // Position 0 is the value
    // Position 1 are the numbers
    var columns = rows[i].Split(":", StringSplitOptions.RemoveEmptyEntries);
    var value = long.Parse(columns[0].Trim());
    var numbers = columns[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x=>long.Parse(x.Trim())).ToArray();

    // Check if the equation is true
    if (IsTrueEquation(value, numbers))
    {
        trueEquations.Add(value);
    }
}

Console.WriteLine($"The total of equations are: {trueEquations.Sum()}");

// Must we see if the combination of numbers using the operators + or * or both is equal to the value
// If it is, add it to the list of true equations
// If it is not, add it to the list of false equations
bool IsTrueEquation(long value, long[] numbers)
{
    return CheckEquation(value, numbers, 0, numbers[0]);
}

bool CheckEquation(long value, long[] numbers, long index, long currentResult)
{
    if (index == numbers.Length - 1)
    {
        return currentResult == value;
    }

    // Try addition
    if (CheckEquation(value, numbers, index + 1, currentResult + numbers[index + 1]))
    {
        return true;
    }

    // Try multiplication
    if (CheckEquation(value, numbers, index + 1, currentResult * numbers[index + 1]))
    {
        return true;
    }

    // Try concatenation
    if (CheckEquation(value, numbers, index + 1, long.Parse(currentResult.ToString() + numbers[index + 1].ToString())))
    {
        return true;
    }

    return false;
}