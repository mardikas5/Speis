using System;
using System.Collections.Generic;
using UnityEngine;


namespace ActorUtils
{
    public enum Direction
    {
        Left = 0, // x
        Right = 1, //-x
        Up = 2, // y
        Down = 3, //-y
        Forward = 4, // z
        Back = 5  //-z
    }

    [System.Serializable]
    public class Movement : Element
    {
        public Vector3 MovementTarget = Vector3.zero;
        //This should be in local space.
        //public float[] DirectionalSpeed = new float[6] { 1, 1, 1, 1, 1, 1 };

        //Values are here mostly for debug.
        public Vector3 BoostTowards = Vector3.zero;
        public Vector3 SlowDown = Vector3.zero;
        public Vector3 currentVelocity = Vector3.zero;
        public Vector3 TimeToTarget = Vector3.zero;
        public Vector3 NeededDir = Vector3.zero;
        //Debug values end.

        public float speed = 2f;
        public float Speed( Direction dir )
        {
            return 0f;
            // return DirectionalSpeed[(int)dir];
        }

        public virtual bool hasReachedTarget( Vector3 Point )
        {
            return Vector3.Distance( Point, transform.position ) < actor.InteractionDistance;
        }

        /// <summary>
        /// Moves actor attached rigidbody wih velocity to location.
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="stopAtTarget"></param>
        /// <returns> time it will take to get to point.</returns>
        public virtual float MoveToPoint( Vector3 Point, bool stopAtTarget = true )
        {
            MovementTarget = Point;
            Rigidbody rb = actor.GetComponent<Rigidbody>();
            //Displacement.
            NeededDir = ( Point - transform.position );

            //Initial velocity.
            Vector3 CurrentDir = rb.velocity;

            //Acceleration. Make this depend on engines n local space.
            Vector3 Acceleration = new Vector3( speed, speed, speed );
            //Time to reach the target.
            TimeToTarget = TimeToPoint( NeededDir, rb.velocity, Acceleration );

            //Transform from local acceleration to world.
            //Acceleration = rb.transform.TransformDirection( Acceleration );

            //Normalized difference between current velocity and required velocity.
            BoostTowards = Vector3.zero;// = NeededDir.normalized - CurrentDir.normalized;

            for( int i = 0; i < 3; i++ )
            {
                if( NeededDir[i] > 0f )
                {
                    BoostTowards[i] = 1f;
                }
                else
                {
                    BoostTowards[i] = -1f;
                }
            }


            //Difference between accelerating and stopping at point = 2 : 1.41, i.e 1.42x longer to stop at point.
            //Final time to target location.
            float longestTimeToAxisTarget = 0f;

            //Calculate longest time to target to get final time to target.
            for( int i = 0; i < 3; i++ )
            {
                BoostTowards[i] *= Acceleration[i] * Time.fixedDeltaTime;
                SlowDown[i] = 0f;

                if( TimeToTarget[i] > longestTimeToAxisTarget )
                {
                    longestTimeToAxisTarget = TimeToTarget[i];
                }

                if( stopAtTarget )
                {
                    longestTimeToAxisTarget *= 1.42f;

                    if( !BoostTowards[i].signMatches( rb.velocity[i] ) )
                    {
                        continue;
                    }
                    if( TimeToTarget[i] < ( Mathf.Abs( rb.velocity[i] ) / Mathf.Abs( Acceleration[i] ) ) )
                    {
                        //slow down
                        SlowDown[i] = 1f;
                        BoostTowards[i] *= -1f;
                    }
                }
                // else do nothing, keep direction.
                // apply acceleration to needed direction.

            }
            //Debug.Log(BoostTowards[0] + ", " + BoostTowards[1] + ", " + BoostTowards[2]);
            rb.velocity += BoostTowards;
            currentVelocity = rb.velocity;

            return longestTimeToAxisTarget;
        }

        public Vector3 TimeToPoint( Vector3 displacement, Vector3 initVelocity, Vector3 Acceleration )
        {
            for( int i = 0; i < 3; i++ )
            {
                if( initVelocity[i].signMatches( displacement[i] ) )
                {
                    initVelocity[i] = Mathf.Abs( initVelocity[i] );
                }
                displacement[i] = Mathf.Abs( displacement[i] );
            }

            float xTime = TimeToPoint( displacement.x, initVelocity.x, Acceleration.x );
            float yTime = TimeToPoint( displacement.y, initVelocity.y, Acceleration.y );
            float zTime = TimeToPoint( displacement.z, initVelocity.z, Acceleration.z );

            return new Vector3( xTime, yTime, zTime );
        }

        //s = displacement, u = initial velocity, a = acceleration
        public float TimeToPoint( float s, float u, float a )
        {
            //quadratic 
            if( s < 0 )
            {
                s *= -1;
            }

            float a1 = .5f * a;
            float b1 = u;
            float c1 = -s;



            float[] times = quadForm( a1, b1, c1 );

            if( !float.IsNaN( times[0] ) )
            {
                if( times[0] > 0 )
                {
                    return times[0];
                }
                else
                {
                    return times[1];
                }
            }

            return 0f;
        }

        //Throw this in another file, math function.
        public static float[] quadForm( float a, float b, float c )
        {
            float preRoot = b * b - 4f * a * c;

            if( preRoot < 0f )
            {
                return new float[] { float.NaN };
            }

            float[] roots = new float[2];

            roots[0] = ( -1f * (float)Math.Sqrt( preRoot ) - b ) / ( 2.0f * a );
            roots[1] = ( 1f * (float)Math.Sqrt( preRoot ) - b ) / ( 2.0f * a );

            return roots;
        }
    }
}