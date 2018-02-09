using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

[Serializable, ProtoContract]
public class GameData
{
    [ProtoMember( 1 )]
    public List<Entity> Entities;

    public GameData()
    {
        Entities = new List<Entity>();
    }

    public void LoadIntoScene()
    {
        foreach( Entity t in Entities )
        {
            GameObject p = Resources.Load("Core") as GameObject;
            
        }
    }
}
