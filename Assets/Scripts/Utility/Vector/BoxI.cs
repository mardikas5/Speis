using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ProtoContract]
public struct BoxI
{    
    [ProtoMember( 1 )]
    public Int3 min;
    [ProtoMember( 2 )]
    public Int3 max;

    
    public Int3 Center
    {
        get { return min + ( max - min ) / 2; }
    }

    public Int3 Size
    {
        get { return max - min; }
    }
    
    public BoxI( Int3 _min, Int3 _max )
    {
        min = _min;
        max = _max;
        Int3.MakeMinMax( ref min, ref max );
    }

    public BoxI( RectI xz, int yLower, int yUpper )
    {
        min = new Int3( xz.min, yLower );
        max = new Int3( xz.max, yUpper );
        Int3.MakeMinMax( ref min, ref max );
    }
	
    public BoxI Overlap( BoxI other )
    {
        Int3 _min = Int3.Max( min, other.min );
        Int3 _max = Int3.Min( max, other.max );

        return new BoxI( _min, _max );
    }

    public bool Contains( Int3 test )
    {
        return test.AllGreaterEquals( min ) && test.AllLess( max );
    }

    public static explicit operator Bounds( BoxI intBounds )
	{
		Bounds bounds = new Bounds();
		bounds.min = (Vector3)intBounds.min;
		bounds.max = (Vector3)intBounds.max;
		return bounds;
	}

    public override string ToString()
    {
        return string.Format( "({0}, {1})", min, max );
    }

    public Int3 RandomPoint()
    {
        return Int3.Range( min, max );
    }
}
