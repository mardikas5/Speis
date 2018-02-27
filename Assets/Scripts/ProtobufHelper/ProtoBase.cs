using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using System;

[ProtoContract, Serializable,
    ProtoInclude( 1000, typeof( Entity.Surrogate ) ),
    ProtoInclude( 1001, typeof( StructureBehaviour.Surrogate ) ),
    ProtoInclude( 1002, typeof( Storable.Surrogate ) )]

public class ProtoBase
{
    [ProtoMember( 1 )]
    public string instanceGUID;

    [ProtoMember( 2 )]
    public List<string> StringDebugType;

    public ProtoBase()
    {

    }

    public virtual void Load( object dataObj )
    {
        
    }

    /// <summary>
    /// Loads latest parent object variables into the surrogate.
    /// </summary>
    public virtual void Save()
    {
        StringDebugType = new List<string>();
        StringDebugType.Add( "base only" );
    }
}
