using System;
using System.Collections.Generic;

public class Structure : Entity
{
    public StructureTemplate Template;

    public bool Initialized = false;
    
    public List<Connector> Connections;
    
    public Station Owner;
    
    public event Action OnDestroyed;
    
    public Structure(StructureTemplate structureTemplate) 
    {
        Template = structureTemplate;

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