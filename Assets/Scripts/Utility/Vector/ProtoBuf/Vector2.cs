using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class Vector2Surrogate
{
	[ProtoMember(1)] private float x;
	[ProtoMember(2)] private float y;

	public static implicit operator Vector2( Vector2Surrogate data )
	{
		if( data == null ) return Vector2.zero;
		return new Vector2( data.x, data.y );
	}

	public static implicit operator Vector2Surrogate( Vector2 data )
	{
		Vector2Surrogate result = new Vector2Surrogate();
		result.x = data.x;
		result.y = data.y;
		return result;
	}
}
 