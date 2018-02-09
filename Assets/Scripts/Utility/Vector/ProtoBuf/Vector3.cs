using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class Vector3Surrogate
{
    [ProtoMember( 1 )]
    private float x;
    [ProtoMember( 2 )]
    private float y;
    [ProtoMember( 3 )]
    private float z;

    public static implicit operator Vector3( Vector3Surrogate data )
    {
        if ( data == null ) return Vector2.zero;
        return new Vector3( data.x, data.y, data.z );
    }

    public static implicit operator Vector3Surrogate( Vector3 data )
    {
        Vector3Surrogate result = new Vector3Surrogate();
        result.x = data.x;
        result.y = data.y;
        result.z = data.z;
        return result;
    }
}

public static class Vector3Extentions
{
    public static Vector3 xz( this Vector3 vector )
    {
        return new Vector3( vector.x, 0, vector.z );
    }

    public static Vector2 v2xz( this Vector3 vector )
    {
        return new Vector2( vector.x, vector.z );
    }
}

public static class Vector2Extentions
{
    public static Vector3 x0y( this Vector2 vector )
    {
        return new Vector3( vector.x, 0, vector.y );
    }
}