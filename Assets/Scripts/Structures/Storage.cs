using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ProtoBuf;

[System.Serializable]
public class Storage : StructureBehaviour, ITransactionable
{
    [ProtoContract]
    public new class Surrogate : StructureBehaviour.Surrogate
    {
        [ProtoMember( 1 )]
        List<Storable.Surrogate> Stored;

        public Surrogate()
        {

        }

        public Surrogate( StructureBehaviour t ) : base( t )
        {

        }

        public override StructureBehaviour AddSelf( Entity parent )
        {
            reference = parent.gameObject.AddComponent<Storage>();

            return reference;
        }


        public override void Load( object dataObj )
        {
            base.Load( dataObj );

            Surrogate t = dataObj as Surrogate;

            Storage storage = reference as Storage;

            if ( storage != null )
            {
                if ( t.Stored == null )
                {
                    return;
                }

                foreach ( Storable.Surrogate r in t.Stored )
                {
                    r.Load( r );

                    storage.Stored.Add( r.referenceStorable );
                }
            }
        }

        public override void Save()
        {
            base.Save();

            Stored = new List<Storable.Surrogate>();

            foreach ( Storable r in ( (Storage)reference ).Stored )
            {
                Storable.Surrogate storedThing = new Storable.Surrogate( r );
                Stored.Add( storedThing );

                StringDebugType.Add( storedThing.ToString() );
            }
        }

        public override string debugString()
        {
            return "storage";
        }
    }



    [SerializeField]
    private List<Storable> stored;
    public List<Storable> Stored { get { return stored; } set { stored = value; } }

    public float Volume = 300;
    public float FilledVolume
    {
        get
        {
            float amt = 0f;
            stored.ForEach( ( x ) => amt += x.Amount );
            return amt;
        }
    }

    public Storage()
    {
        base._name = "Storage";
    }

    public Storage( float volume )
    {
        Volume = volume;
    }


    public override T SaveObject<T>()
    {
        return new Surrogate( this ) as T;
    }


    public override void Init( Structure structure )
    {
        if ( Initialized )
        {
            return;
        }

        base.Init( structure );

        Stored = new List<Storable>();
    }

    public Storable Get( string Name )
    {
        return Get( Name, false );
    }

    public Storable Get( string Name, bool Create = false )
    {
        if ( Stored != null )
        {
            Storable match = Storable.ListHas( Stored, Name );
            if ( match != null )
            {
                return match;
            }
        }
        if ( Create )
        {
            Storable created = new Storable( Name );
            Stored.Add( created );
            return created;
        }
        return null;
    }


    //change to return resource incase left over.
    public Storable Deposit( Storable res )
    {
        Storable inStore = Get( res.Name, res.Amount > 0 );
        if ( res.Amount < 0 )
        {
            return res;
        }
        if ( inStore != null )
        {
            if ( FilledVolume + res.Amount > Volume )
            {
                float amtDeposit = Volume - FilledVolume;
                if ( amtDeposit < 0 )
                {
                    Debug.Log( " Storage size changed? " );
                }
                res.Amount -= amtDeposit;
                inStore.Amount += amtDeposit;
            }
            else
            {
                inStore.Amount += res.Amount;
                res.Amount = 0f;
            }
        }
        return res;
    }

    public void Deposit( string Name, float Amount )
    {
        Deposit( new Storable( Name, Amount ) );
    }

    public Storable Withdraw( Storable res )
    {
        return Withdraw( res.Base.Name, res.Amount );
    }

    public Storable Withdraw( PersistentItem Base, float Amount )
    {
        return Withdraw( Base.Name, Amount );
    }

    public Storable Withdraw( string Name, float Amount )
    {
        Storable inStore = Get( Name );

        Storable outp;

        if ( inStore == null )
        {
            return null;
        }

        if ( inStore.Amount <= Amount )
        {
            outp = new Storable( inStore.Base, Amount );
            Stored.Remove( inStore );
        }
        else
        {
            outp = new Storable( inStore.Base, inStore.Amount );
        }

        inStore.Amount -= outp.Amount;

        return outp;
    }
}