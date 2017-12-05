using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Structure : Entity 
{
    public Station Owner;
    
    public List<Connector> Connectors;
    public Collider PlacementCollider;
    public List<Resource> BuildingCost;
    
    [SerializeField]
    public List<StructureBehaviour> StructureBehaviours;    
    
    public event Action OnDestroyed;
    
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
    
    public override void Initialize()
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

        Connectors = new List<Connector>(transform.GetComponentsInChildren<Connector>());

        Initialized = true;
    }


    public static T Create<T>() where T : Structure
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        return gameObject.AddComponent<T>();       
    }
}