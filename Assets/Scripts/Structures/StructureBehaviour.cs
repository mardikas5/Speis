using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructureBehaviour : MonoBehaviour
{
    public bool Initialized = false;

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

    public virtual void Init<T>(Structure structure) where T : StructureBehaviour
    {
        if (Initialized)
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