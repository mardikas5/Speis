using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class QuaternionSurrogate
{

    [ProtoMember( 1 )]
    private float x;
    [ProtoMember( 2 )]
    private float y;
    [ProtoMember( 3 )]
    private float z;
    [ProtoMember( 4 )]
    private float w;

    public static implicit operator Quaternion( QuaternionSurrogate data )
    {
        if ( data == null ) return Quaternion.identity;
        return new Quaternion( data.x, data.y, data.z, data.w );
    }

    public static implicit operator QuaternionSurrogate( Quaternion data )
    {
        QuaternionSurrogate result = new QuaternionSurrogate();
        result.x = data.x;
        result.y = data.y;
        result.z = data.z;
        result.w = data.w;
        return result;
    }
}
