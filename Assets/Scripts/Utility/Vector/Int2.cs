using UnityEngine;
using System.Collections;
using ProtoBuf;

[System.Serializable]
[ProtoContract]
public struct Int2Rotation
{
	[ProtoMember(1)] public int xCell;
	[ProtoMember(2)] public int yCell;
	[ProtoMember(3)] public int xSign;
	[ProtoMember(4)] public int ySign;
	[ProtoMember(5)] public int N;

	public Int2Rotation( Int2 from, Int2 to )
	{
		xCell = 0;
		yCell = 1;
		xSign = 1;
		ySign = 1;
		N = 0;

		for( int i = 0; i < 4; ++i )
		{
			if( from == to ) break;
			
			N++;

			MathUtil.Swap( ref xCell, ref yCell );

			MathUtil.Swap( ref xSign, ref ySign );
			ySign = -ySign;

			MathUtil.Swap( ref from.x, ref from.y );
			from.y = -from.y;
		}
	}

	public Int2Rotation( int N )
	{
		xCell = 0;
		yCell = 1;
		xSign = 1;
		ySign = 1;
		this.N = N;

		for( int i = 0; i < N; ++i )
		{
			MathUtil.Swap( ref xCell, ref yCell );
			MathUtil.Swap( ref xSign, ref ySign );
			ySign = -ySign;
		}
	}

	public static Int2 operator*( Int2Rotation rotation, Int2 v )
	{
		return new Int2( v[rotation.xCell] * rotation.xSign, v[rotation.yCell] * rotation.ySign );
	}

	public static Int3 operator*( Int2Rotation rotation, Int3 v )
	{
		return new Int3( v[rotation.xCell * 2] * rotation.xSign, v.y, v[rotation.yCell * 2] * rotation.ySign );
	}
}

[System.Serializable]
[ProtoContract]
public struct Int2
{
    public static Int2[] Neighbors4 = { new Int2( 0, 1 ), new Int2( 1, 0 ), 
                                        new Int2( 0, -1  ), new Int2( -1, 0 ) };

	public static Int2[] Neighbors8 = {   new Int2( -1, -1 ), new Int2( 0, -1 ), new Int2( 1, -1 ), 
										  new Int2( -1, 0 ), new Int2( 1, 0 ), 
										  new Int2( -1, 1 ), new Int2( 0, 1 ), new Int2( 1, 1 ) };

    /// <summary>[0, 0]</summary>
	public static Int2 zero = new Int2( 0, 0 );
    /// <summary>[1, 1]</summary>
	public static Int2 one = new Int2( 1, 1 );
    /// <summary>[-1, 0]</summary>
    public static Int2 left = new Int2( -1, 0 );
    /// <summary>[1, 0]</summary>
    public static Int2 right = new Int2( 1, 0 );
    /// <summary>[0, 1]</summary>
    public static Int2 up = new Int2( 0, 1 );
    /// <summary>[0, -1]</summary>
    public static Int2 down = new Int2( 0, -1 );

    [ProtoMember(1)] public int x;
    [ProtoMember(2)] public int y;

	public int this[int key]
	{
		get
		{
			switch(key)
			{
			case 0: return x;
			case 1: return y;
			default: return -1;
			}
		}
		set
		{
			switch( key )
			{
			case 0: x = value; break;
			case 1: y = value; break;
			}
		}
	} 

	public Int3 x0y
	{
		get { return new Int3( x, 0, y ); }
	}

    public Int2( int _x, int _y ) 
    {
        x = _x;
        y = _y;
    }

    public Int2( int v )
    {
        x = v;
        y = v;
    }
	
	public Int2( Vector2 v )
	{ 
		x = (int)v.x;
		y = (int)v.y;
	}

	public float Magnitude()
    {
        return Mathf.Sqrt( (float)( x * x + y * y ) );
    }

	public int Area()
	{
		return x * y;
	}

    public static Int2 operator+( Int2 a, Int2 b )
    {
        return new Int2( a.x + b.x, a.y + b.y );
    }

    public static Int2 operator-( Int2 a, Int2 b )
    {
        return new Int2( a.x - b.x, a.y - b.y );
    }

	public static Int2 operator-( Int2 a )
	{
		return new Int2( -a.x, -a.y );
	}

    public static Int2 operator*( Int2 a, Int2 b )
    {
        return new Int2( a.x * b.x, a.y * b.y );
    }

    public static Int2 operator*( Int2 a, int b )
    {
        return new Int2( a.x * b, a.y * b );
    }

    public static Int2 operator/( Int2 a, Int2 b )
    {
        return new Int2( a.x / b.x, a.y / b.y );
    }

	public static Int2 operator/( Int2 a, int b )
	{
		return new Int2( a.x / b, a.y / b );
	}

	public static bool operator<( Int2 a, Int2 b )
	{
		return a.x < b.x && a.y < b.y;
	}

	public static bool operator>( Int2 a, Int2 b )
	{
		return a.x > b.x && a.y > b.y;
	}

    public static bool operator==( Int2 a, Int2 b )
    {
        if( System.Object.ReferenceEquals( a, b ) ) {
            return true;
        }
        
        if( ( (object)a == null ) || ( (object)b == null ) ) {
            return false;
        }

        return a.x == b.x && a.y == b.y;
    }

    public static bool operator!=( Int2 a, Int2 b )
    {
        return !( a == b );
    }

    public override bool Equals( object obj )
    {
		if( !( obj is Int2 ) ) {
			return false;
		}
		
        Int2 p = (Int2)obj;
		
		return ( x == p.x ) && ( y == p.y );
	}
	
	public override int GetHashCode()
	{
		return x + ( y << 16 );
	}

    public override string ToString()
    {
        return "[" + x + ", " + y + "]";
    }

    public static explicit operator Vector2( Int2 a )
    {
        return new Vector2( a.x, a.y );
    }

    public static explicit operator Vector3( Int2 a )
    {
        return new Vector3( a.x, 0, a.y );
    }

    public static Int2 Clamp( Int2 a, Int2 lower, Int2 upper ) 
    {
        return new Int2( 
            a.x < lower.x ? lower.x : a.x > upper.x ? upper.x : a.x,
            a.y < lower.y ? lower.y : a.y > upper.y ? upper.y : a.y );
    }

	public static Int2 Abs( Int2 v )
	{
		return new Int2( Mathf.Abs( v.x ), Mathf.Abs( v.y ) );
	}

    public static bool IsOutside( Int2 a, Int2 lower, Int2 upper ) 
    {
        return a.x < lower.x || a.y < lower.y || a.x >= upper.x || a.y >= upper.y;
    }

	public static Int2 Range( int lower, int upper )
	{
		return new Int2( Random.Range( lower, upper ), Random.Range( lower, upper ) );
	}

    public static Int2 Range( Int2 lower, Int2 upper )
	{
		return new Int2( Random.Range( lower.x, upper.x ), Random.Range( lower.y, upper.y ) );
	}

	public static void MakeMinMax( ref Int2 min, ref Int2 max )
	{
		if( min.x > max.x )
			MathUtil.Swap( ref min.x, ref max.x );

		if( min.y > max.y )
			MathUtil.Swap( ref min.y, ref max.y );
	}

	public static Int2 Min( Int2 a, Int2 b )
	{
		return new Int2( a.x < b.x ? a.x : b.x, a.y < b.y ? a.y : b.y );
	}

	public static Int2 Max( Int2 a, Int2 b )
	{
		return new Int2( a.x > b.x ? a.x : b.x, a.y > b.y ? a.y : b.y );
	}

	public static int Neighbors8Index( Int2 normalizedDirection )
	{
		normalizedDirection.y += 1;
		normalizedDirection.x += 1;		
		int i = normalizedDirection.y * 3 + normalizedDirection.x;
		i = i > 4 ? i - 1 : i;
		return i;
	}
}