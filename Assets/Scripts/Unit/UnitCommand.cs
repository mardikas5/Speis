using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public virtual class UnitCommand
{
    public static string _missingOwner = "Unit command has no owner";
    
    public Unit Owner;
    
    public string Description;
    
    public virtual void Run()
    {
        if ( Owner == null )
        {
            Debug.Log( _missingOwner );
            return;
        }
        
        Owner.StartCoroutine( CommandCoroutine );
    }
    
    public virtual IEnumerator CommandCoroutine()
    {
        
    }
}