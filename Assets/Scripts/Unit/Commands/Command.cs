using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActorUtils
{
    [System.Serializable]
    public virtual class Command
    {
        public static string _missingOwner = "Actor command has no owner";
        public static string _runningNotNull = "Ran a Actor command, whilst it was already running";
        
        public CommandProcesser Owner;
        
        public string Description;
        
        public Action<Command> OnCommandComplete;
        
        public Coroutine Running;//get private set?
        
        
        public virtual Coroutine Run( CommandProcesser Owner );
        {
            if (Running != null)
            {
                Debug.Log( _runningNotNull );
            }
            
            if ( Owner == null )
            {
                Debug.Log( _missingOwner );
                return;
            }
            
            Running = Owner.StartCoroutine( CommandCoroutine( OnCommandComplete ) );
            return Running;
        }
        
        private IEnumerator CommandCoroutine( Action<ActorCommand> CallBack )
        {
            yield return StartCoroutine( CommandInner() );
           
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