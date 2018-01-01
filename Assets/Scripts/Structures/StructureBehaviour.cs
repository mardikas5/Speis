using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, RequireComponent( typeof( Structure ) )]
public class StructureBehaviour : MonoBehaviour
{
    public bool Initialized = false;
    public bool Enabled = true;

    public virtual string Name { get { return _name; } set { _name = value; } }

    public virtual Structure Structure
    {
        get
        {
            return _structure;
        }
    }



    [SerializeField]
    protected string _name;

    [SerializeField]
    protected Structure _structure;

    public virtual void Start()
    {
        GetComponent<Structure>().RegisterBehaviour( this );
    }

    public virtual void Init( Structure structure )
    {
        if( Initialized )
        {
            return;
        }

        _structure = structure;

        Initialized = true;
    }

    public virtual void Tick()
    {

    }

}