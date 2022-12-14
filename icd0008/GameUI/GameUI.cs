using System.Drawing;
using System.Globalization;
using GameOptions;
using GameParts;

namespace GameUI;

// ReSharper disable once InconsistentNaming
public static class GameUI
{
    private static readonly List<CheckersPiece> CheckersPieces = new();
    // private static bool _spacesAfterNum;
    public static SavedDataFromUi BuildInitialBoard(Options? options)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        List<string> widthSpecifiers = DrawUpperRow(options?.BoardWidth, options?.BoardHeight);
        List<string> heightSpecifiers = new();
        bool currentWhite = true;
        // _spacesAfterNum = options?.BoardHeight / 10 >= 1;
        short lastI = 0;
        for (short i = 0; i < options?.BoardHeight; i++)
        {
            if (i != lastI) currentWhite = !currentWhite;
            WriteVerticalNum(options, i);
            heightSpecifiers.Add((options.BoardHeight - i).ToString());
            for (short j = 0; j < options.BoardWidth; j++)
            {
                if (currentWhite)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("   ");
                }
                else
                {
                    HandleBlackTiles(i, j, options);
                }
                currentWhite = !currentWhite;
            }
        }
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n");
        return new SavedDataFromUi(CheckersPieces, widthSpecifiers!, heightSpecifiers!);
    }

    private static void WriteVerticalNum(Options options, short i)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"\n {options.BoardHeight - i}" +
                      // $"{((options.BoardHeight - i) / 10 >= 1 ? " " : "  ")}" +
                      $"{(i < 17 ? "  " : "   ")}");
    }
    private static void HandleBlackTiles(short y, short x, Options? options)
    {
        Console.BackgroundColor = GetNeededColor("#5F6A6A");
        if (y <= 2)
        {
            // Within black range
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" ⚈ ");
            CheckersPieces.Add(new CheckersPiece(EPieceColor.Black,
                y,
                x,
                false
            ));
        } else if (y >= options?.BoardHeight - 3)
        {
            // Within white range
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ⚈ ");
            CheckersPieces.Add(new CheckersPiece(EPieceColor.White,
                y,
                x,
                false
            ));
        }
        else
        {
            Console.Write("   ");
        }
    }

    private static ConsoleColor GetNeededColor(string hexString)
    {
        int argb = Int32.Parse(hexString.Replace("#", ""), NumberStyles.HexNumber);
        Color c = Color.FromArgb(argb);
        
        int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0; // Bright bit
        index |= (c.R > 64) ? 4 : 0; // Red bit
        index |= (c.G > 64) ? 2 : 0; // Green bit
        index |= (c.B > 64) ? 1 : 0; // Blue bit
        
        return (ConsoleColor)index;
    }

    private static List<string> DrawUpperRow(short? boardWidth, short? boardHeight)
    {
        List<string> widthSpecifiers = new();
        Console.Write($"   {(boardHeight > 10 ? "  " : " ")}");
        for (short i = 0; i < boardWidth; i++) 
        {
            Console.Write($" {Convert.ToChar(65 + i)} ");
            widthSpecifiers.Add(Convert.ToChar(65 + i).ToString());
        }
        return widthSpecifiers;
    }

    public static void UpdateBoardState(List<CheckersPiece>? checkersPieces, Options? options)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        DrawUpperRow(options?.BoardWidth, options?.BoardHeight);
        bool currentWhite = true;
        short lastI = 0;
        for (short i = 0; i < options?.BoardHeight; i++) {
            if (i != lastI) currentWhite = !currentWhite;
            WriteVerticalNum(options, i);
            for (short j = 0; j < options.BoardWidth; j++)
            {
                if (currentWhite) { Console.BackgroundColor = ConsoleColor.White; Console.Write("   "); }
                else { UpdateBlackTiles(i, j, checkersPieces); }
                currentWhite = !currentWhite;
            }
        }
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n");
    }

    private static void UpdateBlackTiles(short y, short x, List<CheckersPiece>? checkersPieces)
    {
        bool pieceFoundOnThoseCoordinates = false;
        CheckersPiece? foundPiece = null;
        foreach (var piece in checkersPieces!)
        {
            if (piece.YCoordinate == y && piece.XCoordinate == x)
            {
                pieceFoundOnThoseCoordinates = true;
                foundPiece = piece;
            }
        }
        Console.BackgroundColor = GetNeededColor("#5F6A6A");
        if (pieceFoundOnThoseCoordinates)
        {
            if (foundPiece?.Color == EPieceColor.Black)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" ⚈ ");
                return;
            }
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ⚈ ");
        }
        else Console.Write("   ");
    }

    
}