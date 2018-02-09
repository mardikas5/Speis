using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Structure : Entity
{

    //dont load this var, set it via station.
    public Station Owner;

    //prefab info
    public List<Connector> Connectors;
    public Collider PlacementCollider;
    public List<Storable> BuildingCost;

    [SerializeField]
    public List<StructureBehaviour> StructureBehaviours;

    public event Action OnDestroyed;

    public void RegisterBehaviour( StructureBehaviour sb )
    {
        if( !StructureBehaviours.Contains( sb ) )
        {
            if( !sb.Initialized )
            {
                sb.Init( this );
            }
            StructureBehaviours.Add( sb );
        }
    }

    public T AddBehaviour<T>() where T : StructureBehaviour
    {
        StructureBehaviour behaviour = this.gameObject.AddComponent<T>();
        StructureBehaviours.Add( behaviour );
        behaviour.Init( this );
        return behaviour as T;
    }

    public bool RemoveBehaviour( StructureBehaviour behaviour )
    {
        if( behaviour != null )
        {
            return StructureBehaviours.Remove( behaviour );
        }

        return false;
    }

    public override void Tick()
    {
        foreach( StructureBehaviour t in StructureBehaviours )
        {
            if( t != null )
            {
                t.Tick();
            }
        }
    }

    public override bool Initialize()
    {
        if( !base.Initialize() )
        {
            return false;
        }
        if( StructureBehaviours == null )
        {
            StructureBehaviours = new List<StructureBehaviour>();
        }
        if( Simulation.Instance != null )
        {
            Simulation.Instance.Register( this );
        }

        //maybe too far reaching.
        GetComponentsInChildren<StructureBehaviour>().ToList().ForEach( ( x ) => RegisterBehaviour( x ) );

        Connectors = new List<Connector>( transform.GetComponentsInChildren<Connector>() );

        if( Owner == null )
        {
            Owner = transform.root.GetComponentInChildren<Station>();
        }

        return true;
    }


    public static T Create<T>() where T : Structure
    {
        GameObject gameObject = GameObject.CreatePrimitive( PrimitiveType.Cube );
        return gameObject.AddComponent<T>();
    }
}