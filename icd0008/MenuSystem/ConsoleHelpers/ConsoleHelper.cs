namespace MenuSystem.ConsoleHelpers;

public static class ConsoleHelper
{
    public static int MultipleChoice(bool canCancel, params string[] options)
    {
        const int startX = 2;
        const int startY = 0;
        const int optionsPerLine = 1;
        const int spacingPerLine = 1;

        int currentSelection = 0;

        ConsoleKey key;

        Console.CursorVisible = false;

        do
        {
            Console.Clear();

            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(startX + (i % optionsPerLine) * spacingPerLine, startY + i / optionsPerLine);

                if(i == currentSelection)
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                Console.Write(options[i]);

                Console.ResetColor();
            }

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                {
                    if (currentSelection >= optionsPerLine)
                        currentSelection -= optionsPerLine;
                    break;
                }
                case ConsoleKey.DownArrow:
                {
                    if (currentSelection + optionsPerLine < options.Length)
                        currentSelection += optionsPerLine;
                    break;
                }
                case ConsoleKey.Escape:
                {
                    if (canCancel)
                        return -1;
                    break;
                }
            }
        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;

        return currentSelection;
    }
    
}