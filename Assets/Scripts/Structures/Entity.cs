using System;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    public bool Initialized = false;

    public Position pos = new Position(0,0);
    
    public string Name = "unNamedEntity";
    public string ID = "xD"; //make into GUID
    
    public virtual void Initialize()
    {
        Initialized = true;
    }

    public virtual void Tick()
    {
        
    }
}