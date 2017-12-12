using System;
using UnityEngine;

    
public enum Direction
{
    Left    = 0, // x
    Right   = 1, //-x
    Up      = 2, // y
    Down    = 3, //-y
    Forward = 4, // z
    Back    = 5  //-z
}

[System.Serializable, RequireComponent(typeof(Rigidbody))]
public class Unit : Entity
{
    [System.Serializable]
    public class Movement
    {
        private Unit unit;
        
        private Transform transform;
        
        private rigidbody rb;
        
        //This should be in local space.
        public float[6] DirectionalSpeed = new float[6] { 1, 1, 1, 1, 1, 1 };
        
        public Movement( Unit t )
        {
            unit = t;
            transform = t.transform;
            rb = t.rb;
        }
        
        public float Speed( Direction dir )
        {
            return DirectionalSpeed[(int)dir];
        }
        

        public virtual bool hasReachedTarget( Vector3 Point )
        {
            return Vector3.Distance(Point, transform.Position) < unit.InteractionDistance;
        }
        
        protected virtual float MoveToPoint( Vector3 Point, bool stopAtTarget = true )
        {
            //Displacement.
            Vector3 NeededDir = ( Point - Transform.Position );
            
            //Initial velocity.
            Vector3 CurrentDir = rb.Velocity;
            
            //Acceleration.
            Vector3 Acceleration = new Vector3( 1f, 1f, 1f) * Time.fixedDeltaTime;
            
            //Transform from local acceleration to world.
            Acceleration = rb.Transform.TransformDirection( Acceleration );
            
            //Normalized difference between current velocity and required velocity.
            Vector3 BoostTowards = NeededDir.normalized - CurrentDir.normalized;
            
            //Time to reach the target.
            Vector3 TimeToTarget = TimeToPoint( NeededDir, rb.Velocity, Acceleration );
            
            //Difference between accelerating and stopping at point = 2 : 1.41, i.e 1.42x longer to stop at point.
            //Final time to target location.
            Float longestTimeToAxisTarget = 0f;
            
            //Calculate longest time to target to get final time to target.
            for (int i = 0; i < TimeToTarget.Length; i++)
            {
                if ( TimeToTarget[i] > longestTimeToAxisTarget )
                {
                    longestTimeToAxisTarget = TimeToTarget[i];
                }
                
                if (stopAtTarget)
                {
                    longestTimeToAxisTarget *= 1.42f;
                    if ( TimeToTarget[i] > rb.Velocity[i] / Acceleration[i] )
                    {
                        //slow down
                        BoostTowards[i] = -BoostTowards[i];
                    }
                }
                // else do nothing, keep direction.
                // apply acceleration to needed direction.
                BoostTowards[i] *= Acceleration[i];
            }
            
            rb.Velocity += BoostTowards;
            
            return longestTimeToAxisTarget;
        }
        
        public Vector3 TimeToPoint( Vector3 displacement, Vector3 initVelocity, Vector3 Acceleration)
        {
            float xTime = TimeToPoint( displacement.x, initVelocity.x, Acceleration.x )
            float yTime = TimeToPoint( displacement.y, initVelocity.y, Acceleration.y )
            float zTime = TimeToPoint( displacement.z, initVelocity.z, Acceleration.z )
            
            Vector3 timesToTarget = new Vector3( xTime, yTime, zTime );
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
            
            if (preRoot < 0f)
            {
                return new float[] { float.NaN }
            }
    
            float[] roots = new float[2];
    
            roots[0] = (-1f * (float)Math.Sqrt(preRoot) - b) / (2.0f * a);
            roots[1] = (1f * (float)Math.Sqrt(preRoot) - b) / (2.0f * a);
            
            return roots;
        }
    }
    
    protected Rigidbody rb;
    
    public Coroutine RunningCommand = null;
    
    public float InteractionDistance = .5f;
    
    public Movement Movement;
    
    public List<UnitCommand> Commands = new List<UnitCommand>();
 
    public Vector3 MovementTarget = Vector3.zero;
    
    public bool isActive = true;
    
    public override bool Initialize()
    {
        if ( !base.Initialize() )
        {
            return false;
        }
        
        rb = GetComponent<Rigidbody>();
        
        Movement = new Movement( this );
        
        MovementTarget = transform.Position;
        
        return true;
    }
    
    public virtual void FixedUpdate()
    {
        if (!isActive)
        {
            return;
        }
        
        if ( Commands.Length > 0 && Commands[0].Running == null )
        {
            Commands[0].Owner = this;
            Commands[0].OnCommandComplete += CommandEndHandler;
            Commands[0].Run();
        }
    }
    
    public virtual void CommandEndHandler( Command c )
    {
        //should be at index 0.
        Commands.Remove(c);
        
        if ( RunningCommand == c )
        {
            RunningCommand = null;
        }
        
    }
    
    public override void Tick()
    {
        
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