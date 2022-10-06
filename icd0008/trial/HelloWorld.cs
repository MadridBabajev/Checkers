using System.Collections;

namespace trial;

    public class HelloWorld
{
    private const byte SomeConst = 1;
    public void Main()
    {
        // some basic actions
        float flt = 1.1f;
        double rounded = Math.Round(flt); // Comment here
        char chr = (char) rounded;
        // int.Parse(someStr)
        // Convert.ToInt32(someStr)
        Console.WriteLine("Hi From hw class, chr: " + chr);
        Console.WriteLine(SomeConst);
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
