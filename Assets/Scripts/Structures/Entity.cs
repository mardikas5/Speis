using System;
using UnityEngine;
using System.Linq;
using ProtoBuf;
using System.Collections.Generic;

[System.Serializable,
    RequireComponent( typeof( TrackedPrefab ) )]

public partial class Entity : ProtoMono
{
    [Serializable, ProtoContract,
        ProtoInclude( 1100, typeof( Structure.Surrogate ) ),
        ProtoInclude( 1101, typeof( Mineable.Surrogate ) )]
    public class Surrogate : ProtoBase
    {
        public Entity referenceEntity;

        [ProtoMember( 1 )]
        public Vector3 pos;

        [ProtoMember( 2 )]
        public string PrefabLoadName;

        [ProtoMember( 3 )]
        public string EntityName;

        [ProtoMember( 4 )]
        public List<string> DebugLines;


        public Surrogate()
        {

        }

        public Surrogate( Entity t )
        {
            referenceEntity = t;

            Save();
        }


        public override void Save()
        {
            base.Save();

            Entity data = referenceEntity;

            if ( data == null )
            {
                return;
            }

            pos = data.gameObject.transform.position;
            PrefabLoadName = data.GetComponent<TrackedPrefab>().ResourceName;
            EntityName = data.name;
            instanceGUID = data.instanceGUID;
            StringDebugType.Add( "Entity" );
        }


        public override void Load( object dataObj )
        {
            base.Load( dataObj );

            DebugLines = new List<string>();

            if ( !Simulation.Instance.FindEntityInScene<Entity>( ( (Surrogate)dataObj ).instanceGUID ) )
            {
                SpawnEntityFromSurrogate( (Surrogate)dataObj );
            }
        }


        public virtual GameObject SpawnEntityFromSurrogate( Surrogate data )
        {
            GameObject obj = Resources.Load( PrefabLoadName ) as GameObject;

            if ( obj == null )
            {
                return null;
            }

            GameObject self = Instantiate( obj, pos, Quaternion.identity, null );

            self.GetComponent<Entity>().instanceGUID = data.instanceGUID;

            referenceEntity = self.GetComponent<Entity>();

            return self;
        }


        public void addDebug( string s )
        {
            if ( DebugLines == null )
            {
                DebugLines = new List<string>();
            }

            DebugLines.Add( s );
        }
    }




    public Vector3 pos = Vector3.zero;

    public string Name = "unNamedEntity";

    public string instanceGUID;

    private bool Destroyed = false;


    [Tooltip( "Is object initialized, use as readonly" )]
    public bool Initialized = false;


    public event Action OnDestroyed;


    public virtual bool Initialize()
    {
        if ( Initialized == true )
        {
            Debug.Log( "Tried to initialize already inited entity" );
            return false;
        }

        if ( string.IsNullOrEmpty( instanceGUID.ToString() ) )
        {
            instanceGUID = Guid.NewGuid().ToString();
        }

        Initialized = true;

        SaveObject();

        return true;
    }

    public virtual void SaveObject()
    {
        Simulation.Instance.Register( SaveObject<Surrogate>() );
    }

    public virtual void Tick()
    {

    }

    public override T SaveObject<T>()
    {
        return new Surrogate( this ) as T;
    }

    public void OnDestroy()
    {
        if ( OnDestroyed != null )
        {
            OnDestroyed();
        }
    }
}