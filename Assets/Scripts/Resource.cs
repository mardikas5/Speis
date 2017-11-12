using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ResourceBase
{
    //Sprite?
    //Id?
    
    public string Name;
    public string DisplayName;
    
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

public class Resource
{
    public readonly ResourceBase Base;
    
    public string Name {get {return Base.Name;}}
    public string DisplayName {get {return Base.DisplayName;}}
    
    public float Amount;
 
    //full copy
    public Resource Copy()
    {
        return new Resource(Base, Amount);   
    }
    
    public Resource Copy(float amount)
    {
        return new Resource(Base, amount);   
    }
 
 
    public Resource(ResourceBase Base) : this(Base, 0)
    {
        
    }
    
    
    public Resource(string Name) : this(Name, 0)
    {
        
    }
    
    
    public Resource(string Name, float Amount) : this(ResourceBase.CreateOrGet(Name), Amount)
    {

    }
    
    
    public Resource(ResourceBase Base, float Amount)
    {
        this.Base = Base;
        this.Amount = Amount;
    }
    

    public override bool Equals(Object obj)
    {
        Resource res = obj as Resource; 
        if (res == null)
            return false;
        else
            return Base == res.Base;
    }
    
    
    
    //all needed resources have to occur only once.
    public static float SmallestNormalizedAvailable(List<Resource> needed, List<Resource> available)
    {
        float smallest = 1f;
        
        if (needed == null || needed.Count == 0)
        {
            return 1f;
        }
        
        for (int i = 0; i < needed.Count; i++)
        {
            List<Resource> resources = available.Where(x => x.Equals(needed[i])).ToList();
            
            if (resources == null || resources.Count == 0)
            {
                return 0;
            }
            
            float amount = 0;
            
            for (int k = 0; k < resources.Count;k++ )
            {
                amount += resources[k].Amount;
            }
            
            float comparison = (amount / needed[i].Amount);
                
            if (smallest > comparison)
            {
                smallest = comparison;
            }
        }
        
        return smallest;
    }   
    
    
    public static Resource ListHas(List<Resource> lookIn, string lookFor)
    {
        Resource match = lookIn.FirstOrDefault(res => res.Base.Name == lookFor);
        return match;
    }
    
    public static Resource ListHas(List<Resource> lookIn, Resource lookFor)
    {
        return ListHas(lookIn, lookFor.Name);
    }
}