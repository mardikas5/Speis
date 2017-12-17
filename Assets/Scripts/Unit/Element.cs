using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActorUtils
{
    [RequireComponent( typeof( Actor ) )]
    public class Element : MonoBehaviour
    {
        //saving
        public Guid entityGUID;
        
        public Actor actor;
        
        public void Initialize()
        {
            actor = GetComponent<Actor>();
        }
    }
}