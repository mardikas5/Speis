using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorUtils
{
    [System.Serializable]
    public abstract class Command
    {
        public static string _missingOwner = "Actor command has no owner";
        public static string _runningNotNull = "Ran a Actor command, whilst it was already running";
        
        public CommandProcesser Owner;
        
        public string Description;
        
        public Action<Command> OnCommandComplete;
        
        public Coroutine Running;//get private set?
        
        public Command( CommandProcesser Owner )
        {
            this.Owner = Owner;
        }
        
        public virtual Coroutine Run( CommandProcesser Owner )
        {
            
            if (Running != null)
            {
                Debug.Log( _runningNotNull );
            }
            
            if ( Owner == null )
            {
                Debug.Log( _missingOwner );
                return null;
            }
            
            Running = Owner.StartCoroutine( CommandCoroutine( OnCommandComplete ) );
            return Running;
        }
        
        private IEnumerator CommandCoroutine( Action<Command> CallBack )
        {
            yield return  Owner.StartCoroutine( CommandInner() );
           
            if (CallBack != null)
            {
                CallBack( this );
            }
        }
        
        protected virtual IEnumerator CommandInner()
        {
            yield break;
        }
    }
}