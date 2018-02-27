using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

[Serializable, ProtoContract]
public class GameData
{
    //Live entities
    [ProtoMember( 1 )]
    public List<ProtoBase> ProtoBaseObjects;

    //Historical / optimization purposes
    [ProtoMember( 2 )]
    public List<Entity.Surrogate> DestroyedEntities;

    public GameData()
    {
        ProtoBaseObjects = new List<ProtoBase>();

        DestroyedEntities = new List<Entity.Surrogate>();
    }

    public void LoadIntoScene()
    {
        foreach ( ProtoBase t in ProtoBaseObjects )
        {
            t.Load( t );
        }
    }
}
