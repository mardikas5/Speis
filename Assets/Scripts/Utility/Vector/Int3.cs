using UnityEngine;
using System.Collections;
using ProtoBuf;


[System.Serializable]
[ProtoContract]
public struct Int3
{
	public static readonly Int3[] HorizontalNeighbors4 = { new Int3( 0, 0, 1 ), new Int3( 1, 0, 0 ),  new Int3( 0, 0, -1  ), new Int3( -1, 0, 0 ) };
	
	public static readonly Int3[] HorizontalNeighbors8 = {   new Int3( -1, 0, -1 ), new Int3( 0, 0, -1 ), new Int3( 1, 0, -1 ), 
										  new Int3( -1, 0, 0 ), new Int3( 1, 0, 0 ), 
										  new Int3( -1, 0, 1 ), new Int3( 0, 0, 1 ), new Int3( 1, 0, 1 ) };

	public static readonly Int3 up = new Int3( 0, 1, 0 );
	public static readonly Int3 down = new Int3( 0, -1, 0 );
	public static readonly Int3 right = new Int3( 1, 0, 0 );
	public static readonly Int3 left = new Int3( -1, 0, 0 );
	public static readonly Int3 forward = new Int3( 0, 0, 1 );
	public static readonly Int3 back = new Int3( 0, 0, -1 );
    public static readonly Int3 zero = new Int3( 0 );
    public static readonly Int3 one = new Int3( 1 );

    [ProtoMember(1)] public int x;
    [ProtoMember(2)] public int y;
    [ProtoMember(3)] public int z;

	public int this[int key]
	{
		get
		{
			switch(key)
			{
			case 0: return x;
			case 1: return y;
			case 2: return z;
			default: return 0;
			}
		}
		set
		{
			switch( key )
			{
			case 0: x = value; break;
			case 1: y = value; break;
			case 2: z = value; break;
			}
		}
	}
	
	public Int2 xz { get { return new Int2( x, z ); } }
	public Int3 _x00 { get { return new Int3( x, 0, 0 ); } }
	public Int3 _0y0 { get { return new Int3( 0, y, 0 ); } }
	public Int3 _00z { get { return new Int3( 0, 0, z ); } }
	public Int3 zyx { get { return new Int3( z, y, x ); } }
	public Int3 x0z { get { return new Int3( x, 0, z ); } }

	public Int3 normalized
	{
		get
		{
			return new Int3( x > 0 ? 1 : x < 0 ? -1 : 0, y > 0 ? 1 : y < 0 ? -1 : 0, z > 0 ? 1 : z < 0 ? -1 : 0 );
		}
	}

    public Int3( int _x, int _y, int _z ) 
    {
        x = _x;
        y = _y;
        z = _z;
    }


    public Int3( int v )
    {
        x = v;
        y = v;
        z = v;
    }

    public Int3( Vector3 v )
    {
        x = (int)v.x;
        y = (int)v.y;
        z = (int)v.z;
    }

    public Int3( Int3 v )
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

	public Int3( Int2 v, int y )
	{
		x = v.x;
		this.y = y;
		z = v.y;
	}

	public int Area
	{
		get
		{
			return x * y * z;
		}
	}

    public static Int3 operator+( Int3 a, Int3 b )
    {
        return new Int3( a.x + b.x, a.y + b.y, a.z + b.z );
    }

    public static Int3 operator-( Int3 a, Int3 b )
    {
        return new Int3( a.x - b.x, a.y - b.y, a.z - b.z );
    }

	public static Int3 operator-( Int3 a )
	{
		return new Int3( -a.x, -a.y, -a.z );
	}

    public static Int3 operator*( Int3 a, Int3 b )
    {
        return new Int3( a.x * b.x, a.y * b.y, a.z * b.z );
    }

    public static Int3 operator*( Int3 a, int b )
    {
        return new Int3( a.x * b, a.y * b, a.z * b );
    }

    public static Int3 operator/( Int3 a, Int3 b )
    {
        return new Int3( a.x / b.x, a.y / b.y, a.z / b.z );
    }

    public static Int3 operator/( Int3 a, int b )
    {
        return new Int3( a.x / b, a.y / b, a.z / b );
    }

    public static Int3 operator%( Int3 a, int b )
    {
        return new Int3( a.x % b, a.y % b, a.z % b );
    }
	
	public bool AllLess( Int3 b )
	{
		return x < b.x && y < b.y && z < b.z;
	}

    public bool AnyLess( int b )
    {
        return x < b || y < b || z < b;
    }

	public bool AllLessEquals( Int3 b )
	{
		return x <= b.x && y <= b.y && z <= b.z;
	}

	public bool AllGreater( Int3 b )
	{
        return x > b.x && y > b.y && z < b.z;
	}

    public bool AnyGreater( int b )
    {
        return x > b || y > b || z > b;
    }

	public bool AllGreaterEquals( Int3 b )
	{
		return x >= b.x && y >= b.y && z >= b.z;
	}

    public static bool operator==( Int3 a, Int3 b )
    {
        if( System.Object.ReferenceEquals( a, b ) ) {
            return true;
        }
        
        if( ( (object)a == null ) || ( (object)b == null ) ) {
            return false;
        }

        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator!=( Int3 a, Int3 b )
    {
        return !( a == b );
    }

    public override bool Equals( object obj )
    {
		if( !( obj is Int3 ) ) {
			return false;
		}
		
        Int3 p = (Int3)obj;		
		return ( x == p.x ) && ( y == p.y ) && ( z == p.z );
	}
	
	public override int GetHashCode()
	{
		return x + ( y << 11 ) + ( z << 22 );
	}

    public override string ToString()
    {
        return "[" + x + ", " + y + ", " + z + "]";
    }

    public static explicit operator Vector3( Int3 a )
    {
        return new Vector3( a.x, a.y, a.z );
    }
    
    public static Int3 Clamp( Int3 a, Int3 lower, Int3 upper ) 
    {
        return new Int3( 
            a.x < lower.x ? lower.x : a.x > upper.x ? upper.x : a.x,
            a.y < lower.y ? lower.y : a.y > upper.y ? upper.y : a.y,
            a.z < lower.z ? lower.z : a.z > upper.z ? upper.z : a.z );
    }

	public static Int3 Min( Int3 a, Int3 b )
	{
		return new Int3( a.x < b.x ? a.x : b.x, a.y < b.y ? a.y : b.y, a.z < b.z ? a.z : b.z );
	}

	public static Int3 Max( Int3 a, Int3 b )
	{
		return new Int3( a.x > b.x ? a.x : b.x, a.y > b.y ? a.y : b.y, a.z > b.z ? a.z : b.z );
	}

    public static bool IsOutside( Int3 a, Int3 lower, Int3 upper ) 
    {
        return a.x < lower.x || a.y < lower.y || a.z < lower.z || a.x >= upper.x || a.y >= upper.y || a.z >= upper.z;
    }

    public static Int3 Abs( Int3 v )
    {
        return new Int3( Mathf.Abs( v.x ), Mathf.Abs( v.y ), Mathf.Abs( v.z ) );
    }

    public static int GetLargestAxis( Int3 v )
    {
        Int3 abs = Abs( v );

        return abs.x > abs.y ? 
            ( abs.x > abs.z ? 0 : 2 ) :
            ( abs.y > abs.z ? 1 : 2 );
    }

    public static Int3 GetUnitAxis( int axis )
    {
        Int3 result = new Int3( 0 );
        result[axis] = 1;
        return result;
    }

	public static void MakeMinMax( ref Int3 min, ref Int3 max )
	{
		if( min.x > max.x )
			MathUtil.Swap( ref min.x, ref max.x );

		if( min.y > max.y )
			MathUtil.Swap( ref min.y, ref max.y );

		if( min.z > max.z )
			MathUtil.Swap( ref min.z, ref max.z );
	}
	
	public float Magnitude()
    {
        return Mathf.Sqrt( (float)( x * x + y * y + z * z ) );
    }

    public static Int3 Range( Int3 lower, Int3 upper )
    {
		return new Int3( Random.Range( lower.x, upper.x ), Random.Range( lower.y, upper.y ), Random.Range( lower.z, upper.z ) );       
    }
}