using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Storage : StructureBehaviour, ITransactionable
{
    [SerializeField]
    private List<Resource> stored;
    public List<Resource> Stored { get { return stored; } set { stored = value; } }

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

    public override void Init( Structure structure )
    {
        if( Initialized )
        {
            return;
        }

        base.Init( structure );

        Stored = new List<Resource>();
    }

    public Resource Get( string Name )
    {
        return Get( Name, false );
    }

    public Resource Get( string Name, bool Create = false )
    {
        if( Stored != null )
        {
            Resource match = Resource.ListHas( Stored, Name );
            if( match != null )
            {
                return match;
            }
        }
        if( Create )
        {
            Resource created = new Resource( Name );
            Stored.Add( created );
            return created;
        }
        return null;
    }

    //change to return resource incase left over.
    public Resource Deposit( Resource res )
    {
        Resource inStore = Get( res.Name, res.Amount > 0 );
        if( res.Amount < 0 )
        {
            return res;
        }
        if( inStore != null )
        {
            if( FilledVolume + res.Amount > Volume )
            {
                float amtDeposit = Volume - FilledVolume;
                if( amtDeposit < 0 )
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
        Deposit( new Resource( Name, Amount ) );
    }

    public Resource Withdraw( Resource res )
    {
        return Withdraw( res.Base.Name, res.Amount );
    }

    public Resource Withdraw( ResourceBase Base, float Amount )
    {
        return Withdraw( Base.Name, Amount );
    }

    public Resource Withdraw( string Name, float Amount )
    {
        Resource inStore = Get( Name );

        Resource outp;

        if( inStore == null )
        {
            return null;
        }

        if( inStore.Amount <= Amount )
        {
            outp = new Resource( inStore.Base, Amount );
            Stored.Remove( inStore );
        }
        else
        {
            outp = new Resource( inStore.Base, inStore.Amount );
        }

        inStore.Amount -= outp.Amount;

        return outp;
    }
}