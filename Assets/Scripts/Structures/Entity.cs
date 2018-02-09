using System;
using UnityEngine;
using System.Linq;
using ProtoBuf;



[System.Serializable, ProtoContract]
public class Entity : MonoBehaviour
{
    [ProtoContract]
    public class Surrogate
    {
        [ProtoMember( 1 )]
        public Vector3 pos;

        [ProtoMember( 2 )]
        public string name;

        [ProtoMember( 3 )]
        public string instanceGUID;

        [ProtoMember( 4 )]
        public string prefabGUID;

        public static implicit operator Entity( Surrogate data )
        {
            if( data == null ) { return null; }
            //null checks.
            return Simulation.Instance.GameData.Entities.Find( ( x ) => x.instanceGUID == data.instanceGUID );
        }

        public static implicit operator Entity.Surrogate( Entity data )
        {
            if( data == null ) { return null; }

            Surrogate s = new Surrogate();
            s.pos = data.pos;
            s.name = data.name;
            s.instanceGUID = data.instanceGUID;
            s.prefabGUID = data.prefabGUID;

            return s;
        }
    }

    [ProtoMember( 1 )]
    public Vector3 pos = Vector3.zero;

    [ProtoMember( 2 )]
    public string Name = "unNamedEntity";

    [ProtoMember( 3 )]
    public string instanceGUID;

    [ProtoMember( 4 )]
    public string prefabGUID;

    [Tooltip( "Is object initialized, use as readonly" )]
    public bool Initialized = false;


    public virtual bool Initialize()
    {
        if( Initialized == true )
        {
            Debug.Log( "Tried to initialize already inited entity" );
            return false;
        }

        if( string.IsNullOrEmpty( instanceGUID.ToString() ) )
        {
            instanceGUID = Guid.NewGuid().ToString();
        }

        Simulation.Instance.Register( this );
        d

        Initialized = true;

        return true;
    }

    public virtual void Tick()
    {

    }
}