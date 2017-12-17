using System;

public class Enemy : Entity
{
    private Random rand;
    
    public Enemy()
    {
        rand = new Random();
    }
    
    public Enemy(char symbol) : this()
    {

    }
    
    public override void Tick()
    {
        //base.pos += new position(rand.Next(-1,2),rand.Next(-1,2));
    }
}