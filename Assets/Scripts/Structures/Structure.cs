using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Structure : Entity 
{
    public bool Initialized = false;
    
    
    public Station Owner;
    

    public List<Resource> BuildingCost;
    
    [SerializeField]
    public List<StructureBehaviour> StructureBehaviours;    
    
    
    public event Action OnDestroyed;
    

    public Structure() 
    {
        MyDebug.DebugWrite("Started");
        Initialize();   
    }
    
    public T AddBehaviour<T>() where T : StructureBehaviour
    {
        StructureBehaviour behaviour = this.gameObject.AddComponent<T>();
        StructureBehaviours.Add(behaviour);
        behaviour.Init<T>(this);
        return behaviour as T;
    }
    
    public bool RemoveBehaviour(StructureBehaviour behaviour)
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
        if (StructureBehaviours == null)
        {
            StructureBehaviours = new List<StructureBehaviour>();
        }
        if (Simulation.Instance != null)
        {
            Simulation.Instance.Register(this);
        }
        Initialized = true;
    }


    public static T CreateStructureObject<T>() where T : Structure
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        return gameObject.AddComponent<T>();       
    }
}