using UnityEngine;

public static class MathUtil
{
	public static float NormRand( int N )
	{
		float acc = 0.0f;
		for( int i = 0; i < N; ++i )
		{
			acc += Random.value;
		}
		return acc / (float)N;
	}

	public static void Swap<T>( ref T a, ref T b )
	{
		T tmp = b;
		b = a;
		a = tmp;
	}

	public static void MakeMinMax( ref Vector3 min, ref Vector3 max )
	{
		if( min.x > max.x )
			MathUtil.Swap( ref min.x, ref max.x );

		if( min.y > max.y )
			MathUtil.Swap( ref min.y, ref max.y );

		if( min.z > max.z )
			MathUtil.Swap( ref min.z, ref max.z );
	}
    
	public static Vector3 Div( Vector2 A, Vector2 B )
	{
		return new Vector3( A.x / B.x, A.y / B.y );
	}

	public static Vector3 Div( Vector3 A, Vector3 B )
	{
		return new Vector3( A.x / B.x, A.y / B.y, A.z / B.z );
	}

	public static Vector3 Mul( Vector3 A, Vector3 B )
	{
		return new Vector3( A.x * B.x, A.y * B.y, A.z * B.z );
	}

	public static Vector3 Div( float A, Vector3 B )
	{
		return new Vector3( A / B.x, A / B.y, A / B.z );
	}

    public static int Mod( int x, int m )
    {
        return ( x % m + m ) % m;
    }

    public static float Mod( float x, float m )
    {
        return ( x % m + m ) % m;
    }

    public static int Fibonacci( int N )
    {
        int previous = 1;
        int result = 1;
        for( int i = 0; i < N; i++ )
        {
            previous = result;
            result += previous;
        }
        return result;
    }
    
    public static float Pack( float p )
    {
        return (float)( ( p + 1.0f ) / 2.0f );
    }
}