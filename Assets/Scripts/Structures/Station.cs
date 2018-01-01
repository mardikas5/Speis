using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Station : Structure
{
    public List<Structure> Parts;

    public List<T> PartsOfType<T>() where T : StructureBehaviour
    {
        List<T> returnValues = new List<T>();

        for( int i = 0; i < Parts.Count; i++ )
        {
            if( Parts[i].StructureBehaviours == null )
            {
                continue;
            }

            for( int k = 0; k < Parts[i].StructureBehaviours.Count; k++ )
            {
                if( Parts[i].StructureBehaviours[k] == null )
                {
                    continue;
                }

                if( Parts[i].StructureBehaviours[k].GetType() == typeof( T ) )
                {
                    returnValues.Add( (T)Parts[i].StructureBehaviours[k] );
                }
            }
        }

        return returnValues;
    }


    public override bool Initialize()
    {

        if( !base.Initialize() )
        {
            return false;
        }

        Owner = this;
        Parts = new List<Structure>() { this };
        //Parts.Add( this );
        return true;
    }

    public bool TryDeposit( List<Resource> resources )
    {
        foreach( Resource r in resources )
        {
            if( TryDeposit( r ) )
            {
                return true;
            }
        }
        return false;
    }

    public bool TryDeposit( Resource r )
    {
        List<Storage> Storages = PartsOfType<Storage>();

        for( int i = 0; i < Storages.Count; i++ )
        {
            r = Storages[i].Deposit( r );
            if( r.Amount == 0f ) // less than too?
            {
                return true;
            }
        }
        return false;
    }

    public void Register( Structure structure )
    {
        if( Parts.FirstOrDefault( res => res == structure ) != null )
        {
            return;
        }

        if( !structure.Initialized )
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