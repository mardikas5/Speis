using System;
#if Console
public static class ConsoleExtentions
{
    public static void WriteBottomLine(String text)
    {
        int x = Console.CursorLeft;
        int y = Console.CursorTop;
        Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
        Console.Write(text);
        Console.WriteLine("");
        // Restore previous position
        Console.SetCursorPosition(x, y);
    }
}
#endif
