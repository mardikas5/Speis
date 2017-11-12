using System;

public static class Input
{
    public enum Direction
    {
        left = 0,
        right = 1,
        up = 2,
        down = 3
    }
    
    public static Direction KeyToDirection(ConsoleKeyInfo input)
    {
        if (input.Key == ConsoleKey.LeftArrow)
        {
            return Direction.left;
        }
        if (input.Key == ConsoleKey.RightArrow)
        {
            return Direction.right;
        }
        if (input.Key == ConsoleKey.UpArrow)
        {
            return Direction.up;
        }
        if (input.Key == ConsoleKey.DownArrow)
        {
            return Direction.down;
        }
        return Direction.up;
    }
    
    public static Position DirectionToOffset(Direction t)
    {
        Position moveTowards = new Position(0, 0);
        if (t == Direction.left)
        {
            moveTowards.col -= 1;
        }
        if (t == Direction.right)
        {
            moveTowards.col += 1;
        }
        if (t == Direction.up)
        {
            moveTowards.row -= 1;
        }
        if (t == Direction.down)
        {
            moveTowards.row += 1;
        }
        
        return moveTowards;
    }
}