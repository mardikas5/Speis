using System;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    public Position pos = new Position(0,0);
    
    public string Name = "unNamedEntity";
    public string ID = "xD"; //make into GUID
    
    public virtual void Tick()
    {
        
    }
}