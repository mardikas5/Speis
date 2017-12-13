using System;
using UnityEngine;
using ActorUtils;



[System.Serializable, RequireComponent( typeof(Rigidbody) )]
public class Actor : Entity
{
    protected Rigidbody rb;
    
    public float InteractionDistance = .5f;
    
    public bool isActive = true;
    
    public override bool Initialize()
    {
        if ( !base.Initialize() )
        {
            return false;
        }
        
        rb = GetComponent<Rigidbody>();
        
        return true;
    }
    
    public virtual void FixedUpdate()
    {
        if (!isActive)
        {
            return;
        }
    }
    
    public override void Tick()
    {
        
    }
    
    
    
    //Math functions...
    public bool signMatches(this float f, float o)
    {
        if (f.isNegative() && o.isNegative() || !f.isNegative() && !o.isNegative)
        {
            return true;
        }
        
        return false;
    }
    
    public bool isNegative(this float f)
    {
        if (f < 0)
        {
            return true
        }
        
        return false;
    }
}