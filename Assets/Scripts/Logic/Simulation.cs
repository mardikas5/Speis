using System;
using System.Collections.Generic;

public class Simulation : Singleton<Simulation>
{
    //make simulationObjectList
    public List<Entity> Entities;

    public void Initialize()
    {
        Entities = new List<Entity>();
    }
   

    //public event Action Tick;
    
    public void Register(Entity t) //More generic object
    {
        MyDebug.DebugWrite(t.Name);
        Entities.Add(t);
    }

    public void Tick()
    {
        foreach (Entity t in Entities)
        {
            t.Tick();
        }
    }
}