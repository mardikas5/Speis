using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;



[System.Serializable]
public class Mineable : Entity
{
    [Serializable, ProtoContract]
    public new class Surrogate : Entity.Surrogate
    {
        [ProtoMember( 1 )]
        List<Storable.Surrogate> Resources;

        public Surrogate()
        {

        }

        public Surrogate( Mineable t ) : base( t )
        {
           
        }

        public override void Save()
        {
            base.Save();

            Resources = new List<Storable.Surrogate>();

            StringDebugType.Add( "mineable" );

            foreach ( Storable r in ( (Mineable)referenceEntity ).Resources )
            {
                Storable.Surrogate butts = new Storable.Surrogate( r );
                Resources.Add( butts );
                StringDebugType.Add( butts.ToString() );
            }
        }

        public override void Load( object dataObj )
        {
            base.Load( dataObj );

            Surrogate t = dataObj as Surrogate;

            Mineable min = referenceEntity.GetComponent<Mineable>();

            if ( min != null )
            {
                if ( t.Resources == null )
                {
                    return;
                }
                foreach ( Storable.Surrogate r in t.Resources )
                {
                    r.Load( r );

                    min.Resources.Add( r.referenceStorable );
                }
            }
        }
    }

    [SerializeField]
    public List<Storable> Resources;

    public GameObject SpawnOnMined;

    public override bool Initialize()
    {
        base.Initialize();

        GetComponent<Health>().onDestroyed += OnDestroyHandler;

        return true;
    }

    public override T SaveObject<T>()
    {
        return new Surrogate( this ) as T;
    }

    void OnDestroyHandler()
    {
        foreach ( Storable r in Resources )
        {
            GameObject MinedPiece = Instantiate( SpawnOnMined, transform.position, Quaternion.identity, null );
            MinedPiece.GetComponent<Pickable>().Data.Resource = r;
            Debug.Log( MinedPiece );
        }
    }
}


