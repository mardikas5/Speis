using System;

public struct Position
{
        public int row;
        public int col;
        
            public Position(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
            
        public static Position operator -(Position a, Position b)
        {
            return new Position(a.row - b.row, a.col - b.col);
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.row + b.row, a.col + b.col);
        }
        
        public static implicit operator string(Position p)
        {
            return "[" + p.row + ", " + p.col + "]";
        }
}

