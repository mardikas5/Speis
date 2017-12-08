using System;
using System.Collections
using UnityEngine;

namespace UnitCommand
{
    //ToDo
    public class Move : UnitCommand
    {
        public Vector3 Target;
        
        public override IEnumerator CommandCoroutine()
        {
            yield return StartCoroutine( base.CommandCoroutine );
            
            
        }
    }
}