using AdventOfCode.Shared;

var text = FileReader.ReadAllText("input.txt");

// 0 : Rules
// 1 : Page Numbers Ordering
var split = text.Split("\r\n\r\n");
var orderRule = new List<OrderRule>();
var pageNumbers = split[1].Split("\r\n", StringSplitOptions.RemoveEmptyEntries);


foreach (var rules in split[0].Split(["\r\n"], StringSplitOptions.RemoveEmptyEntries))
{
    var rule = rules.Split("|");
    orderRule.Add(new OrderRule
    {
        Former = int.Parse(rule[0]),
        Latter = int.Parse(rule[1])
    });
}

var middleCorrectSequence = new List<int>();
var middleIncorrectSequence = new List<int>();
var incorrectList = new List<List<int>>();

bool isIncorrect;
foreach (var pageNumber in pageNumbers)
{
    isIncorrect = false;
    var list = pageNumber.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

    foreach (var rule in orderRule)
    {
        var index_former = list.IndexOf(rule.Former);
        var index_latter = list.IndexOf(rule.Latter);

        if (index_former == -1 || index_latter == -1)
        {
            continue;
        }

        // Breaks the rules.
        if (index_former > index_latter)
        {
            isIncorrect = true;
            break;
        }
    }

    //Find the middle number 
    if (!isIncorrect)
    {
        var middle = list.Count / 2;
        middleCorrectSequence.Add(list[middle]);
    }
    else
    {
        incorrectList.Add(list);
    }
}


/* Part 2 */

foreach (var incorrect in incorrectList)
{
    var tempList = new List<int>(incorrect);
    bool changed;

    do
    {
        changed = false;
        foreach (var rule in orderRule)
        {
            var index_former = tempList.IndexOf(rule.Former);
            var index_latter = tempList.IndexOf(rule.Latter);

            if (index_former == -1 || index_latter == -1)
            {
                continue;
            }
            
            if (index_former > index_latter)
            {
                var temp = tempList[index_former];
                tempList[index_former] = tempList[index_latter];
                tempList[index_latter] = temp;
                changed = true;
            }
        }
    } while (changed);
    
    middleIncorrectSequence.Add(tempList[tempList.Count / 2]);
}

Console.WriteLine(middleCorrectSequence.Sum());
Console.WriteLine(middleIncorrectSequence.Sum());


class OrderRule
{
    public int Former { get; set; }
    public int Latter { get; set; }
}