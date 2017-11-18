using System;
using System.Collections.Generic;


[System.Serializable]
public class Structure : Entity
{
    public bool Initialized = false;
    
    
    public Station Owner;
    
    public List<Resource> BuildingCost;
    
    
    //serialize.
    protected List<StructureBehaviour> StructureBehaviours;
    
    
    
    public event Action OnDestroyed;
    
    public Structure() 
    {
        Initialize();   
    }
    
    public bool AddBehaviour(StructureBehaviours behaviour)
    {
        if (behaviour != null)
        {
            return StructureBehaviours.Add(behaviour);
        }
        return false;
    }
    
    public bool RemoveBehaviour(StructureBehaviours behaviour)
    {
        if (behaviour != null)
        {
           return StructureBehaviours.Remove(behaviour);
        }
        return false;
    }
    
    public override void Tick()
    {
        foreach (StructureBehaviour t in StructureBehaviours)
        {
            if (t != null)
            {
                t.Tick();
            }
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