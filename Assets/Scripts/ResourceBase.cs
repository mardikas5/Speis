using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ResourceBase : ScriptableObject 
{
    public Sprite sprite;
    public string Name;
    public string DisplayName;
    
    private ResourceBase() { }

    private ResourceBase(string Name) : this(Name, Name)
    {
        
    }
    
    private ResourceBase(string Name, string DisplayName)
    {
        this.Name = Name;
        this.DisplayName = DisplayName;
    }
    
    public static ResourceBase Get(string Name)
    {
        if (ResourceDatabase.Instance != null)
        {
            ResourceBase match = ResourceDatabase.Instance.Resources.FirstOrDefault(res => res.Name == Name);
            if (match != null)
            {
                return match;
            }
        }
        return null;
    }
    
    public static ResourceBase CreateOrGet(string Name)
    {
        return CreateOrGet(Name, Name);
    }
    
    public static ResourceBase CreateOrGet(string Name, string DisplayName)
    {
        ResourceBase existing = Get(Name);

        if (existing == null)
        {
            existing = new ResourceBase(Name, DisplayName);
        }

        return existing;
    }
}

