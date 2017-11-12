using System;
using System.Collections.Generic;

public class Structure : Entity
{
    public bool Initialized = false;
    
    public List<Resource> BuildingCost;
    
    public List<Connector> Connections;
    
    public Station Owner;
    
    public event Action OnDestroyed;
    
    public Structure()
    {
        Initialize();   
    }
    
    public override void Tick()
    {
        
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