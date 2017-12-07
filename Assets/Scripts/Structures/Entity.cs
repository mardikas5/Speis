using System;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    public bool Initialized = false;

    public Vector3 pos = Vector3.zero;
    
    public string Name = "unNamedEntity";
    public string GUID = "";
    
    public virtual bool Initialize()
    {
        if (Initialized == true)
        {
            Debug.Log( "Tried to initialize already inited entity" );
            return false;
        }
        
        if (GUID == string.null || GUID == string.empty)
        {
            GUID = System.Guid.NewGuid().ToString();
        }
        
        Initialized = true;
        return true;
    }
    
    public virtual bool Load()
    {
        
    }
    
    public virtual void Tick()
    {
        
    }
}