using System;
using System.Collections.Generic;
using UnityEngine;


namespace ActorUtils
{
    [Serializable]
    public class Move : Command
    {
        private Vector3 Position;
        
        public Movement( Unit owner, Vector3 position ) : base( owner )
        {
            Position = position;
        }
        
        protected override IEnumerator CommandInner( Action CallBack )
        {
            while ( !unit.Movement.hasReachedTarget )
            {
                if (unit.MovementTarget != Position)
                {
                    unit.Movement.MoveToPoint( Position );
                }
                
                yield return new waitForFixedUpdate();
            }
        }
    }
}