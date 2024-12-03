using System.Threading.Channels;

using var reader = new StreamReader("input.txt");

var str = reader.ReadToEnd();

var list_one = new List<int>();
var list_two = new List<int>();

foreach (var line in str.Split("\n", StringSplitOptions.RemoveEmptyEntries))
{
    if (line.Length == 0)
    {
        continue;
    }
    
    var numbers = line.Split(new []{" ", "\r"}, StringSplitOptions.RemoveEmptyEntries);
    list_one.Add(int.Parse(numbers[0]));
    list_two.Add(int.Parse(numbers[1]));
    
}


list_one.Sort();
list_two.Sort();

var d = new Dictionary<int, int>();

for (var i = 0; i < list_one.Count; i++)
{
    d.Add(i, Math.Abs(list_one[i] - list_two[i]));
}

Console.WriteLine($"The total distance is {d.Values.Sum()}");

var totals = new List<int>();

for(var i = 0; i < list_one.Count; i++)
{
    var totalTimes = 0;
    for (var j = 0; j < list_two.Count; j++)
    {
        if (list_one[i] == list_two[j])
        {
            totalTimes++;
        }
    }
    
    totals.Add(list_one[i] * totalTimes);
}



Console.WriteLine($"The total of the totals is {totals.Sum()}");





Console.WriteLine($"The cound of the first list is: {list_one.Count} and of the second is: {list_two.Count}");

