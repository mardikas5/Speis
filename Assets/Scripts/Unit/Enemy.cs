using System;

public class Enemy : Entity
{
    private Random rand;
    
    public Enemy()
    {
        rand = new Random();
        base.Symbol = 'F';
    }
    
    public Enemy(char symbol) : this()
    {
        base.Symbol = symbol;
    }
    
    public override void Tick()
    {
        base.pos += new Position(rand.Next(-1,2),rand.Next(-1,2));
    }
}