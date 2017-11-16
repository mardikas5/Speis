using System;

//use ID
public class Entity
{
    public Position pos = new Position(0,0);
    
    public string Name = "betts";
    public string ID = "xD";
    
    public char Symbol = 'E';
    
    public virtual void Tick()
    {
        
    }
}