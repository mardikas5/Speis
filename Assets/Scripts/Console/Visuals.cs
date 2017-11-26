using System;
using System.Collections.Generic;

#if Console
public static class Visuals
{
    public static void Draw(Position Size, Position DrawFrom)
    {
        Position CursorPos = new Position(DrawFrom.col, DrawFrom.row);
        
        int Oldx = Console.CursorLeft;
        int Oldy = Console.CursorTop;
        
        for (int y = 0; y < Size.row;y++)
        {
            Console.WriteLine("");
            Console.SetCursorPosition(0,CursorPos.col + 1 + y);
            
            for (int x = 0; x < Size.col;x++ )
            {
                Console.SetCursorPosition(x,CursorPos.col + 1 + y);
                Console.Write("x");
            }
        }
        
        Console.SetCursorPosition(Oldx, Oldy);
    }
    
    public static void DrawEntities(Position Size, Position Location, List<Entity> entities)
    {
        int Oldx = Console.CursorLeft;
        int Oldy = Console.CursorTop;
        
        ConsoleExtentions.WriteBottomLine(Location);
        
        foreach (Entity t in entities)
        {
            Position posOnScreen = t.pos - Location;
            
            //ConsoleExtentions.WriteBottomLine(t.pos.row + ", " + t.pos.col + ", " + posOnScreen.row + ", " + posOnScreen.col);
            
            if (posOnScreen.row < 0 || posOnScreen.col < 1 || posOnScreen.row >= World.Size.row || posOnScreen.col >= World.Size.col )
            {
                continue;   
            }
            
            Console.SetCursorPosition(posOnScreen.col, posOnScreen.row);
            Console.Write(t.Symbol);
            Console.SetCursorPosition(Oldx, Oldy);
        }
        
        Console.SetCursorPosition(Oldx, Oldy);
    }
}
#endif