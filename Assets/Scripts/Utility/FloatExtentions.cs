using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtentions
{
    //Math functions...
    public static bool signMatches( this float f, float o )
    {
        if( ( f.isNegative() && o.isNegative() ) || ( !f.isNegative() && !o.isNegative() ) )
        {
            return true;
        }

        return false;
    }

    public static bool isNegative( this float f )
    {
        if( f < 0 )
        {
            return true;
        }

        return false;
    }
}
