using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActorUtils
{
    public CommandProcesser : Behaviour
    {
        public Coroutine RunningCommand = null;
        
        public List<Command> Commands = new List<Command>();
        
        public void FixedUpdate()
        {
            if (Commands != null && Commands.Length > 0)
            {
                Commands[0].OnCommandComplete += CommandEndHandler;
                RunningCommand = Commands[0].Run( this );
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
    }
}