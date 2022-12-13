using System.Collections;

namespace trial;

    public class HelloWorld
{
    public void Main()
    {
        // some basic actions

        
        bool boolean = true;
        ArrayList nums = new ArrayList();
        if (boolean && nums.Count == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(i);
                nums.Add(i);
            }
        }
        
        foreach (int num in nums)
        { Console.WriteLine("1. " + Convert.ToString(num)); }
        
        Another();
    }

    private void Another()
    {
        Console.WriteLine("========");
        // Lists and lambdas
        List<int> numbers = new List<int>()
        {
            2, 3, 4, 5, 10,
            11, 12, 20, 100
        };

        var squares = numbers.Select(num => num * num);
        foreach (int square in squares)
        { Console.WriteLine(square); }

        List<int> evens = numbers.FindAll(num => (num % 2) == 0);
        foreach (int evenNum in evens)
        { Console.WriteLine(evenNum); }
        
        // Sort equivalent: list.OrderBy(obj => obj.name)
    }
}
