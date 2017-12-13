using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActorUtils
{
    [RequireComponent( typeof(Actor) )]
    public class Behaviour : MonoBehaviour
    {
        public GUID entityGUID;
        
        public Actor actor;
        
        public void Initialize()
        {
            actor = GetComponent<Actor>();
        }
    }
}