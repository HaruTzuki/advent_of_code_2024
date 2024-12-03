using var reader = new StreamReader("input.txt");
var str = reader.ReadToEnd();

var reports = str.Split(["\n", "\r"], StringSplitOptions.RemoveEmptyEntries);

// Key: Report
// Value : Is Safe or not
var d = new Dictionary<int, bool>();


for (var i = 0; i < reports.Length; i++)
{
    var levels = reports[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
    var diff = 0;
    var isIncremental = -1;

    for (var j = 0; j < levels.Length - 1; j++)
    {
        // Converting str nums to interger
        var num1 = int.Parse(levels[j]);
        var num2 = int.Parse(levels[j + 1]);

        // Checking if the sequence is incremental or decremental
        if (num1 < num2 && isIncremental == -1)
        {
            isIncremental = 1;
        }
        else if (num1 > num2 && isIncremental == -1)
        {
            isIncremental = 0;
        }

        if (num1 > num2 && isIncremental == 1)
        {
            d.Add(i, false);
            goto LabelForBreak;
        }
        else if (num1 < num2 && isIncremental == 0)
        {
            d.Add(i, false);
            goto LabelForBreak;
        }


        diff = Math.Abs(int.Parse(levels[j]) - int.Parse(levels[j + 1]));

        if (diff < 1 || diff > 3)
        {
            d.Add(i, false);
            goto LabelForBreak;
        }
    }

    d.Add(i, true);

    LabelForBreak:
    diff = 0;
}


Console.WriteLine(
    $"The number of safe reports are {d.Count(x => x.Value)} while the unsafe are {d.Count(x => !x.Value)}");

var ud = new Dictionary<int, bool>();

foreach (var unsafeReport in d.Where(x => !x.Value))
{
    var levels = reports[unsafeReport.Key].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    var isSafe = false;

    for (int i = 0; i < levels.Count; i++)
    {
        // Save to temp list
        var tempLevels = new List<int>(levels);
        tempLevels.RemoveAt(i); // Removes in every iterate the current element
        var isIncremental = tempLevels[0] < tempLevels[1] ? 1 : 0;
        var isValid = true;

        for (var j = 0; j < tempLevels.Count - 1; j++)
        {
            var diff = Math.Abs(tempLevels[j] - tempLevels[j + 1]);

            if (diff < 1 || diff > 3 || (isIncremental == 1 && tempLevels[j] > tempLevels[j + 1]) || (isIncremental == 0 && tempLevels[j] < tempLevels[j + 1]))
            {
                isValid = false;
                break;
            }
        }

        if (isValid)
        {
            isSafe = true;
            break;
        }
    }

    ud.Add(unsafeReport.Key, isSafe);
}

Console.WriteLine(
    $"The number of safe reports are {ud.Count(x => x.Value)} out of {ud.Count} reports");
    
    Console.WriteLine($"So the total safe reports are: {d.Count(x => x.Value) + ud.Count(x => x.Value)}");