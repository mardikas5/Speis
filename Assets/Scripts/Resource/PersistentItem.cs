using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ProtoBuf;

public enum ItemType
{
    Resource,
    Weapon
}

[System.Serializable]
public class PersistentItem : ScriptableObject
{
    public Sprite sprite;
    public string Name;
    public string DisplayName;

    public ItemType Type;

    public int StackSize = 256;
    public float UnitVolume = 1f;

    private PersistentItem() { }

    private PersistentItem( string Name ) : this( Name, Name )
    {

    }

    private PersistentItem( string Name, string DisplayName )
    {
        this.Name = Name;
        this.DisplayName = DisplayName;
    }

    public static PersistentItem Get( string Name )
    {
        if ( ResourceDatabase.Instance != null )
        {
            PersistentItem match = ResourceDatabase.Instance.Resources.FirstOrDefault( res => res.Name == Name );
            if ( match != null )
            {
                return match;
            }
        }

        return Resources.Load( Persistence.ResourceObjectPath + Name ) as PersistentItem;
    }

    public static PersistentItem CreateOrGet( string Name )
    {
        return CreateOrGet( Name, Name );
    }

    public static PersistentItem CreateOrGet( string Name, string DisplayName )
    {
        PersistentItem existing = Get( Name );

        if ( existing == null )
        {
            existing = new PersistentItem( Name, DisplayName );
        }

        return existing;
    }
}

