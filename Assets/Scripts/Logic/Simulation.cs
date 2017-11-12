using System;
using System.Collections.Generic;

public class Simulation : Singleton<Simulation>
{
    //make simulationObjectList
    public List<Entity> EntityDB;
    
    public List<Structure> Structures;
    
    public void Initialize()
    {
        EntityDB = new List<Entity>();
        Structures = new List<Structure>();
    }
    
    //public event Action Tick;
    
    public void Register(Structure t) //More generic object
    {
        Structures.Add(t);
    }
    
    public void Tick(List<Entity> entities)
    {
        foreach (Entity t in entities)
        {
            t.Tick();
        }
        foreach (Structure t in Structures)
        {
            t.Tick();
        }
    }
}