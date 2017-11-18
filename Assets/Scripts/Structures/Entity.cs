using System;

[System.Serializable]
public class Entity
{
    public Position pos = new Position(0,0);
    
    public string Name = "unNamedEntity";
    public string ID = "xD"; //make into GUID
    
    public virtual void Tick()
    {
        
    }
}