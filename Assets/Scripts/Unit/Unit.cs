using System;
using UnityEngine;

    
public enum Direction
{
    Left    = 0, //x
    Right   = 1, //-x
    Up      = 2, //y
    Down    = 3, //-y
    Forward = 4, //z
    Back    = 5  //-z
}

[System.Serializable, RequireComponent(typeof(Rigidbody))]
public class Unit : Entity
{
    protected Rigidbody rb;
    
    protected float[6] BoostNextTick = new float[6]; 
    
    public float[6] DirectionalSpeed = new float[6] { 1, 1, 1, 1, 1, 1 };
    
    public List<UnitCommand> Commands = new List<UnitCommand>();
 
    public Vector3 MovementTarget = Vector3.zero;
 
    public float Speed( Direction dir )
    {
        return DirectionalSpeed[(int)dir];
    }
    
    public override bool Initialize()
    {
        if ( !base.Initialize() )
        {
            return false;
        }
        
        rb = GetComponent<Rigidbody>();
        
        return true;
    }
 
    //ToDo: 
    public override void Tick()
    {
        //BoostNextTick = new float[6] { 0, 0, 0, 0, 0, 0 }
        
        if (MoveDirection != Vector3.zero )
        {
            rb.Velocity += 
        }
        
        
    }
    
    protected virtual Vector3 MoveToPoint( Vector3 Point )
    {
        Vector3 NeededDir = ( Point - Transform.Position );
        Vector3 CurrentDir = rb.Velocity;
        
        Vector3 BoostTowards = NeededDir.normalized - CurrentDir.normalized;
        
        Vector3 BoostToDirection( BoostTowards );
        
        
        //if the vehicle is going faster than it can stop at specified point
        if (rb.Velocity.x > NeededDir.x *  )
        if (rb.Velocity.y > )
        if (rb.Velocity.z )
        rb.Velocity += 
        
        
    }
    
    
    public float TimeToTargetX( )
    {
        
    }
    
    public float TimeToTargetY( )
    {
        return 
    }
    
    public float TimeToTargetZ( )
    {
        return 
    }
    
    //s = displacement, u = initial velocity, a = acceleration
    public float TimeToPoint( float s, float u, float a )
    {
        //quadratic 
        float a1 = .5f * a;
        float b1 = u;
        float c1 = -s;

        float[] times = quadForm( a1, b1, c1 );
        
        if ( !float.IsNaN( times[0] ) )
        {
            if ( times[0] > 0 )
            {
                return times[0];
            }
            else
            {
                return times[1];
            }
        }
    }
    
    //Throw this in another file, math function.
    public static float[] quadForm(float a, float b, float c)
    {
        float preRoot = b * b - 4f * a * c;
        float[] roots = new float[2];
        if (preRoot < 0f)
        {
            return new float[] { float.NaN }
        }
        else
        {
            roots[0] = (-1f * (float)Math.Sqrt(preRoot) - b) / (2.0f * a);
            roots[1] = (1f * (float)Math.Sqrt(preRoot) - b) / (2.0f * a);
        }
        
        return roots;
    }
    
    
    public float TimeToStopInDirection()
    {
        
    }
    
    public float MoveTowards( Vector3 Dir )
    {
        
    }
    
    public Vector3 BoostToDirection( Vector3 Towards )
    {
        return Towards.normalized;
    }
    
    public bool signMatches(this float f, float o)
    {
        if (f.isNegative() && o.isNegative() || !f.isNegative() && !o.isNegative)
        {
            return true;
        }
        
        return false;
    }
    
    public bool isNegative(this float f)
    {
        if (f < 0)
        {
            return true
        }
        
        return false;
    }
}