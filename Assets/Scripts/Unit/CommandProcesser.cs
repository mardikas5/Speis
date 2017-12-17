using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActorUtils
{
    public class CommandProcesser : Element
    {
        public Coroutine RunningCommand = null;
        
        public List<Command> Commands = new List<Command>();
        
        public void FixedUpdate()
        {
            if (Commands != null && Commands.Count > 0 && RunningCommand == null)
            {
                Commands[0].OnCommandComplete += CommandEndHandler;
                RunningCommand = Commands[0].Run( this );
            }
        }

        public void AddCommand( Command c)
        {
            Commands.Add( c );
        }
        
        public virtual void CommandEndHandler( Command c )
        {
            //should be at index 0.
            Commands.Remove( c );
            
            if ( RunningCommand == c.Running )
            {
                RunningCommand = null;
            }
        }
    }
}