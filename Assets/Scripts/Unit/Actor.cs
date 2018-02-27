using System;
using System.Collections.Generic;
using UnityEngine;
using ActorUtils;



[System.Serializable, RequireComponent( typeof( Rigidbody ))]
public class Actor : ProtoMono
{
    //Same as structurebehaviour but applies to actors.
    public List<ActorUtils.Element> Elements = new List<ActorUtils.Element>();

    public Movement ActiveMovement;

    protected Rigidbody rb;

    public float InteractionDistance = .5f;

    public bool isActive = true;

    public void Start()
    {
        Initialize();
    }

    public virtual bool Initialize()
    {
        //if( !base.Initialize() )
        //{
        //    return false;
        //}

        Element[] el = GetComponentsInChildren<Element>();

        foreach( Element e in el )
        {
            e.Initialize();
        }

        if( GetComponent<CommandProcesser>() != null )
        {
            GetComponent<CommandProcesser>().AddCommand( new Move( GetComponent<CommandProcesser>(), new Vector3( 15, 15, 35 ) ) );
        }

        rb = GetComponent<Rigidbody>();

        return true;
    }

    public virtual void FixedUpdate()
    {
        if( !isActive )
        {
            return;
        }

    }

    public override T SaveObject<T>()
    {
        throw new NotImplementedException();
    }
}