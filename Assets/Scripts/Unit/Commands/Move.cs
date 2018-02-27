using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ActorUtils
{
    [Serializable]
    public class Move : Command
    {
        private Vector3 Position;

        public bool CanRun( Actor actor )
        {
            return actor.Elements.Find( ( x ) => x.GetType() == typeof( Movement ) );
        }

        public Move( CommandProcesser Owner, Vector3 position ) : base( Owner )
        {
            Position = position;
        }

        protected override IEnumerator CommandInner()
        {
            Movement runBehaviour = Owner.actor.ActiveMovement;

            while( !runBehaviour.hasReachedTarget( Position ) )
            {
                runBehaviour.MoveToPoint( Position );

                yield return new WaitForFixedUpdate();
            }

            Debug.Log("done cmd");
        }
    }
}