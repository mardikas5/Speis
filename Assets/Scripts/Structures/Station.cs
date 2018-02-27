using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ProtoBuf;

[Serializable]
public class Station : Structure
{
    [SerializeField]
    public List<Structure> Parts;

    public override List<T> PartBehavioursOfType<T>()
    {
        List<T> returnValues = new List<T>();

        for ( int i = 0; i < Parts.Count; i++ )
        {
            List<T> behaviours = Parts[i].PartBehavioursOfType<T>();

            if (behaviours != null)
            {
                returnValues.AddRange( behaviours );
            }
        }

        return returnValues;
    }


    public override bool Initialize()
    {
        if ( !base.Initialize() )
        {
            return false;
        }

        Owner = this;
        Parts = new List<Structure>() { this };

        return true;
    }

    public bool TryDeposit( List<Storable> resources )
    {
        foreach ( Storable r in resources )
        {
            if ( TryDeposit( r ) )
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Tries to deposit a storable object in storage
    /// </summary>
    /// <param name="r"></param>
    /// <returns>True on success/returns>
    public override bool TryDeposit( Storable r )
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

    public void Register( Structure structure )
    {
        if ( Parts.FirstOrDefault( res => res == structure ) != null )
        {
            return;
        }

        if ( !structure.Initialized )
        {
            structure.Initialize();
        }

        structure.Owner = this;

        Parts.Add( structure );

        structure.OnDestroyed += () => OnPartDestroyedHandler( structure );
    }

    public void OnPartDestroyedHandler( Structure part )
    {
        Parts.Remove( part );
    }
}