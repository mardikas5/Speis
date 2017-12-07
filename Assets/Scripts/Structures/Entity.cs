using System;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    public bool Initialized = false;

    public Position pos = new Position(0,0);
    
    public string Name = "unNamedEntity";
    public string ID = "xD"; //make into GUID
    
    public virtual bool Initialize()
    {
        if (Initialized == true)
        {
            Debug.Log( "Tried to initialize already inited entity" );
            return false;
        }
        
        Initialized = true;
        return true;
    }

    public virtual void Tick()
    {
        
    }
}