using System;
using System.Collections.Generic;


public class Structure : Entity
{
    public string Name;
    
    public bool Initialized = false;
    
    public Station Owner;
    
    
    public List<Resource> BuildingCost;
    
    public List<Connector> Connections;
    
    //serialize.
    public List<StructureBehaviour> StructureBehaviours;
    
    
    public event Action OnDestroyed;
    
    public Structure() 
    {
        Initialize();   
    }
    
    public override void Tick()
    {
        foreach (StructureBehaviour t in StructureBehaviours)
        {
            t.Tick();
        }
    }
    
    public virtual void Initialize()
    {
        if (Initialized)
        {
            return;
        }
        if (Simulation.Instance != null)
        {
            Simulation.Instance.Register(this);
        }
        Initialized = true;
    }
}