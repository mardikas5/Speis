using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

[System.Serializable, RequireComponent( typeof( Structure ) )]
public class StructureBehaviour : ProtoMono
{
    [ProtoContract,
        ProtoInclude( 2001, typeof( ResourceProducer.Surrogate ) ),
        ProtoInclude( 2002, typeof( Storage.Surrogate ) )]
    public class Surrogate : ProtoBase
    {
        public StructureBehaviour reference;

        public Surrogate()
        {

        }

        public Surrogate( StructureBehaviour t )
        {
            reference = t;
        }

        public virtual StructureBehaviour AddSelf( Entity parent)
        {
            
            //
            return new StructureBehaviour();
        }

        public override void Load( object dataObj )
        {
            base.Load( dataObj );
        }

        public override void Save()
        {
            base.Save();
        }

        public virtual string debugString()
        {
            return "structure behaviour base";
        }
    }



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
        if ( Initialized )
        {
            return;
        }

        _structure = structure;

        Initialized = true;
    }


    public override T SaveObject<T>()
    {
        return new Surrogate( this ) as T;
    }


    public virtual void Tick()
    {

    }


    private void OnDestroy()
    {
        GetComponent<Structure>().RemoveBehaviour( this );
    }


}