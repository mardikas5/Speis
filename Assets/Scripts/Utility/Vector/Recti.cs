using ProtoBuf;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ProtoContract]
public class RectI
{
	[ProtoMember( 1 )]
	public Int2 min;
	[ProtoMember( 2 )]
	public Int2 max;

	public Int2 Center
	{
		get { return min + ( max - max ) / 2; }
	}
	public int xMax { get { return max.x; } }
	public int yMax { get { return max.y; } }
	public int xMin { get { return min.x; } }
	public int yMin { get { return min.y; } }

	public RectI( )
	{
	}

	public RectI( Int2 _min, Int2 _max )
	{
		this.min = _min;
		this.max = _max;
		Int2.MakeMinMax( ref min, ref max );
	}

	public RectI( int minX, int minY, int maxX, int maxY )
	{
		this.min = new Int2( minX, minY );
		this.max = new Int2( maxX, maxY );
	}

	public static RectI CreateRandom( Int2 from, Int2 to, Int2 minSize, Int2 maxSize )
	{
		Int2 min = Int2.Range( from, to - maxSize );
		Int2 max = min + Int2.Range( minSize, maxSize );

		return new RectI( min, max );
	}

	public bool Contains( Int2 position )
	{
		return position.x >= min.x && position.y >= min.y &&
			position.x < max.x && position.y < max.y;
	}

	/// <summary>
	/// confines point to the bounds.
	/// </summary>
	/// <param name="rect"></param>
	/// <param name="contain"></param>
	/// <param name="roundFunction"></param>
	/// <returns></returns>
	public Int2 Contain( Int2 contain )
	{
		Int2 value = contain;
		if ( value.x > xMax ) { value.x = xMax; }
		if ( value.y > yMax ) { value.y = yMax; }
		if ( value.x < xMin ) { value.x = xMin; }
		if ( value.y < yMin ) { value.y = yMin; }
		return value;
	}
}
