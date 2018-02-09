using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Storable
{
    [SerializeField]
    //use as readonly
    public PersistentItem Base;

    private bool isBaseNull { get { Debug.Log( "Base of item is null" ); return Base == null; } }

    public string Name { get { return isBaseNull ? null : Base.Name; } }
    public string DisplayName { get { return isBaseNull ? null : Base.DisplayName; } }

    public float Amount;

    //full copy
    public Storable Copy()
    {
        return new Storable( Base, Amount );
    }

    public Storable Copy( float amount )
    {
        return new Storable( Base, amount );
    }


    public Storable( PersistentItem Base ) : this( Base, 0 )
    {

    }


    public Storable( string Name ) : this( Name, 0 )
    {

    }


    public Storable( string Name, float Amount ) : this( PersistentItem.CreateOrGet( Name ), Amount )
    {

    }


    public Storable( PersistentItem Base, float Amount )
    {
        this.Base = Base;
        this.Amount = Amount;
    }

    /// <summary>
    /// Name comparison is made.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( System.Object obj )
    {
        Storable res = obj as Storable;
        if( res == null )
            return false;
        else
            return Base == res.Base;
    }


    public static List<Storable> CombineInstancesOfResources( List<Storable> resources )
    {
        List<Storable> output = new List<Storable>();

        for( int i = 0; i < resources.Count; i++ )
        {
            Storable res = output.Find( x => x.Equals( resources[i] ) );

            if( res == null )
            {
                res = resources[i].Copy( 0 );
                output.Add( res );
            }

            res.Amount += resources[i].Amount;
        }

        return output;
    }

    public static Dictionary<Storable, float> NormalizedAvailableAmounts( List<Storable> needed, List<Storable> available )
    {
        Dictionary<Storable, float> availabilities = new Dictionary<Storable, float>();

        List<Storable> Needed = CombineInstancesOfResources( needed );
        List<Storable> Available = CombineInstancesOfResources( available );

        for( int i = 0; i < Needed.Count; i++ )
        {

            if( needed[i] == null || needed[i].Base == null )
            {
                continue;
            }

            Storable availRes = Available.FirstOrDefault( x => x.Equals( Needed[i] ) );

            float comparison = 0f;

            if( availRes != null )
            {
                comparison = ( availRes.Amount / Needed[i].Amount );
            }

            availabilities.Add( Needed[i].Copy( 0 ), comparison );
        }

        return availabilities;
    }

    public static float SmallestNormalizedAvailable( List<Storable> needed, List<Storable> available )
    {
        float smallest = 1f;

        Dictionary<Storable, float> avail = NormalizedAvailableAmounts( needed, available );

        foreach( KeyValuePair<Storable, float> k in avail )
        {
            if( k.Value < smallest )
            {
                smallest = k.Value;
            }
        }

        return smallest;
    }


    public static Storable ListHas( List<Storable> lookIn, string lookFor )
    {
        Storable match = lookIn.FirstOrDefault( res => res.Base.Name == lookFor );
        return match;
    }

    public static Storable ListHas( List<Storable> lookIn, Storable lookFor )
    {
        return ListHas( lookIn, lookFor.Name );
    }

    public override int GetHashCode()
    {
        return 539060726 + EqualityComparer<string>.Default.GetHashCode( Name );
    }
}