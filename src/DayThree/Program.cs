using System.Text.RegularExpressions;

using var input = new StreamReader("input.txt");
var str = input.ReadToEnd();

var regEx = new Regex(@"mul\(\d{1,3},\d{1,3}\)");


var matches = regEx.Matches(str);

Console.WriteLine($"The result is: {matches.Count}");
var mul = new List<int>();

for(var i = 0; i < matches.Count; i++)
{
    var match = matches[i].Value;
    var numbers = match.Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
    var a = int.Parse(numbers[1]);
    var b = int.Parse(numbers[2]);
    mul.Add(a * b);
}


Console.WriteLine($"The result is: {mul.Sum()}");

// Part 2

var regEx2 = new Regex(@"mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\)");

var matches2 = regEx2.Matches(str);
Console.WriteLine($"The result is: {matches2.Count}");

var mul2 = new List<int>();
var doMul = true;
for (var i = 0; i < matches2.Count; i++)
{
    var match  = matches2[i].Value;
    
    if (match.Contains("don't"))
    {
        doMul = false;
        continue;
    }

    if (match.Contains("do"))
    {
        doMul = true;
        continue;
    }
    
    if (!doMul)
    {
        continue;
    }
    
    var numbers = match.Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
    var a = int.Parse(numbers[1]);
    var b = int.Parse(numbers[2]);
    mul2.Add(a * b);
}


Console.WriteLine($"The result is: {mul2.Sum()}");