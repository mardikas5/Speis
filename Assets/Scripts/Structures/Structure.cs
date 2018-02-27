using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ProtoBuf;

[System.Serializable]
public class Structure : Entity, IInitialize
{
    [Serializable, ProtoContract]
    public new class Surrogate : Entity.Surrogate
    {
        [ProtoMember( 1 )]
        public List<StructureBehaviour.Surrogate> behaviours = new List<StructureBehaviour.Surrogate>();

        public Surrogate()
        {

        }


        public Surrogate( Entity t ) : base( t )
        {

        }


        public override void Save()
        {
            base.Save();

            Structure structData = referenceEntity as Structure;

            StringDebugType.Add( "Structure" );

            behaviours = new List<StructureBehaviour.Surrogate>();

            foreach ( StructureBehaviour structBehaviour in structData.StructureBehaviours )
            {
                StructureBehaviour.Surrogate surrogate = structBehaviour.SaveObject<StructureBehaviour.Surrogate>();
                surrogate.Save();
                behaviours.Add( surrogate );
                StringDebugType.Add( surrogate.debugString() );
            }
        }

        public override void Load( object dataObj )
        {
            base.Load( dataObj );

            Surrogate structData = dataObj as Surrogate;

            foreach ( StructureBehaviour b in referenceEntity.GetComponents<StructureBehaviour>() )
            {
                Destroy( b );
            }

            foreach ( StructureBehaviour.Surrogate surrogate in structData.behaviours )
            {
                if ( surrogate != null )
                {
                    ( (Structure)referenceEntity ).RegisterBehaviour( surrogate.AddSelf( referenceEntity ) );

                    surrogate.Load( surrogate );

                    DebugLines.Add( surrogate.ToString() );
                }
            }
        }
    }

    //dont load this var, set it via station.
    public Station Owner;

    //prefab info
    public List<Connector> Connectors;
    public Collider PlacementCollider;
    public List<Storable> BuildingCost;


    [SerializeField]
    public List<StructureBehaviour> StructureBehaviours;


    public void RegisterBehaviour( StructureBehaviour sb )
    {
        if ( !StructureBehaviours.Contains( sb ) )
        {
            if ( !sb.Initialized )
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
        if ( behaviour != null )
        {
            return StructureBehaviours.Remove( behaviour );
        }

        return false;
    }


    public override void Tick()
    {
        foreach ( StructureBehaviour t in StructureBehaviours )
        {
            if ( t != null )
            {
                t.Tick();
            }
        }
    }


    public override bool Initialize()
    {
        if ( !base.Initialize() )
        {
            return false;
        }
        if ( StructureBehaviours == null )
        {
            StructureBehaviours = new List<StructureBehaviour>();
        }
        if ( Simulation.Instance != null )
        {
            //Simulation.Instance.Register( this, this );
        }

        //maybe too far reaching.
        GetComponentsInChildren<StructureBehaviour>().ToList().ForEach( ( x ) => RegisterBehaviour( x ) );

        Connectors = new List<Connector>( transform.GetComponentsInChildren<Connector>() );

        if ( Owner == null )
        {
            Owner = transform.root.GetComponentInChildren<Station>();
        }

        return true;
    }


    public override void SaveObject()
    {
        Simulation.Instance.Register( SaveObject<Surrogate>() );
    }


    public override T SaveObject<T>()
    {
        return new Surrogate( this ) as T;
    }


    public virtual bool TryDeposit( Storable r )
    {
        List<Storage> Storages = PartBehavioursOfType<Storage>();

        for ( int i = 0; i < Storages.Count; i++ )
        {
            r = Storages[i].Deposit( r );
            if ( r.Amount == 0f ) // less than too?
            {
                return true;
            }
        }
        return false;
    }


    public virtual List<T> PartBehavioursOfType<T>() where T : StructureBehaviour
    {
        if ( StructureBehaviours == null )
        {
            return null;
        }

        List<T> returnValues = new List<T>();

        for ( int i = 0; i < StructureBehaviours.Count; i++ )
        {
            if ( StructureBehaviours[i].GetType() == typeof( T ) )
            {
                returnValues.Add( (T)StructureBehaviours[i] );
            }
        }

        return returnValues;
    }
}